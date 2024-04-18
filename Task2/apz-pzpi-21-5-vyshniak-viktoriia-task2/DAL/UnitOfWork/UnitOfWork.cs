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
        Lazy<INotificationRepository> notifications)
    {
        Users = users;
        Pets = pets;
        DiaryNotes = diaryNotes;
        Institutions = institutions;
        Facilities = facilities;
        HealthRecords = healthRecords;
        Notifications = notifications;
    }

    public Lazy<IUserRepository> Users { get; }

    public Lazy<IPetRepository> Pets { get; }

    public Lazy<IDiaryNoteRepository> DiaryNotes { get; }

    public Lazy<IInstitutionRepository> Institutions { get; }

    public Lazy<IFacilityRepository> Facilities { get; }

    public Lazy<IHealthRecordRepository> HealthRecords { get; }

    public Lazy<INotificationRepository> Notifications { get; }
}
