using AutoMapper;
using Common.Resources;
using DAL.Contracts;
using DAL.DbContexts;
using DAL.Infrastructure.Helpers;
using DAL.Infrastructure.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class DiaryNoteRepository : IDiaryNoteRepository
{
    private readonly DbContextBase _dbContext;
    private readonly Lazy<IMapper> _mapper;

    private readonly DbSet<DiaryNote> _diaryNotes;

    public DiaryNoteRepository(
        DbContextBase dbContext,
        Lazy<IMapper> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

        _diaryNotes = dbContext.DiaryNotes;
    }

    public void Apply(DiaryNote diaryNote)
    {
        using var scope = _dbContext.Database.BeginTransaction();
        try
        {
            var dbDiaryNote = _diaryNotes.FirstOrDefault(dn => dn.DiaryNoteId == diaryNote.DiaryNoteId);
            var isForEdit = dbDiaryNote != null;

            dbDiaryNote = _mapper.Value.Map(diaryNote, dbDiaryNote);
            if (!isForEdit)
            {
                dbDiaryNote.CreatedOnUtc = DateTime.UtcNow;
                _diaryNotes.Add(dbDiaryNote);
                _dbContext.Commit();
            }

            dbDiaryNote.LastUpdatedOnUtc = DateTime.UtcNow;
            _dbContext.Commit();
            scope.Commit();
        }
        catch (Exception ex)
        {
            scope.Rollback();
            throw new Exception(ex.Message);
        }
    }

    public void Delete(Guid diaryNoteId)
    {
        var diaryNote = _diaryNotes.FirstOrDefault(dn => dn.DiaryNoteId == diaryNoteId)
            ?? throw new ArgumentException(Resources.Get("NOTE_NOT_FOUND"));

        _diaryNotes.Remove(diaryNote);
        _dbContext.Commit();
    }

    public IQueryable<DiaryNote> GetAllByPetId(Guid petId)
    {
        return _diaryNotes.Where(dn => dn.PetId == petId);
    }

    public DiaryNote GetById(Guid diaryNoteId)
    {
        var diaryNote = _diaryNotes.FirstOrDefault(dn => dn.DiaryNoteId == diaryNoteId)
            ?? new DiaryNote();

        return diaryNote;
    }

    public DocumentDataModel GetDocument(Guid diaryNoteId)
    {
        var diaryNote = _diaryNotes.FirstOrDefault(dn =>dn.DiaryNoteId == diaryNoteId)
            ?? throw new ArgumentException(Resources.Get("NOTE_NOT_FOUND"));

        var documentModel = new DocumentDataModel { DocumentName = diaryNote.Title };
        if (diaryNote.FileBytes != null && diaryNote.FileBytes.Any())
            documentModel.FileBytes = HashHelper.Decrypt(diaryNote.FileBytes);

        return documentModel;
    }

    public void UploadDocument(byte[] documentBytes, Guid diaryNoteId)
    {
        var diaryNote = _diaryNotes.FirstOrDefault(dn => dn.DiaryNoteId == diaryNoteId)
            ?? throw new ArgumentException(Resources.Get("NOTE_NOT_FOUND"));

        diaryNote.FileBytes = HashHelper.Encrypt(documentBytes);
        _dbContext.Commit();
    }
}
