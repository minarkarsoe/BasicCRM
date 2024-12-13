using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IAdminTaxonomyBusinessLogic
{
    Task<List<NoteTypeDto>> GetNoteTypes();
    Task UpdateNoteType(List<NoteTypeDto> noteTypeDtos);
    Task DeleteNoteType(NoteTypeDto noteTypeDto);
    Task<List<JobStatusDto>> GetJobStatus();
    Task UpdateJobStatus(List<JobStatusDto> jobStatusDto);
    Task DeleteJobStatus(JobStatusDto request);
    Task<List<DocumentTypeDto>> GetDocumentTypes();
    Task UpdateDocumentTypes(List<DocumentTypeDto> documentTypes);
    Task DeletDocumentType(DocumentTypeDto request);
    Task<List<ContactStageDto>> GetContactStages();
    Task UpdateContactStages(List<ContactStageDto> contactStageDtos);
    Task DeleteContactStage(ContactStageDto request);
    Task<List<CandidateSourceDto>> GetCandidateSources();
    Task UpdateCandidateSources(List<CandidateSourceDto> candidateSources);
    Task DeletCandidateSource(CandidateSourceDto request);
    Task<List<JobCategoryDto>> GetJobCategories();
    Task UpdateJobCategories(List<JobCategoryDto> jobCategories);
    Task DeleteJobCategory(JobCategoryDto request);
    Task DeleteJobSubCategory(JobSubCategoryDto request);
    Task<List<JobLocationDto>> GetJoblocations();
    Task UpdateJobLocations(List<JobLocationDto> jobLocations);
    Task DeleteJobLocation(JobLocationDto request);
    Task DeleteJobSubLocation(JobSubLocationDto request);

}
