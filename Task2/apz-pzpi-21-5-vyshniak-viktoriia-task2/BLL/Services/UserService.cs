using AutoMapper;
using BLL.Contracts;
using Common.Configs;
using DAL.Infrastructure.Models;
using Common.Enums;
using BLL.Infrastructure.Models.User;
using DAL.Infrastructure.Models.Filters;
using DAL.UnitOfWork;

namespace BLL.Services;
public class UserService : IUserService
{
    private readonly AuthOptions _authOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public UserService(
        AuthOptions authOptions,
        IUnitOfWork unitOfWork,
        Lazy<IMapper> mapper)
    {
        _authOptions = authOptions;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Guid CreateRefreshToken(int userId)
    {
        return _unitOfWork.Users.Value.CreateRefreshToken(
            userId,
            _authOptions.RefreshTokenLifetime
        );
    }

    public IEnumerable<UserProfileModel> GetAllUsers(PagingModel pagingModel)
    {
        var users = _unitOfWork.Users.Value
            .GetAll(pagingModel)
            .ToList();

        var userProfiles = _mapper.Value.Map<List<UserProfileModel>>(users);

        return userProfiles;
    }

    public UserModel GetUserByRefreshToken(Guid refreshToken)
    {
        var user = _unitOfWork.Users.Value.GetUserByRefreshToken(refreshToken);
        var userModel = _mapper.Value.Map<UserModel>(user);

        return userModel;
    }

    public UserProfileModel GetUserProfileById(int userId)
    {
        var user = _unitOfWork.Users.Value.GetUserById(userId);
        var userProfileModel = _mapper.Value.Map<UserProfileModel>(user);

        return userProfileModel;
    }

    public UserModel LoginUser(string login, string password)
    {
        var user = _unitOfWork.Users.Value.LoginUser(login, password);
        var userModel = _mapper.Value.Map<UserModel>(user);

        return userModel;
    }

    public void RegisterUser(RegisterUserModel model)
    {
        _unitOfWork.Users.Value.RegisterUser(model);
    }

    public void ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        _unitOfWork.Users.Value.ResetPassword(
            resetPasswordModel.Token,
            resetPasswordModel.Password);
    }

    public void UpdateUserInfo(UserProfileInfo profileInfo)
    {
        _unitOfWork.Users.Value.UpdateUserProfileInfo(profileInfo);
    }

    public bool IsResetPasswordTokenValid(string token)
    {
        return _unitOfWork.Users.Value.IsResetPasswordTokenValid(token);
    }

    public void SetUserRole(int userId, Role newUserRole)
    {
        _unitOfWork.Users.Value.SetUserRole(userId, newUserRole);
    }

    public void DoDatabaseBackup()
    {
        _unitOfWork.Users.Value.DoDatabaseBackup();
    }
}
