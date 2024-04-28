using DAL.Contracts;

namespace DAL.UnitOfWork;
public interface IUnitOfWork
{
    Lazy<IUserRepository> Users { get; }

    Lazy<IPetRepository> Pets { get; }

    Lazy<IDiaryNoteRepository> DiaryNotes { get; }

    Lazy<IInstitutionRepository> Institutions { get; }

    Lazy<IFacilityRepository> Facilities { get; }

    Lazy<IHealthRecordRepository> HealthRecords { get; }

    Lazy<INotificationRepository> Notifications { get; }

    Lazy<IArduinoSettingsRepository> ArduinoSettings { get; }
}
