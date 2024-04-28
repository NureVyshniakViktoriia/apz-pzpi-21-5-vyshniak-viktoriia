using Common.Enums;

namespace BLL.Infrastructure.Models.User;
public class UserModel
{
    public int UserId { get; set; }

    public Role Role { get; set; }

    public string Login { get; set; }

    public DateTime RegisteredOnUtc { get; set; }
}
