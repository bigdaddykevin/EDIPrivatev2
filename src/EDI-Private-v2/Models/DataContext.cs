using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EDIPrivate.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCollection>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblCollection$PrimaryKey");

                entity.HasIndex(e => e.DetailId)
                    .HasName("tblCollection$tblDetailtblCollection");

                entity.Property(e => e.SurveyFaq).HasDefaultValueSql("0");

                entity.Property(e => e.SurveyBrochure).HasDefaultValueSql("0");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblContact>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblContact$PrimaryKey");
            });

            modelBuilder.Entity<TblDataset>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tblDatas__C59C019CC814936A");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<TblDetail>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblDetail$PrimaryKey");

                entity.HasIndex(e => e.FollowUpId)
                    .HasName("tblDetail$tblFollow-UptblDetail");

                entity.Property(e => e.ExperimentFlag).HasDefaultValueSql("0");

                entity.Property(e => e.UniverseSurvey).HasDefaultValueSql("0");

                entity.Property(e => e.SampleSurvey).HasDefaultValueSql("0");

                entity.Property(e => e.CrossSectional).HasDefaultValueSql("0");

                entity.Property(e => e.Longitudinal).HasDefaultValueSql("0");

                entity.Property(e => e.Voluntary).HasDefaultValueSql("0");

                entity.Property(e => e.Mandatory).HasDefaultValueSql("0");

                entity.Property(e => e.RequiredForBenefits).HasDefaultValueSql("0");

                entity.Property(e => e.ProgramMonitoring).HasDefaultValueSql("0");

                entity.Property(e => e.GranteeReports).HasDefaultValueSql("0");

                entity.Property(e => e.ProgramCode).HasDefaultValueSql("N'018:000'");

                entity.Property(e => e.PII_DI).HasDefaultValueSql("0");

                entity.Property(e => e.PIA).HasDefaultValueSql("0");

                entity.Property(e => e.CouldBePublic).HasDefaultValueSql("0");

                entity.Property(e => e.PublicAccessLevel).HasDefaultValueSql("N'Public'");

                entity.Property(e => e.PlannedPubStats).HasDefaultValueSql("0");

                entity.Property(e => e.PlannedPubData).HasDefaultValueSql("0");

                entity.Property(e => e.PlannedRestrictedData).HasDefaultValueSql("0");

                entity.Property(e => e.IndividualData).HasDefaultValueSql("0");

                entity.Property(e => e.ClassroomData).HasDefaultValueSql("0");

                entity.Property(e => e.GradeLevelData).HasDefaultValueSql("0");

                entity.Property(e => e.SchoolInstitutionData).HasDefaultValueSql("0");

                entity.Property(e => e.LeaData).HasDefaultValueSql("0");

                entity.Property(e => e.StateData).HasDefaultValueSql("0");

                entity.Property(e => e.RegionData).HasDefaultValueSql("0");

                entity.Property(e => e.NationalData).HasDefaultValueSql("0");

                entity.Property(e => e.NodIssued).HasDefaultValueSql("0");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.FollowUp)
                    .WithMany(p => p.Details)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblDetail$tblFollow-UptblDetail");
            });

            modelBuilder.Entity<TblDFLink>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tblDFLin__EC7E544990A86DCE");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<TblDivision>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblDivision$PrimaryKey");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("tblDivision$Division Abbreviation");

                entity.HasIndex(e => e.Name)
                    .HasName("tblDivision$Division Name");

                entity.HasIndex(e => e.UnitId)
                    .HasName("tblDivision$Unit ID");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Divisions)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblDivision$tblUnittblDivision");
            });

            modelBuilder.Entity<TblElement>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_tblElement$");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");

                entity.Property(e => e.Vector).HasDefaultValueSql("0");

                entity.Property(e => e.EDFacts).HasDefaultValueSql("0");

                entity.Property(e => e.IDEA).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TblEVLink>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_tblEVLink$");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<TblFELink>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_tblFELink$");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<TblFile>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tblFile$__20435FC3A1D51BA2");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<TblFollowUp>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblFollow-Up$Survey ID");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("tblFollow-Up$Follow-Up Abbreviation");

                entity.HasIndex(e => e.Name)
                    .HasName("tblFollow-Up$Follow-Up Name");

                entity.HasIndex(e => e.PrecedingId)
                    .HasName("tblFollow-Up$Preceding Follow-up ID");

                entity.HasIndex(e => e.StudyId)
                    .HasName("tblFollow-Up$StudyPanel");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Study)
                    .WithMany(p => p.FollowUps)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblFollow-Up$StudyPanel");
            });

            modelBuilder.Entity<TblLink>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblLink$PrimaryKey");

                entity.HasIndex(e => e.CollectionId)
                    .HasName("tblLink$tblCollectiontblLink");

                entity.HasIndex(e => e.PackageId)
                    .HasName("tblLink$PackageLink");

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.Links)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblLink$tblCollectiontblLink");
            });

            modelBuilder.Entity<TblPackage>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblPackage$PrimaryKey");

                entity.Property(e => e.GenericClearance).HasDefaultValueSql("0");

                entity.Property(e => e.PercentCollectedElectronically).HasDefaultValueSql("0");

                entity.Property(e => e.ResponseToPublic).HasDefaultValueSql("0");

                entity.Property(e => e.OMBPassback).HasDefaultValueSql("0");

                entity.Property(e => e.OAFlag).HasDefaultValueSql("0");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<TblPrincipalOffice>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblPrincipalOffice$PrimaryKey");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("tblPrincipalOffice$Principal Office Abbreviation");

                entity.HasIndex(e => e.Name)
                    .HasName("tblPrincipalOffice$Principal Office Name")
                    .IsUnique();
            });

            modelBuilder.Entity<TblProgram>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblProgram$Survey ID");

                entity.HasIndex(e => e.DivisionId)
                    .HasName("tblProgram$Program AreaProgram");

                entity.HasIndex(e => e.PrimaryContactId)
                    .HasName("tblProgram$Primary Contact ID");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("tblProgram$Program Abbreviation");

                entity.HasIndex(e => e.Name)
                    .HasName("tblProgram$Program Name")
                    .IsUnique();

                entity.HasIndex(e => e.SupervisorId)
                    .HasName("tblProgram$Supervisor ID");

                entity.HasIndex(e => e.SecondContactId)
                    .HasName("tblProgram$2nd Contact ID");

                entity.HasIndex(e => e.ThirdContactId)
                    .HasName("tblProgram$3rd Contact ID");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.Programs)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblProgram$Program AreaProgram");
            });

            modelBuilder.Entity<TblRespondent>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblRespondent$Respondent ID");

                entity.HasIndex(e => e.CollectionId)
                    .HasName("tblRespondent$Package ID");

                entity.Property(e => e.Experiment).HasDefaultValueSql("0");

                entity.Property(e => e.Voluntary).HasDefaultValueSql("0");

                entity.Property(e => e.Mandatory).HasDefaultValueSql("0");

                entity.Property(e => e.RequiredForBenefits).HasDefaultValueSql("0");

                entity.Property(e => e.ExplicitConsent).HasDefaultValueSql("0");

                entity.Property(e => e.ImplicitConsent).HasDefaultValueSql("0");

                entity.Property(e => e.ConsentNotApplicable).HasDefaultValueSql("0");

                entity.Property(e => e.Age0_2).HasDefaultValueSql("0");

                entity.Property(e => e.Age3_5).HasDefaultValueSql("0");

                entity.Property(e => e.Age6_21).HasDefaultValueSql("0");

                entity.Property(e => e.AgeOlderThan21).HasDefaultValueSql("0");

                entity.Property(e => e.AgeNa).HasDefaultValueSql("0");

                entity.Property(e => e.PreK).HasDefaultValueSql("0");

                entity.Property(e => e.ElementarySchool).HasDefaultValueSql("0");

                entity.Property(e => e.MiddleSchool).HasDefaultValueSql("0");

                entity.Property(e => e.HighSchool).HasDefaultValueSql("0");

                entity.Property(e => e.Postsecondary).HasDefaultValueSql("0");

                entity.Property(e => e.Graduate).HasDefaultValueSql("0");

                entity.Property(e => e.ContinuedTechnicalEd).HasDefaultValueSql("0");

                entity.Property(e => e.AdultEducation).HasDefaultValueSql("0");

                entity.Property(e => e.EducationLevelNa).HasDefaultValueSql("0");

                entity.Property(e => e.AdministrativeRecords).HasDefaultValueSql("0");

                entity.Property(e => e.AddressUpdate).HasDefaultValueSql("0");

                entity.Property(e => e.ListData).HasDefaultValueSql("0");

                entity.Property(e => e.Recruitment).HasDefaultValueSql("0");

                entity.Property(e => e.CoordinationHelp).HasDefaultValueSql("0");

                entity.Property(e => e.Screener).HasDefaultValueSql("0");

                entity.Property(e => e.Assessment).HasDefaultValueSql("0");

                entity.Property(e => e.Survey).HasDefaultValueSql("0");

                entity.Property(e => e.AbbreviatedSurvey).HasDefaultValueSql("0");

                entity.Property(e => e.ProgramReporting).HasDefaultValueSql("0");

                entity.Property(e => e.EDFacts).HasDefaultValueSql("0");

                entity.Property(e => e.CPS).HasDefaultValueSql("0");

                entity.Property(e => e.NSLDS).HasDefaultValueSql("0");

                entity.Property(e => e.FeasibilityCalls).HasDefaultValueSql("0");

                entity.Property(e => e.Pretest).HasDefaultValueSql("0");

                entity.Property(e => e.CogLabs).HasDefaultValueSql("0");

                entity.Property(e => e.FocusGroups).HasDefaultValueSql("0");

                entity.Property(e => e.PilotTest).HasDefaultValueSql("0");

                entity.Property(e => e.FieldTest).HasDefaultValueSql("0");

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.Respondents)
                    .HasConstraintName("tblRespondent$CollectionRespondent");
            });

            modelBuilder.Entity<TblSDLink>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tblSDLin__10A95C1270A5C83D");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<TblStudy>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblStudy$Survey ID");

                entity.HasIndex(e => e.ProgramId)
                    .HasName("tblStudy$ProgramStudy");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("tblStudy$Study Abbreviation");

                entity.HasIndex(e => e.Name)
                    .HasName("tblStudy$Study Name")
                    .IsUnique();

                entity.Property(e => e.SsmaTimeStamp).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.Studies)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblStudy$ProgramStudy");
            });

            modelBuilder.Entity<TblUnit>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tblUnit$PrimaryKey");

                entity.HasIndex(e => e.PrincipalOfficeId)
                    .HasName("tblUnit$tblPrincipalOfficetblUnit");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("tblUnit$Unit Abbreviation");

                entity.HasIndex(e => e.Name)
                    .HasName("tblUnit$Unit Name")
                    .IsUnique();

                entity.HasOne(d => d.PrincipalOffice)
                    .WithMany(p => p.Units)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("tblUnit$tblPrincipalOfficetblUnit");
            });

            modelBuilder.Entity<TblValue>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_tblValue$");

                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
            });
        }

        public virtual DbSet<TblCollection> TblCollection { get; set; }
        public virtual DbSet<TblContact> TblContact { get; set; }
        public virtual DbSet<TblDataset> TblDataset { get; set; }
        public virtual DbSet<TblDetail> TblDetail { get; set; }
        public virtual DbSet<TblDFLink> TblDFLink { get; set; }
        public virtual DbSet<TblDivision> TblDivision { get; set; }
        public virtual DbSet<TblElement> TblElement { get; set; }
        public virtual DbSet<TblEVLink> TblEVLink { get; set; }
        public virtual DbSet<TblFELink> TblFELink { get; set; }
        public virtual DbSet<TblFile> TblFile { get; set; }
        public virtual DbSet<TblFollowUp> TblFollowUp { get; set; }
        public virtual DbSet<TblLink> TblLink { get; set; }
        public virtual DbSet<TblPackage> TblPackage { get; set; }
        public virtual DbSet<TblPrincipalOffice> TblPrincipalOffice { get; set; }
        public virtual DbSet<TblProgram> TblProgram { get; set; }
        public virtual DbSet<TblRespondent> TblRespondent { get; set; }
        public virtual DbSet<TblSDLink> TblSDLink { get; set; }
        public virtual DbSet<TblStudy> TblStudy { get; set; }
        public virtual DbSet<TblUnit> TblUnit { get; set; }
        public virtual DbSet<TblValue> TblValue { get; set; }
    }
}