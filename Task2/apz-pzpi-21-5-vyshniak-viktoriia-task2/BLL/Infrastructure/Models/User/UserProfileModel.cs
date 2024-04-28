using Common.Enums;

namespace BLL.Infrastructure.Models.User;
public class UserProfileModel
{
    public int UserId { get; set; }

    public Region Region { get; set; }

    public Gender Gender { get; set; }

    public string Email { get; set; }

    public string Login { get; set; }

    public DateTime RegisteredOnUtc { get; set; }

    public Role Role { get; set; }
}
