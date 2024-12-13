namespace Recsite_Ats.Domain.DataTransferObject;
public class NoteTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsCustomized { get; set; }
}

public class JobStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCustomized { get; set; }
    public int Sort { get; set; }
}

public class DocumentTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCustomized { get; set; }
}

public class ContactStageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsCustomized { get; set; }
    public int Sort { get; set; }
}

public class CandidateSourceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCustomized { get; set; }
}

public class JobCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<JobSubCategoryDto>? JobSubCategories { get; set; }
}

public class JobSubCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }

}

public class JobLocationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<JobSubLocationDto>? JobSubLocations { get; set; }
}

public class JobSubLocationDto
{
    public int Id { get; set; }
    public string Name { get; set; }

}