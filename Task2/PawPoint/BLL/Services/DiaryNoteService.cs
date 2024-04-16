using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.DiaryNote;
using DAL.UnitOfWork;
using Domain.Models;

namespace BLL.Services;
public class DiaryNoteService : IDiaryNoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public DiaryNoteService(
        IUnitOfWork unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void Apply(CreateUpdateDiaryNoteModel diaryNoteModel)
    {
        var diaryNote = _mapper.Value.Map<DiaryNote>(diaryNoteModel);
        _unitOfWork.DiaryNotes.Value.Apply(diaryNote);
    }

    public void Delete(Guid diaryNoteId)
    {
        _unitOfWork.DiaryNotes.Value.Delete(diaryNoteId);
    }

    public IEnumerable<DiaryNoteListItem> GetAllByPetId(Guid petId)
    {
        var diaryNotes = _unitOfWork.DiaryNotes.Value.GetAllByPetId(petId);
        var diaryNoteModels = _mapper.Value.Map<List<DiaryNoteListItem>>(diaryNotes);

        return diaryNoteModels;
    }

    public DiaryNoteModel GetById(Guid diaryNoteId)
    {
        var diaryNote = _unitOfWork.DiaryNotes.Value.GetById(diaryNoteId);
        var diaryNoteModel = _mapper.Value.Map<DiaryNoteModel>(diaryNote);

        return diaryNoteModel;
    }

    public DiaryNoteDocumentModel GetDocument(Guid diaryNoteId)
    {
        var documentDataModel = _unitOfWork.DiaryNotes.Value.GetDocument(diaryNoteId);
        var documentModel = _mapper.Value.Map<DiaryNoteDocumentModel>(documentDataModel);

        return documentModel;
    }

    public void UploadDocument(byte[] documentBytes, Guid diaryNoteId)
    {
        _unitOfWork.DiaryNotes.Value.UploadDocument(documentBytes, diaryNoteId);
    }
}
