using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.DiaryNote;
using BLL.Infrastructure.Models.HealthRecord;
using Common.Enums;

namespace BLL.Infrastructure.Models.Pet;
public class PetModel
{
    public Guid PetId { get; set; }

    public int OwnerId { get; set; }

    public string NickName { get; set; }

    public PetType PetType { get; set; }

    public DateTime BirthDate { get; set; }

    public string Breed { get; set; }

    public double Weight { get; set; }

    public double Height { get; set; }

    public string Characteristics { get; set; }

    public string Illnesses { get; set; }

    public string Preferences { get; set; }

    public string RFID { get; set; }

    public IEnumerable<HealthRecordModel> HealthRecords { get; set; }

    public IEnumerable<DiaryNoteListItem> DiaryNotes { get; set; }

    public ArduinoSettingsModel ArduinoSettings { get; set; }
}
