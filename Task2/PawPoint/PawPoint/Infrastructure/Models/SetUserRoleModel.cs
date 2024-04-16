using Common.Enums;

namespace WebAPI.Infrastructure.Models;
public class SetUserRoleModel
{
    public int UserId { get; set; }

    public Role Role { get; set; }
}
