namespace BLL.Infrastructure.Models.Institution;
public class RatingModel
{
    public int InstitutionId { get; set; }

    public bool IsSetByCurrentUser { get; set; }

    public double Mark {  get; set; }
}
