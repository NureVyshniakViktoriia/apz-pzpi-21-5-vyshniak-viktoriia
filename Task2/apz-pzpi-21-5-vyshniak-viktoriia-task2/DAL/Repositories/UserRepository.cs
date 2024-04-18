using System.Security.Cryptography;
using AutoMapper;
using DAL.Contracts;
using DAL.DbContexts;
using DAL.Infrastructure.Extensions;
using DAL.Infrastructure.Helpers;
using DAL.Infrastructure.Models;
using Common.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Common.Resources;
using DAL.Infrastructure.Models.Filters;
using Common.Exceptions;
using Common.Configs;

namespace DAL.Repositories;
public class UserRepository : IUserRepository
{
    private const int TOKEN_LENGTH = 64;
    private const int RESET_PWD_TOKEN_LIFETIME_IN_DAYS = 1;

    private readonly DbContextBase _dbContext;
    private readonly ConnectionModel _connectionModel;
    private readonly Lazy<IMapper> _mapper;

    private readonly DbSet<User> _users;
    private readonly DbSet<UserProfile> _userProfiles;
    private readonly DbSet<RefreshToken> _refreshTokens;
    private readonly DbSet<ResetPasswordToken> _resetPasswordTokens;

    public UserRepository(
        DbContextBase dbContext,
        ConnectionModel connectionModel,
        Lazy<IMapper> mapper)
    {
        _dbContext = dbContext;
        _connectionModel = connectionModel;
        _mapper = mapper;

        _users = dbContext.Users;
        _userProfiles = dbContext.UserProfiles;
        _refreshTokens = dbContext.RefreshTokens;
        _resetPasswordTokens = dbContext.ResetPasswordTokens;
    }

    public Guid CreateRefreshToken(int userId, int shiftInSeconds)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var user = _users.FirstOrDefault(x => x.UserId == userId)
                ?? throw new ArgumentException(Resources.Get("USER_NOT_FOUND"));

            _refreshTokens.RemoveRange(
                _refreshTokens.Where(x => x.UserId == userId)
            );

            _dbContext.Commit();

            var refreshToken = new RefreshToken()
            {
                UserId = userId,
                ExpiresOnUtc = DateTime.UtcNow.AddSeconds(shiftInSeconds)
            };

            _refreshTokens.Add(refreshToken);
            _dbContext.Commit();

            scope.Commit();

            return refreshToken.RefreshTokenId;
        }
        catch(Exception ex)
        {
            scope.Rollback();
            throw new Exception(ex.Message);
        }
    }

    public IQueryable<User> GetAll(PagingModel pagingModel)
    {
        return _users.Include(u => u.UserProfile)
            .AsQueryable()
            .OrderBy(x => x.RegisteredOnUtc)
            .GetPage(pagingModel);
    }

    public User GetUserById(int userId)
    {
        var user = _users
            .Include(u => u.UserProfile)
            .FirstOrDefault(u => u.UserId == userId)
                ?? throw new ArgumentException(Resources.Get("USER_NOT_FOUND"));

        return user;
    }

    public User GetUserByRefreshToken(Guid refreshToken)
    {
        var token = _refreshTokens.FirstOrDefault(x =>
            x.RefreshTokenId == refreshToken
            && x.ExpiresOnUtc >= DateTime.UtcNow
        ) ?? throw new InvalidRefreshTokenException(Resources.Get("INVALID_REFRESH_TOKEN"));

        var user = _users.FirstOrDefault(x => x.UserId == token.UserId)
            ?? throw new ArgumentException(Resources.Get("USER_NOT_FOUND"));

        return user;
    }

    public User LoginUser(string login, string password)
    {
        var user = _users.FirstOrDefault(x => x.Login == login
            || x.UserProfile.Email == login);

        if (user is null
            || !HashHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
        {
            throw new UnauthorizedAccessException();
        }

        return user;
    }

    public void RegisterUser(RegisterUserModel model)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            if (_users.Any(x => x.Login == model.Login))
                throw new ArgumentException(Resources.Get("LOGIN_EXISTS"));

            if (_users.Any(x => x.UserProfile.Email == model.Email))
                throw new ArgumentException(Resources.Get("EMAIL_EXISTS"));

            var (salt, passwordHash) = HashHelper.GenerateNewPasswordHash(model.Password);
            var user = _mapper.Value.Map<User>(model);
            var userProfile = _mapper.Value.Map<UserProfile>(model);

            user.PasswordSalt = salt;
            user.PasswordHash = passwordHash;
            user.RegisteredOnUtc = DateTime.UtcNow;
            user.UserProfile = userProfile;

            _users.Add(user);
            _dbContext.Commit();

            scope.Commit();
        }
        catch(Exception ex)
        {
            scope.Rollback();
            throw new Exception(ex.Message);
        }
    }

    public void ResetPassword(string token, string newPassword)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var user = _users.Include(u => u.ResetPasswordTokens)
                .FirstOrDefault(u => u.ResetPasswordTokens
                .Any(t => t.Token == token
                    && t.ExpiresOnUtc >= DateTime.UtcNow))
                ?? throw new ArgumentException(Resources.Get("INVALID_RESET_PASSWORD_TOKEN"));

            var (salt, passwordHash) = HashHelper.GenerateNewPasswordHash(newPassword);
            user.PasswordSalt = salt;
            user.PasswordHash = passwordHash;
            user.ResetPasswordTokens.Clear();

            _dbContext.Commit();

            scope.Commit();
        }
        catch(Exception ex)
        {
            scope.Rollback();
            throw new Exception(ex.Message);
        }
    }

    public void UpdateUserProfileInfo(UserProfileInfo model)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var profile = _userProfiles.FirstOrDefault(p => p.UserId == model.UserId)
                ?? throw new ArgumentException(Resources.Get("USER_NOT_FOUND"));

            _mapper.Value.Map(model, profile);
            _dbContext.Commit();

            scope.Commit();
        }
        catch(Exception ex)
        {
            scope.Rollback();
            throw new Exception(ex.Message);
        }
    }

    public User GetUserByEmail(string email)
    {
        var userProfile = _userProfiles
            .Include(p => p.User)
            .FirstOrDefault(p => p.Email == email);

        if (userProfile == null)
            throw new ArgumentException(Resources.Get("USER_WITH_EMAIL_NOT_FOUND"));

        return userProfile.User;
    }

    public bool IsResetPasswordTokenValid(string token)
    {
        var resetPasswordToken = _resetPasswordTokens.FirstOrDefault(x => x.Token == token
            && x.ExpiresOnUtc >= DateTime.UtcNow);

        return resetPasswordToken != null;
    }

    public string GenerateResetPasswordToken(int userId)
    {
        var scope = _dbContext.Database.BeginTransaction();
        try
        {
            _resetPasswordTokens.RemoveRange(
                _resetPasswordTokens.Where(t => t.UserId == userId));
            _dbContext.Commit();

            var token = CreateRandomToken();
            var resetPasswordToken = new ResetPasswordToken()
            {
                Token = token,
                UserId = userId,
                ExpiresOnUtc = DateTime.UtcNow.AddDays(RESET_PWD_TOKEN_LIFETIME_IN_DAYS),
            };

            _resetPasswordTokens.Add(resetPasswordToken);
            _dbContext.Commit();
            scope.Commit();

            return token;
        }
        catch(Exception)
        {
            scope.Rollback();
            throw;
        }
    }

    public void SetUserRole(int userId, Role newUserRole)
    {
        var user = GetUserById(userId);
        user.Role = newUserRole;

        _dbContext.Commit();
    }

    private string CreateRandomToken()
    {
        var newToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(TOKEN_LENGTH));
        while (_resetPasswordTokens.Any(t => t.Token == newToken))
            newToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(TOKEN_LENGTH));

        return newToken;
    }

    public void DoDatabaseBackup()
    {
        var dbName = _connectionModel.Database;
        var query = @$"BACKUP DATABASE {dbName} TO DISK = 'C:\DBBackups\{dbName}_Full.bak'";
        _dbContext.Database.ExecuteSqlRaw(query);
    }
}
