using DAL.Infrastructure.Models;
using Common.Enums;
using BLL.Infrastructure.Models.User;
using DAL.Infrastructure.Models.Filters;

namespace BLL.Contracts;
public interface IUserService
{
    UserProfileModel GetUserProfileById(int userId);

    IEnumerable<UserProfileModel> GetAllUsers(PagingModel pagingModel);

    void UpdateUserInfo(UserProfileInfo model);

    UserModel LoginUser(string login, string password);

    void RegisterUser(RegisterUserModel model);

    void ResetPassword(ResetPasswordModel resetPasswordModel);

    Guid CreateRefreshToken(int userId);

    UserModel GetUserByRefreshToken(Guid refreshToken);

    bool IsResetPasswordTokenValid(string token);

    void SetUserRole(int userId, Role newUserRole);

    void DoDatabaseBackup();
}
