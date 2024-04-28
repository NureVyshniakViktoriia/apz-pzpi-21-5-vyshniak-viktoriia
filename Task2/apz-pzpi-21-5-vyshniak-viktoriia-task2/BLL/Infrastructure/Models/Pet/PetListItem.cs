using BLL.Infrastructure.Models.Arduino;
using Common.Enums;

namespace BLL.Infrastructure.Models.Pet;
public class PetListItem
{
    public Guid PetId { get; set; }

    public string NickName { get; set; }

    public PetType PetType { get; set; }

    public string OwnerLogin { get; set; }

    public ArduinoSettingsModel ArduinoSettings { get; set; }
}
