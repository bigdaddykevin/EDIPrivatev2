using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblDetail")]
    public partial class TblDetail
    {
        [Column("Detail ID")]
        [Key]
        public int Id { get; set; }

        [Column("Follow-Up ID")]
        public int FollowUpId { get; set; }

        public bool? Releasable { get; set; }
        public string Keywords { get; set; }
        public string Steward { get; set; }

        [Column("Full-Scale Years")]
        [MaxLength(255)]
        public string FullScaleYears { get; set; }

        [Column("Total Cost")]
        public int? TotalCost { get; set; }

        [Column("Total Cost Detail")]
        public string TotalCostDetail { get; set; }

        [Column("Experiment Flag")]
        public bool? ExperimentFlag { get; set; }

        [Column("Universe Survey")]
        public bool? UniverseSurvey { get; set; }

        [Column("Sample Survey")]
        public bool? SampleSurvey { get; set; }

        [Column("Cross-Sectional")]
        public bool? CrossSectional { get; set; }

        public bool? Longitudinal { get; set; }
        public bool? Voluntary { get; set; }
        public bool? Mandatory { get; set; }

        [Column("Required for Benefits")]
        public bool? RequiredForBenefits { get; set; }

        [Column("Requirement Detail")]
        public string RequirementDetail { get; set; }

        [Column("Requirement Reason")]
        public string RequirementReason { get; set; }

        [Column("Program Monitoring")]
        public bool? ProgramMonitoring { get; set; }

        [Column("Grantee Reports")]
        public bool? GranteeReports { get; set; }

        [Column("Authorizing Law")]
        public string AuthorizingLaw { get; set; }

        [Column("Bureau Code")]
        public string BureauCode { get; set; }

        [Column("Program Code")]
        public string ProgramCode { get; set; }

        [Column("PII-DI")]
        public bool? PII_DI { get; set; }

        [Column("PIA")]
        public bool? PIA { get; set; }

        [Column("SORN")]
        [MaxLength(255)]
        public string SORN { get; set; }

        [Column("SORN URL")]
        public string SORNUrl { get; set; }

        [Column("Could Be Public")]
        public bool? CouldBePublic { get; set; }

        [Column("Public Access Level")]
        [MaxLength(255)]
        public string PublicAccessLevel { get; set; }

        [Column("Confidentiality Restrictions")]
        public string ConfidentialityRestrictions { get; set; }

        [Column("Publication Date (Stats)", TypeName = "date")]
        public DateTime? PublicationDateStats { get; set; }

        [Column("Publication Type (Stats)")]
        [MaxLength(255)]
        public string PublicationTypeStats { get; set; }

        [Column("Publication URL (Stats)")]
        [MaxLength(255)]
        public string PublicationUrlStats { get; set; }

        [Column("Planned Pub (Stats)")]
        public bool? PlannedPubStats { get; set; }

        [Column("Publication Date (Data)", TypeName = "date")]
        public DateTime? PublicationDateData { get; set; }

        [Column("Publication URL (Data)")]
        [MaxLength(255)]
        public string PublicationUrlData { get; set; }

        [Column("Planned Pub (Data)")]
        public bool? PlannedPubData { get; set; }

        [Column("Restricted-Use Date", TypeName = "date")]
        public DateTime? RestrictedUseDate { get; set; }

        [Column("Planned Restricted Data")]
        public bool? PlannedRestrictedData { get; set; }

        public int? Students { get; set; }
        public int? Staff { get; set; }
        public int? Institutions { get; set; }
        public int? Programs { get; set; }

        [Column("Other Subject")]
        [MaxLength(50)]
        public string OtherSubject { get; set; }

        [Column("Age 0-2")]
        public int? Age0_2 { get; set; }

        [Column("Age 3-5")]
        public int? Age3_5 { get; set; }

        [Column("Age 6-21")]
        public int? Age6_21 { get; set; }

        [Column("Age Older Than 21")]
        public int? AgeOlderThan21 { get; set; }

        [Column("Age NA")]
        public int? AgeNa { get; set; }

        [Column("Pre-K")]
        public int? PreK { get; set; }

        [Column("Elementary")]
        public int? ElementarySchool { get; set; }

        [Column("Middle")]
        public int? MiddleSchool { get; set; }

        [Column("High School")]
        public int? HighSchool { get; set; }

        public int? Postsecondary { get; set; }
        public int? Graduate { get; set; }

        [Column("Continued Technical Ed")]
        public int? ContinuedTechnicalEd { get; set; }

        [Column("Adult Education")]
        public int? AdultEducation { get; set; }

        [Column("General Adult")]
        public int? GeneralAdult { get; set; }

        [Column("Education Level NA")]
        public int? EducationLevelNa { get; set; }

        [Column("Other Population")]
        [MaxLength(255)]
        public string OtherPopulation { get; set; }

        [Column("Subject Population Detail")]
        public string SubjectPopulationDetail { get; set; }

        [Column("Individual Data")]
        public bool? IndividualData { get; set; }

        [Column("Classroom Data")]
        public bool? ClassroomData { get; set; }

        [Column("Grade Level Data")]
        public bool? GradeLevelData { get; set; }

        [Column("School/Institution Data")]
        public bool? SchoolInstitutionData { get; set; }

        [Column("LEA Data")]
        public bool? LeaData { get; set; }

        [Column("State Data")]
        public bool? StateData { get; set; }

        [Column("Region Data")]
        public bool? RegionData { get; set; }

        [Column("National Data")]
        public bool? NationalData { get; set; }

        [Column("Other Data Level")]
        [MaxLength(255)]
        public string OtherDataLevel { get; set; }

        [Column("Data Level Detail")]
        public string DataLevelDetail { get; set; }

        [Column("Public Data Level")]
        [MaxLength(255)]
        public string PublicDataLevel { get; set; }

        [Column("NOD Issued")]
        public bool? NodIssued { get; set; }

        [Column("Date NOD Issued", TypeName = "date")]
        public DateTime? DateNodIssued { get; set; }

        [Column("NOD Expiration Date", TypeName = "date")]
        public DateTime? NodExpirationDate { get; set; }

        [Column("NOD Explanation")]
        public string NodExplanation { get; set; }

        [Column("NOD TOC")]
        public string NodToc { get; set; }

        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [ForeignKey("FollowUpId")]
        [InverseProperty("Details")]
        public virtual TblFollowUp FollowUp { get; set; }
    }
}