using DAL.Infrastructure.Models;
using Common.Enums;
using Domain.Models;
using DAL.Infrastructure.Models.Filters;

namespace DAL.Contracts;
public interface IUserRepository
{
    IQueryable<User> GetAll(PagingModel pagingModel);

    User GetUserById(int userId);

    void UpdateUserProfileInfo(UserProfileInfo model);

    User LoginUser(string login, string password);

    void RegisterUser(RegisterUserModel model);

    void ResetPassword(string token, string newPassword);

    Guid CreateRefreshToken(int userId, int shiftInSeconds);

    User GetUserByRefreshToken(Guid refreshToken);

    User GetUserByEmail(string email);

    bool IsResetPasswordTokenValid(string token);

    string GenerateResetPasswordToken(int userId);

    void SetUserRole(int userId, Role newUserRole);

    void DoDatabaseBackup();
}
