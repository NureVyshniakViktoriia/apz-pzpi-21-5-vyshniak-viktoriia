using BLL.Infrastructure.Models.DiaryNote;
using Common.Enums;

namespace BLL.Infrastructure.Models.Pet;
public class PetNotificationProfile
{
    public Guid PetId { get; set; }

    public string NickName { get; set; }

    public PetType PetType { get; set; }

    public string Breed { get; set; }

    public string Characteristics { get; set; }

    public string Illnesses { get; set; }

    public string Preferences { get; set; }

    public IEnumerable<DiaryNoteListItem> Documents { get; set; }
}
