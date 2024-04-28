using AutoMapper;
using BLL.Infrastructure.Helpers;
using BLL.Infrastructure.Models.DiaryNote;
using DAL.Infrastructure.Models;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class DiaryNoteProfile : Profile
{
    public DiaryNoteProfile()
    {
        CreateMap<DiaryNote, DiaryNoteModel>()
            .ReverseMap();

        CreateMap<DiaryNote, DiaryNoteListItem>();

        CreateMap<DocumentDataModel, DiaryNoteDocumentModel>()
            .ForMember(dest => dest.Document,
                opt => opt.MapFrom(src => PdfHelper.GetPdfMemoryStreamFromBytes(src.FileBytes)));

        CreateMap<CreateUpdateDiaryNoteModel, DiaryNote>()
            .ForMember(dest => dest.CreatedOnUtc,
                opt => opt.Ignore());
    }
}
