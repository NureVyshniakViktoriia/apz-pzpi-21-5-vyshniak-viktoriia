using DAL.Infrastructure.Models;
using Domain.Models;

namespace DAL.Contracts;
public interface IDiaryNoteRepository
{
    void Delete(Guid diaryNoteId);

    void Apply(DiaryNote diaryNote);

    DiaryNote GetById(Guid diaryNoteId);

    IQueryable<DiaryNote> GetAllByPetId(Guid petId);

    void UploadDocument(byte[] documentBytes, Guid diaryNoteId);

    DocumentDataModel GetDocument(Guid diaryNoteId);
}
