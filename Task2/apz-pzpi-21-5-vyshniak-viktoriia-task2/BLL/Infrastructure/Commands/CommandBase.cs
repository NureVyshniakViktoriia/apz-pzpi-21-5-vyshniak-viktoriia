using Common.Enums;

namespace BLL.Infrastructure.Commands;
public class CommandBase
{
    public CommandType CommandType { get; set; }

    public string Locale { get; set; } = "uk-UA";
}
