using BLL.Infrastructure.Models.Arduino;

namespace BLL.Infrastructure.Commands;
public class ConfigureCommand : CommandBase
{
    public WifiSettingsModel WifiSettings { get; set; }
}
