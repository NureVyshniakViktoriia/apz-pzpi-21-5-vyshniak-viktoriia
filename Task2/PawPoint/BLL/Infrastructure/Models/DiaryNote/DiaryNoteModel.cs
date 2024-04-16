﻿namespace BLL.Infrastructure.Models.DiaryNote;
public class DiaryNoteModel
{
    public Guid DiaryNoteId { get; set; }

    public Guid PetId { get; set; }

    public string Title { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastUpdatedOnUtc { get; set; }
}
