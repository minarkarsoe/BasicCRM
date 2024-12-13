using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;

namespace Recsite_Ats.Infrastructure.BusinessLogic;
public class AdminTaxonomyBusinessLogic : IAdminTaxonomyBusinessLogic
{
    private readonly IServices _services;
    private readonly IUnitOfWork _unitOfWork;
    public AdminTaxonomyBusinessLogic(IUnitOfWork unitOfWork, IServices services)
    {
        _unitOfWork = unitOfWork;
        _services = services;
    }
    public async Task<List<NoteTypeDto>> GetNoteTypes()
    {
        var noteTypes = await _unitOfWork.NoteType.GetAll();

        List<NoteTypeDto> notes = new List<NoteTypeDto>();
        foreach (var noteType in noteTypes)
        {
            var note = new NoteTypeDto()
            {
                Id = noteType.Id,
                Name = noteType.Name,
                IsCustomized = noteType.IsCustomize,
                IsDefault = noteType.IsDefault,
            };
            notes.Add(note);
        }
        return notes;
    }

    public async Task UpdateNoteType(List<NoteTypeDto> noteTypeDtos)
    {
        List<NoteType> newNotes = new List<NoteType>();
        List<NoteType> updateNotes = new List<NoteType>();
        foreach (var noteType in noteTypeDtos)
        {
            var note = await _unitOfWork.NoteType.Get(x => x.Id == noteType.Id);
            if (note == null)
            {
                NoteType newNote = new NoteType()
                {
                    Name = noteType.Name,
                    IsDefault = noteType.IsDefault,
                    IsCustomize = true,
                };
                newNotes.Add(newNote);
            }
            else
            {
                note.IsDefault = noteType.IsDefault;
                note.Name = noteType.Name;
                updateNotes.Add(note);
            }
        }
        if (newNotes.Count > 0)
        {
            await _unitOfWork.NoteType.BulkAdd(newNotes);
            await _unitOfWork.Save();
        }
        if (updateNotes.Count > 0)
        {
            await _unitOfWork.NoteType.BulkUpdate(updateNotes);
            await _unitOfWork.Save();
        }
    }

    public async Task DeleteNoteType(NoteTypeDto noteType)
    {
        var deleteNote = await _unitOfWork.NoteType.Get(x => x.Id == noteType.Id);
        if (deleteNote != null)
        {
            _unitOfWork.NoteType.Remove(deleteNote);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Note Type to delete.");
        }
    }


    public async Task<List<JobStatusDto>> GetJobStatus()
    {
        var jobStatuses = await _unitOfWork.JobStatus.GetAll();

        List<JobStatusDto> statues = new List<JobStatusDto>();
        foreach (var jobStatus in jobStatuses)
        {
            var status = new JobStatusDto()
            {
                Id = jobStatus.Id,
                Name = jobStatus.Name,
                IsCustomized = jobStatus.IsCustomized,
                Sort = jobStatus.Sort,
            };
            statues.Add(status);
        }
        return statues.OrderBy(x => x.Sort).ToList();
    }
    public async Task UpdateJobStatus(List<JobStatusDto> jobStatuses)
    {
        List<JobStatus> newJobStatus = new List<JobStatus>();
        List<JobStatus> updateStatus = new List<JobStatus>();
        foreach (var jobStatus in jobStatuses)
        {
            var status = await _unitOfWork.JobStatus.Get(x => x.Id == jobStatus.Id);
            if (status == null)
            {
                JobStatus newStatus = new JobStatus()
                {
                    Name = jobStatus.Name,
                    Sort = jobStatus.Sort,
                    IsCustomized = true,
                };
                newJobStatus.Add(newStatus);
            }
            else
            {
                status.Name = jobStatus.Name;
                status.Sort = jobStatus.Sort;
                updateStatus.Add(status);
            }
        }
        if (newJobStatus.Count > 0)
        {
            await _unitOfWork.JobStatus.BulkAdd(newJobStatus);
            await _unitOfWork.Save();
        }
        if (updateStatus.Count > 0)
        {
            await _unitOfWork.JobStatus.BulkUpdate(updateStatus);
            await _unitOfWork.Save();
        }
    }
    public async Task DeleteJobStatus(JobStatusDto request)
    {
        var deleteJobStatus = await _unitOfWork.JobStatus.Get(x => x.Id == request.Id);
        if (deleteJobStatus != null)
        {
            _unitOfWork.JobStatus.Remove(deleteJobStatus);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Job Status to delete.");
        }
    }

    public async Task<List<DocumentTypeDto>> GetDocumentTypes()
    {
        var documentTypes = await _unitOfWork.DocumentType.GetAll();

        List<DocumentTypeDto> docTypes = new List<DocumentTypeDto>();
        foreach (var documentType in documentTypes)
        {
            var docType = new DocumentTypeDto()
            {
                Id = documentType.Id,
                Name = documentType.Name,
                IsCustomized = documentType.IsCustomized,
            };
            docTypes.Add(docType);
        }
        return docTypes;
    }
    public async Task UpdateDocumentTypes(List<DocumentTypeDto> documentTypes)
    {
        List<DocumentType> newDocTypes = new List<DocumentType>();
        List<DocumentType> updateDocTypes = new List<DocumentType>();
        foreach (var docTyoe in documentTypes)
        {
            var documentType = await _unitOfWork.DocumentType.Get(x => x.Id == docTyoe.Id);
            if (documentType == null)
            {
                DocumentType newDocType = new DocumentType()
                {
                    Name = docTyoe.Name,
                    IsCustomized = true,
                };
                newDocTypes.Add(newDocType);
            }
            else
            {
                documentType.Name = docTyoe.Name;
                updateDocTypes.Add(documentType);
            }
        }
        if (newDocTypes.Count > 0)
        {
            await _unitOfWork.DocumentType.BulkAdd(newDocTypes);
            await _unitOfWork.Save();
        }
        if (updateDocTypes.Count > 0)
        {
            await _unitOfWork.DocumentType.BulkUpdate(updateDocTypes);
            await _unitOfWork.Save();
        }
    }
    public async Task DeletDocumentType(DocumentTypeDto request)
    {
        var deletDocType = await _unitOfWork.DocumentType.Get(x => x.Id == request.Id);
        if (deletDocType != null)
        {
            _unitOfWork.DocumentType.Remove(deletDocType);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Document Type to delete.");
        }
    }

    public async Task<List<ContactStageDto>> GetContactStages()
    {
        var getContactStages = await _unitOfWork.ContactStage.GetAll();

        List<ContactStageDto> contactStages = new List<ContactStageDto>();
        foreach (var contactStage in getContactStages)
        {
            var stage = new ContactStageDto()
            {
                Id = contactStage.Id,
                Name = contactStage.Name,
                IsDefault = contactStage.IsDefault,
                IsCustomized = contactStage.IsCustomized,
                Sort = contactStage.Sort,
            };
            contactStages.Add(stage);
        }
        return contactStages.OrderBy(x => x.Sort).ToList();
    }
    public async Task UpdateContactStages(List<ContactStageDto> contactStageDtos)
    {
        List<ContactStages> newContactStages = new List<ContactStages>();
        List<ContactStages> updateContactStages = new List<ContactStages>();
        foreach (var contactStageDto in contactStageDtos)
        {
            var contactStage = await _unitOfWork.ContactStage.Get(x => x.Id == contactStageDto.Id);
            if (contactStage == null)
            {
                ContactStages newContactStage = new ContactStages()
                {
                    Name = contactStageDto.Name,
                    Sort = contactStageDto.Sort,
                    IsDefault = contactStageDto.IsDefault,
                    IsCustomized = true,
                };
                newContactStages.Add(newContactStage);
            }
            else
            {
                contactStage.Name = contactStageDto.Name;
                contactStage.Sort = contactStageDto.Sort;
                contactStage.IsDefault = contactStageDto.IsDefault;
                updateContactStages.Add(contactStage);
            }
        }
        if (newContactStages.Count > 0)
        {
            await _unitOfWork.ContactStage.BulkAdd(newContactStages);
            await _unitOfWork.Save();
        }
        if (updateContactStages.Count > 0)
        {
            await _unitOfWork.ContactStage.BulkUpdate(updateContactStages);
            await _unitOfWork.Save();
        }
    }
    public async Task DeleteContactStage(ContactStageDto request)
    {
        var deleteContactStage = await _unitOfWork.ContactStage.Get(x => x.Id == request.Id);
        if (deleteContactStage != null)
        {
            _unitOfWork.ContactStage.Remove(deleteContactStage);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Contact Stage to delete.");
        }
    }

    public async Task<List<CandidateSourceDto>> GetCandidateSources()
    {
        var candidateSources = await _unitOfWork.CandidateSource.GetAll();

        List<CandidateSourceDto> candidateSourceList = new List<CandidateSourceDto>();
        foreach (var candidateSource in candidateSources)
        {
            var source = new CandidateSourceDto()
            {
                Id = candidateSource.Id,
                Name = candidateSource.Name,
                IsCustomized = candidateSource.IsCustomized,
            };
            candidateSourceList.Add(source);
        }
        return candidateSourceList;
    }
    public async Task UpdateCandidateSources(List<CandidateSourceDto> candidateSources)
    {
        List<CandidateSource> newCandidateSources = new List<CandidateSource>();
        List<CandidateSource> updateCandidateSources = new List<CandidateSource>();
        foreach (var candidateSource in candidateSources)
        {
            var source = await _unitOfWork.CandidateSource.Get(x => x.Id == candidateSource.Id);
            if (source == null)
            {
                CandidateSource newCandidateSource = new CandidateSource()
                {
                    Name = candidateSource.Name,
                    IsCustomized = true,
                };
                newCandidateSources.Add(newCandidateSource);
            }
            else
            {
                source.Name = candidateSource.Name;
                updateCandidateSources.Add(source);
            }
        }
        if (newCandidateSources.Count > 0)
        {
            await _unitOfWork.CandidateSource.BulkAdd(newCandidateSources);
            await _unitOfWork.Save();
        }
        if (updateCandidateSources.Count > 0)
        {
            await _unitOfWork.CandidateSource.BulkUpdate(updateCandidateSources);
            await _unitOfWork.Save();
        }
    }
    public async Task DeletCandidateSource(CandidateSourceDto request)
    {
        var deleteCandidateSource = await _unitOfWork.CandidateSource.Get(x => x.Id == request.Id);
        if (deleteCandidateSource != null)
        {
            _unitOfWork.CandidateSource.Remove(deleteCandidateSource);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Candidate Source to delete.");
        }
    }

    public async Task<List<JobCategoryDto>> GetJobCategories()
    {
        var jobCategories = await _unitOfWork.JobCategory.GetAll();
        List<JobCategoryDto> jobCategoryDtos = new List<JobCategoryDto>();
        foreach (var jobCategory in jobCategories)
        {
            var category = new JobCategoryDto()
            {
                Id = jobCategory.Id,
                Name = jobCategory.Name,
            };
            var subCategories = await _unitOfWork.JobSubCategory.GetAll(x => x.JobCategoryId == jobCategory.Id);
            if (!subCategories.Any() || subCategories != null)
            {
                var subCategoriesList = subCategories.Select(x => new JobSubCategoryDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();

                category.JobSubCategories = subCategoriesList;
            }
            jobCategoryDtos.Add(category);
        }

        return jobCategoryDtos;
    }

    public async Task UpdateJobCategories(List<JobCategoryDto> jobCategories)
    {
        List<JobCategory> updateJobCategories = new List<JobCategory>();
        List<JobSubCategory> newJobSubCategories = new List<JobSubCategory>();
        List<JobSubCategory> updateJobSubCategories = new List<JobSubCategory>();

        var newAddJobCategoriesList = jobCategories.Where(x => x.Id == 0).ToList();
        var updateJobCategoriesList = jobCategories.Where(x => x.Id != 0).ToList();
        foreach (var jobCategory in newAddJobCategoriesList)
        {
            JobCategory newJobCategory = new JobCategory()
            {
                Name = jobCategory.Name,
            };
            await _unitOfWork.JobCategory.Add(newJobCategory);
            await _unitOfWork.Save();
            if (jobCategory.JobSubCategories != null)
            {
                var newSubCategories = jobCategory.JobSubCategories.Where(x => x.Id == 0).ToList();
                var updateSubCategories = jobCategory.JobSubCategories.Where(x => x.Id != 0).ToList();
                foreach (var subCategory in newSubCategories)
                {
                    var newSubCategory = new JobSubCategory()
                    {
                        Name = subCategory.Name,
                        JobCategoryId = newJobCategory.Id,
                    };
                    newJobSubCategories.Add(newSubCategory);
                }
                foreach (var subCategory in updateSubCategories)
                {
                    var updateSubCategory = await _unitOfWork.JobSubCategory.Get(x => x.Id == subCategory.Id);
                    updateSubCategory.Name = subCategory.Name;
                    updateJobSubCategories.Add(updateSubCategory);
                }
            }
        }

        foreach (var jobCategory in updateJobCategoriesList)
        {
            var updateJobCategroy = await _unitOfWork.JobCategory.Get(x => x.Id == jobCategory.Id);
            updateJobCategroy.Name = jobCategory.Name;
            updateJobCategories.Add(updateJobCategroy);
            if (jobCategory.JobSubCategories != null)
            {
                var newSubCategories = jobCategory.JobSubCategories.Where(x => x.Id == 0).ToList();
                var updateSubCategories = jobCategory.JobSubCategories.Where(x => x.Id != 0).ToList();

                foreach (var subCategory in newSubCategories)
                {
                    var newSubCategory = new JobSubCategory()
                    {
                        Name = subCategory.Name,
                        JobCategoryId = updateJobCategroy.Id,
                    };
                    newJobSubCategories.Add(newSubCategory);
                }
                foreach (var subCategory in updateSubCategories)
                {
                    var updateSubCategory = await _unitOfWork.JobSubCategory.Get(x => x.Id == subCategory.Id);
                    updateSubCategory.Name = subCategory.Name;
                    updateJobSubCategories.Add(updateSubCategory);
                }
            }
        }
        if (updateJobCategories.Count > 0)
        {
            await _unitOfWork.JobCategory.BulkUpdate(updateJobCategories);
            await _unitOfWork.Save();
        }
        if (newJobSubCategories.Count > 0)
        {
            await _unitOfWork.JobSubCategory.BulkAdd(newJobSubCategories);
        }
        if (updateJobSubCategories.Count > 0)
        {
            await _unitOfWork.JobSubCategory.BulkUpdate(updateJobSubCategories);
            await _unitOfWork.Save();
        }

    }

    public async Task DeleteJobCategory(JobCategoryDto request)
    {
        var deleteJobCategory = await _unitOfWork.JobCategory.Get(x => x.Id == request.Id, "JobSubCategories");
        if (deleteJobCategory != null)
        {
            if (deleteJobCategory.JobSubCategories != null && deleteJobCategory.JobSubCategories.Count > 0)
            {
                await _unitOfWork.JobSubCategory.BulkDelete(deleteJobCategory.JobSubCategories);
                await _unitOfWork.Save();
            }
            _unitOfWork.JobCategory.Remove(deleteJobCategory);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Job Category to delete.");
        }
    }

    public async Task DeleteJobSubCategory(JobSubCategoryDto request)
    {
        var deleteJobSubCategory = await _unitOfWork.JobSubCategory.Get(x => x.Id == request.Id);
        if (deleteJobSubCategory != null)
        {
            _unitOfWork.JobSubCategory.Remove(deleteJobSubCategory);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Job Sub Category to delete.");
        }
    }

    public async Task<List<JobLocationDto>> GetJoblocations()
    {
        var joblocations = await _unitOfWork.JobLocation.GetAll();
        List<JobLocationDto> jobLocationDtos = new List<JobLocationDto>();
        foreach (var jobLocation in joblocations)
        {
            var location = new JobLocationDto()
            {
                Id = jobLocation.Id,
                Name = jobLocation.Name,
            };
            var sublocations = await _unitOfWork.JobSubLocation.GetAll(x => x.JobLocationId == jobLocation.Id);
            if (!sublocations.Any() || sublocations != null)
            {
                var sublocationsList = sublocations.Select(x => new JobSubLocationDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();

                location.JobSubLocations = sublocationsList;
            }
            jobLocationDtos.Add(location);
        }

        return jobLocationDtos;
    }

    public async Task UpdateJobLocations(List<JobLocationDto> jobLocations)
    {
        List<JobLocation> updateJoblocations = new List<JobLocation>();
        List<JobSubLocation> newJobSublocations = new List<JobSubLocation>();
        List<JobSubLocation> updateJobSublocations = new List<JobSubLocation>();

        var newAddJoblocationsList = jobLocations.Where(x => x.Id == 0).ToList();
        var updateJoblocationsList = jobLocations.Where(x => x.Id != 0).ToList();
        foreach (var jobLocation in newAddJoblocationsList)
        {
            JobLocation newJobLocation = new JobLocation()
            {
                Name = jobLocation.Name,
            };
            await _unitOfWork.JobLocation.Add(newJobLocation);
            await _unitOfWork.Save();
            if (jobLocation.JobSubLocations != null)
            {
                var newSublocations = jobLocation.JobSubLocations.Where(x => x.Id == 0).ToList();
                var updateSublocations = jobLocation.JobSubLocations.Where(x => x.Id != 0).ToList();
                foreach (var subLocation in newSublocations)
                {
                    var newSubLocation = new JobSubLocation()
                    {
                        Name = subLocation.Name,
                        JobLocationId = newJobLocation.Id,
                    };
                    newJobSublocations.Add(newSubLocation);
                }
                foreach (var subLocation in updateSublocations)
                {
                    var updateSubLocation = await _unitOfWork.JobSubLocation.Get(x => x.Id == subLocation.Id);
                    updateSubLocation.Name = subLocation.Name;
                    updateJobSublocations.Add(updateSubLocation);
                }
            }
        }

        foreach (var jobLocation in updateJoblocationsList)
        {
            var updateJobLocation = await _unitOfWork.JobLocation.Get(x => x.Id == jobLocation.Id);
            updateJobLocation.Name = jobLocation.Name;
            updateJoblocations.Add(updateJobLocation);
            if (jobLocation.JobSubLocations != null)
            {
                var newSublocations = jobLocation.JobSubLocations.Where(x => x.Id == 0).ToList();
                var updateSublocations = jobLocation.JobSubLocations.Where(x => x.Id != 0).ToList();

                foreach (var subLocation in newSublocations)
                {
                    var newSubCategory = new JobSubLocation()
                    {
                        Name = subLocation.Name,
                        JobLocationId = updateJobLocation.Id,
                    };
                    newJobSublocations.Add(newSubCategory);
                }
                foreach (var subLocation in updateSublocations)
                {
                    var updateSubLocation = await _unitOfWork.JobSubLocation.Get(x => x.Id == subLocation.Id);
                    updateSubLocation.Name = subLocation.Name;
                    updateJobSublocations.Add(updateSubLocation);
                }
            }
        }
        if (updateJoblocations.Count > 0)
        {
            await _unitOfWork.JobLocation.BulkUpdate(updateJoblocations);
            await _unitOfWork.Save();
        }
        if (newJobSublocations.Count > 0)
        {
            await _unitOfWork.JobSubLocation.BulkAdd(newJobSublocations);
        }
        if (updateJobSublocations.Count > 0)
        {
            await _unitOfWork.JobSubLocation.BulkUpdate(updateJobSublocations);
            await _unitOfWork.Save();
        }

    }

    public async Task DeleteJobLocation(JobLocationDto request)
    {
        // Retrieve the JobLocation entity along with its JobSubLocations
        var deleteJobLocation = await _unitOfWork.JobLocation.Get(x => x.Id == request.Id);

        if (deleteJobLocation == null)
        {
            throw new BusinessLogicException("404", "Invalid Job Location to delete.");
        }

        // Retrieve associated JobSubLocations
        var deleteJobSubLocations = deleteJobLocation.JobSubLocations;

        // Bulk delete JobSubLocations if any exist
        if (deleteJobSubLocations != null && deleteJobSubLocations.Any())
        {
            await _unitOfWork.JobSubLocation.BulkDelete(deleteJobSubLocations);
            await _unitOfWork.Save();
        }

        // Remove the main JobLocation
        _unitOfWork.JobLocation.Remove(deleteJobLocation);

        // Save all changes within the transaction
        await _unitOfWork.Save();

    }

    public async Task DeleteJobSubLocation(JobSubLocationDto request)
    {
        var deleteJobSubLoaction = await _unitOfWork.JobSubLocation.Get(x => x.Id == request.Id);
        if (deleteJobSubLoaction != null)
        {
            _unitOfWork.JobSubLocation.Remove(deleteJobSubLoaction);
            await _unitOfWork.Save();
        }
        else
        {
            throw new BusinessLogicException("404", "Invalid Job Sub Location to delete.");
        }
    }

}
