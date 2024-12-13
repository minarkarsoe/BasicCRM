using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recsite_Ats.Domain.Entites;


namespace Recsite_Ats.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> //Other options: DbContext
{
    public DbSet<Seat> Seats { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyContacts> CompanyContacts { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<JobFollower> JobFollowers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Category> Categories { get; set; }
    //public DbSet<JobStatus> JobStatuses { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<EmploymentType> EmploymentTypes { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobApplication> Applications { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<CustomField> CustomFields { get; set; }
    public DbSet<FieldLayout> FieldLayouts { get; set; }
    public DbSet<FieldMapping> FieldMappings { get; set; }
    public DbSet<FieldType> FieldTypes { get; set; }
    public DbSet<SectionLayout> SectionLayouts { get; set; }
    public DbSet<StoreCustomFieldValue> StoreCustomFieldValues { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<CompanyNotes> CompanyNotes { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<CompanyFollowers> CompanyFollowers { get; set; }
    public DbSet<CountryData> CountryData { get; set; }
    public DbSet<BillingInformation> BillingInformation { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<NoteType> NoteTypes { get; set; }
    public DbSet<JobStatus> JobStatus { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<ContactStages> ContactStages { get; set; }
    public DbSet<CandidateSource> CandidateSources { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<JobSubCategory> JobSubCategories { get; set; }
    public DbSet<JobLocation> JobLocations { get; set; }
    public DbSet<JobSubLocation> JobSubLocations { get; set; }
    public DbSet<ApplicationModules> ApplicationModules { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<MailGunSetting> MailGunSettings { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        int adminRoleId = 1;
        int adminUserId = 1;

        builder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                AccountId = 1,
                Title = "Programming"
            }
            );
        //Seed Skills
        builder.Entity<Skill>().HasData(
            new Skill()
            {
                Id = 1,
                Title = "C#",
                CategoryId = 1
            });

        //Seed Location
        builder.Entity<Location>().HasData(
            new Location()
            {
                Id = 1,
                Title = "UK",
                AccountId = 1,
            });

        builder.Entity<EmploymentType>().HasData(
            new EmploymentType()
            {
                Id = 1,
                Title = "Permanent",
                AccountId = 1,
            });

        // Seed default Admin user
        var hasher = new PasswordHasher<ApplicationUser>();
        var seat = new Seat()
        {
            Id = 1,
            AccountId = 1,
            TotalSeats = 10,
            UsedSeats = 0,
            SeatRenewelAmount = 0,
            SeatRenewelDate = DateTime.Now,
        };
        var account = new Account()
        {
            Id = 1,
            PrimaryCountry = "UK",
            ContactFirstName = "Min Arkar",
            ContactLastName = "Soe",
            CompanyName = "New Recruitment Agency",
            ContactPhone = "09799314337",
            ContactEmail = "minarkarsoe@gmail.com",
            CreatedDate = DateTime.Now,
            CreatedBy = 0,
            EditedDate = DateTime.Now,
            SeatId = 1,

        };
        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            AccountId = 1,
            FirstName = "Min Arkar",
            LastName = "Soe",
            PhoneNumber = "09799314337",
            UserName = "admin@admin.com",
            NormalizedUserName = "ADMIN@ADMIN.COM",
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            EmailConfirmed = true,
            CreatedDate = DateTime.Now,
            CreatedBy = 0,
            EditedDate = DateTime.Now,
            SecurityStamp = string.Empty
        };

        var role = new ApplicationRole()
        {
            Id = 1,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Mar@ResciteDesign1");
        builder.Entity<Account>().HasData(account);
        builder.Entity<Seat>().HasData(seat);
        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new ApplicationRole
            {
                Id = 2,
                Name = "Recruiter",
                NormalizedName = "RECRUITER"
            });
        builder.Entity<ApplicationUser>().HasData(adminUser);

        // Seed user-role relationship
        builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
        {
            RoleId = adminRoleId,
            UserId = adminUserId,
        });

        // Seed Section Layouts 
        builder.Entity<SectionLayout>().HasData(
            new SectionLayout
            {
                SectionLayoutId = 1,
                TableName = nameof(Companies),
                SectionName = "Company Details",
                Sort = 1000
            },
            new SectionLayout
            {
                SectionLayoutId = 2,
                TableName = nameof(Jobs),
                SectionName = "Job Details",
                Sort = 1000
            },
            new SectionLayout
            {
                SectionLayoutId = 3,
                TableName = nameof(Contacts),
                SectionName = "Contact Details",
                Sort = 1000
            },
            new SectionLayout
            {
                SectionLayoutId = 4,
                TableName = nameof(Candidates),
                SectionName = "Candidate Details",
                Sort = 1000
            }
            );

        // Seed Field Type
        builder.Entity<FieldType>().HasData(
            new FieldType
            {
                Id = 1,
                FieldTypeName = "Text"
            },
            new FieldType
            {
                Id = 2,
                FieldTypeName = "LongText"
            },
            new FieldType
            {
                Id = 3,
                FieldTypeName = "Number"
            },
            new FieldType
            {
                Id = 4,
                FieldTypeName = "DropDown"
            },
            new FieldType
            {
                Id = 5,
                FieldTypeName = "MultiSelect"
            },
            new FieldType
            {
                Id = 6,
                FieldTypeName = "True/False"
            },
            new FieldType
            {
                Id = 7,
                FieldTypeName = "Decimal"
            },
            new FieldType
            {
                Id = 8,
                FieldTypeName = "DateTime"
            },
            new FieldType
            {
                Id = 9,
                FieldTypeName = "Image"
            }
            );

        builder.Entity<NoteType>().HasData(
            new NoteType
            {
                Id = 1,
                Name = "Call",
                IsCustomize = false,
                IsDefault = false,
            },
            new NoteType
            {
                Id = 2,
                Name = "To Do",
                IsCustomize = false,
                IsDefault = false,
            }
            );
        builder.Entity<JobStatus>().HasData(
            new JobStatus
            {
                Id = 1,
                Name = "Open",
                IsCustomized = false,
                Sort = 1
            },
            new JobStatus
            {
                Id = 2,
                Name = "On Hold",
                IsCustomized = false,
                Sort = 2
            },
            new JobStatus
            {
                Id = 3,
                Name = "Canceled",
                IsCustomized = false,
                Sort = 3
            },
            new JobStatus
            {
                Id = 4,
                Name = "Closed",
                IsCustomized = false,
                Sort = 4
            }
            );
        builder.Entity<DocumentType>().HasData(
            new DocumentType
            {
                Id = 1,
                Name = "Resume",
                IsCustomized = false
            },
            new DocumentType
            {
                Id = 2,
                Name = "Job Decription",
                IsCustomized = false,
            }
            );
        builder.Entity<ContactStages>().HasData(
           new ContactStages
           {
               Id = 1,
               Name = "Phone Call",
               IsCustomized = false,
               Sort = 1
           },
           new ContactStages
           {
               Id = 2,
               Name = "Email",
               IsCustomized = false,
               Sort = 2
           }
           );

        builder.Entity<ApplicationModules>().HasData(
            new ApplicationModules
            {
                Id = 1,
                Name = "Contacts",
                IsActive = true,
            },
            new ApplicationModules
            {
                Id = 2,
                Name = "Jobs",
                IsActive = true,
            },
            new ApplicationModules
            {
                Id = 3,
                Name = "Candidates",
                IsActive = true,
            },
            new ApplicationModules
            {
                Id = 4,
                Name = "Companies",
                IsActive = true,
            }
            );
        builder.Entity<EmailTemplate>().HasData(
            new EmailTemplate
            {
                Id = 1,
                AccountId = 0,
                ModuleId = 1,
                Name = "Contact Default Template",
                Description = "This is Contact Default Template",
                IsDefault = true,
                Template = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <title>Recovery Code</title>\r\n</head>\r\n<body>\r\n    <h1>This is Contact Default Template</h1>\r\n    <p>Thank you for using our service.</p>\r\n</body>\r\n</html>"
            }
            );

        // Account - ApplicationUser one-to-many relationship
        builder.Entity<Account>()
             .HasMany(a => a.ApplicationUsers)
             .WithOne(u => u.Account)
             .HasForeignKey(u => u.AccountId)
             .OnDelete(DeleteBehavior.Restrict);

        // Account - Seat one-to-one relationship
        builder.Entity<Account>()
            .HasOne(a => a.Seat)
            .WithOne(s => s.Account)
            .HasForeignKey<Seat>(s => s.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // ApplicationUser - UserToken one-to-many relationship
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.UserTokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Account>()
            .HasMany(a => a.AccountCompanies)
            .WithOne(a => a.Account)
            .HasForeignKey(fk => fk.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Account>()
            .HasMany(a => a.AccountContacts)
            .WithOne(a => a.Account)
            .HasForeignKey(fk => fk.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Account>()
            .HasMany(a => a.ApplicationUsers)
            .WithOne(a => a.Account)
            .HasForeignKey(fk => fk.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Account>()
            .HasMany(a => a.Categories)
            .WithOne(a => a.Account)
            .HasForeignKey(fk => fk.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Contact>()
            .HasMany(c => c.Addresses)
            .WithOne(c => c.Contact)
            .HasForeignKey(fk => fk.ContactId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Company>()
            .HasMany(c => c.Addresses)
            .WithOne(c => c.Company)
            .HasForeignKey(fk => fk.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Candidate>()
            .HasMany(c => c.Addresses)
            .WithOne(c => c.Candidate)
            .HasForeignKey(fk => fk.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Candidate>()
            .HasMany(c => c.Applications)
            .WithOne(c => c.Candidate)
            .HasForeignKey(fk => fk.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Job>()
            .HasMany(j => j.JobFollowers)
            .WithOne(j => j.Jobs)
            .HasForeignKey(fk => fk.JobId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Job>()
            .HasOne(j => j.Category)
            .WithMany(j => j.Jobs)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Job>()
            .HasOne(j => j.Location)
            .WithMany(j => j.Jobs)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Job>()
            .HasOne(j => j.EmploymentType)
            .WithMany(j => j.Jobs)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Job>()
            .HasMany(j => j.Applications)
            .WithOne(j => j.Jobs)
            .HasForeignKey(fk => fk.JobId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Category>()
            .HasMany(c => c.Skills)
            .WithOne(c => c.Category)
            .HasForeignKey(fk => fk.CategoryId);

        builder.Entity<CompanyContacts>()
            .HasOne(cc => cc.Company)
            .WithMany(c => c.CompanyContacts)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(cc => cc.CompanyId)
            .IsRequired(true);

        builder.Entity<CompanyContacts>()
            .HasOne(cc => cc.Contact)
            .WithMany(c => c.CompanyContacts)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(cc => cc.ContactId)
            .IsRequired(true);

        builder.Entity<CompanyNotes>()
            .HasOne(cc => cc.Company)
            .WithMany(c => c.CompanyNotes)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(cc => cc.CompanyId)
            .IsRequired(true);

        builder.Entity<CompanyNotes>()
            .HasOne(cc => cc.Note)
            .WithMany(c => c.CompanyNotes)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(cc => cc.NoteId)
            .IsRequired(true);

        builder.Entity<CompanyDocuments>()
            .HasOne(cc => cc.Company)
            .WithMany(c => c.CompanyDocuments)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(cc => cc.CompanyId)
            .IsRequired(true);

        builder.Entity<CompanyDocuments>()
            .HasOne(cc => cc.Document)
            .WithMany(c => c.CompanyDocuments)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(cc => cc.DocumentId)
            .IsRequired(true);

        builder.Entity<CompanyFollowers>()
             .HasOne(cc => cc.Company)
             .WithMany(c => c.CompanyFollowers)
             .HasPrincipalKey(c => c.Id)
             .HasForeignKey(cc => cc.CompanyId)
             .IsRequired(true);

        builder.Entity<CompanyFollowers>()
             .HasOne(cc => cc.User)
             .WithMany(c => c.CompanyFollowers)
             .HasPrincipalKey(c => c.Id)
             .HasForeignKey(cc => cc.UserId)
             .IsRequired(true);

        /*builder.Entity<JobLocation>()
            .HasMany(j => j.JobSubLocations)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<JobCategory>()
            .HasMany(j => j.JobSubCategories)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);*/
    }
}
