using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.Facility;
using Common.Enums;

namespace BLL.Infrastructure.Models.Institution;
public class InstitutionModel
{
    public int InstitutionId { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public InstitutionType InstitutionType { get; set; } = InstitutionType.Cafe;

    public string Logo { get; set; }

    public RatingModel Rating { get; set; }

    public Region Region { get; set; }

    public string WebsiteUrl { get; set; }

    public IEnumerable<FacilityListItem> Facilities { get; set; }

    public RFIDSettingsModel RFIDSettings { get; set; }
}
