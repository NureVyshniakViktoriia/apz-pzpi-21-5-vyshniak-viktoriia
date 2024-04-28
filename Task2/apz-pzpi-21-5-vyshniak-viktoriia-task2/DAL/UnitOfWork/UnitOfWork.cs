using DAL.Contracts;

namespace DAL.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(
        Lazy<IUserRepository> users,
        Lazy<IPetRepository> pets,
        Lazy<IDiaryNoteRepository> diaryNotes,
        Lazy<IInstitutionRepository> institutions,
        Lazy<IFacilityRepository> facilities,
        Lazy<IHealthRecordRepository> healthRecords,
        Lazy<INotificationRepository> notifications,
        Lazy<IArduinoSettingsRepository> arduinoSettings)
    {
        Users = users;
        Pets = pets;
        DiaryNotes = diaryNotes;
        Institutions = institutions;
        Facilities = facilities;
        HealthRecords = healthRecords;
        Notifications = notifications;
        ArduinoSettings = arduinoSettings;
    }

    public Lazy<IUserRepository> Users { get; }

    public Lazy<IPetRepository> Pets { get; }

    public Lazy<IDiaryNoteRepository> DiaryNotes { get; }

    public Lazy<IInstitutionRepository> Institutions { get; }

    public Lazy<IFacilityRepository> Facilities { get; }

    public Lazy<IHealthRecordRepository> HealthRecords { get; }

    public Lazy<INotificationRepository> Notifications { get; }

    public Lazy<IArduinoSettingsRepository> ArduinoSettings { get; }
}
