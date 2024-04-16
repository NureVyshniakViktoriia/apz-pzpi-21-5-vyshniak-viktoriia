namespace BLL.Contracts;
public interface IEmailService
{
    void SendResetPasswordEmail(string email);
}
