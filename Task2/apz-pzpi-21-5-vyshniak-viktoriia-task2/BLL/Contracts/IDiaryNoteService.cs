using BLL.Infrastructure.Models.DiaryNote;

namespace BLL.Contracts;
public interface IDiaryNoteService
{
    void Delete(Guid diaryNoteId);

    void Apply(CreateUpdateDiaryNoteModel diaryNoteModel);

    DiaryNoteModel GetById(Guid diaryNoteId);

    IEnumerable<DiaryNoteListItem> GetAllByPetId(Guid petId);

    void UploadDocument(byte[] documentBytes, Guid diaryNoteId);

    DiaryNoteDocumentModel GetDocument(Guid diaryNoteId);
}
