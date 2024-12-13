using AutoMapper;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;

namespace Recsite_Ats.Infrastructure.Mappings;
public class EntitiesToDto : Profile
{
    public EntitiesToDto()
    {
        CreateMap<CustomField, CustomFieldsDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.TableName))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.FieldName))
            .ForMember(dest => dest.CustomFieldTypeId, opt => opt.MapFrom(src => src.FieldTypeId))
            .ForMember(dest => dest.CustomFieldTypeName, opt => opt.MapFrom(src => src.FieldType.FieldTypeName))
            .ReverseMap();

        CreateMap<CustomFieldViewModel, FieldLayout>()
            .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => src.SortOrder))
            .ForMember(dest => dest.Required, opt => opt.MapFrom(src => src.IsRequired))
            .ForMember(dest => dest.Visible, opt => opt.MapFrom(src => src.IsVisible))
            .ForMember(dest => dest.IsCustomField, opt => opt.MapFrom(src => src.IsCustomField))
            .ForMember(dest => dest.SectionLayoutId, opt => opt.MapFrom(src => src.SectionLayoutId))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.TableName))
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.FieldName))
            .ReverseMap();

        CreateMap<SectionLayout, SectionDTO>()
            .ForMember(dest => dest.SectionLayoutId, opt => opt.MapFrom(src => src.SectionLayoutId))
            .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.TableName))
            .ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.SectionName))
            .ForMember(dest => dest.IsCustomSection, opt => opt.MapFrom(src => src.IsCustomSection))
            .ForMember(dest => dest.Visible, opt => opt.MapFrom(src => src.Visible))
            .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => src.Sort))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ReverseMap();
    }

}
