﻿namespace BLL.Infrastructure.Models.Institution;
public class InstitutionListItem
{
    public int InstitutionId { get; set; }

    public string Name { get; set; }

    public RatingModel Rating { get; set; }

    public double WeightedRating { get; set; }

    public string InstitutionTypeName { get; set; }

    public string RegionName { get; set; }

    public string Logo { get; set; }
}
