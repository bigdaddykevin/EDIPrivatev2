using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblRespondent")]
    public partial class TblRespondent
    {
        [Column("Respondent ID")]
        [Key]
        public int Id { get; set; }

        [Column("Collection ID")]
        public int? CollectionId { get; set; }

        [Column("Respondent Type")]
        [MaxLength(255)]
        public string Type { get; set; }

        [Column("Respondent Type Detail")]
        [MaxLength(255)]
        public string TypeDetail { get; set; }

        public string Topics { get; set; }

        [Column("Expected Sample Size")]
        public int? ExpectedSampleSize { get; set; }

        [Column("Actual Sample Size")]
        public int? ActualSampleSize { get; set; }

        [Column("Actual Sample Size Detail")]
        public string ActualSampleSizeDetail { get; set; }

        [Column("Respondent Size Estimate")]
        public int? RespondentSizeEstimate { get; set; }

        [Column("Respondent Size Actual")]
        public int? RespondentSizeActual { get; set; }

        [Column("Response Size")]
        public int? ResponseSize { get; set; }

        [Column("Burden Time Total")]
        public int? BurdenTimeTotal { get; set; }

        [Column("Burden Time Per Respondent")]
        public int? BurdenTimePerRespondent { get; set; }

        [Column("Survey Burden Per Respondent")]
        public int? SurveyBurdenPerRespondent { get; set; }

        [Column("Assessment Burden Per Respondent")]
        public int? AssessmentBurdenPerRespondent { get; set; }

        [Column("Response Rate Estimated")]
        public double? ResponseRateEstimated { get; set; }

        [Column("Response Rate Actual")]
        public double? ResponseRateActual { get; set; }

        [Column("Response Rate Actual Detail")]
        public string ResponseRateActualDetail { get; set; }

        [Column("# Variables")]
        public int? Variables { get; set; }

        [Column("# Varaibles w/o Replicate Weights & Imputation Flags")]
        public int? VaraiblesWithoutReplicateWeightsAndImputationFlags { get; set; }

        [Column("Final Data Format")]
        [MaxLength(255)]
        public string FinalDataFormat { get; set; }

        [Column("Respondent Experiment")]
        public bool? Experiment { get; set; }

        [Column("Non-Cash Range Amount")]
        [MaxLength(255)]
        public string NonCashRangeAmount { get; set; }

        [Column("Non-Cash Detail")]
        [MaxLength(255)]
        public string NonCashDetail { get; set; }

        [Column("Cash Range Amount")]
        [MaxLength(50)]
        public string CashRangeAmount { get; set; }

        [Column("Cash Detail")]
        [MaxLength(255)]
        public string CashDetail { get; set; }

        [Column("Justification for Incentive")]
        public string JustificationForIncentive { get; set; }

        [Column("Voluntary Response")]
        public bool? Voluntary { get; set; }

        [Column("Mandatory Response")]
        public bool? Mandatory { get; set; }

        [Column("Required for Benefits Response")]
        public bool? RequiredForBenefits { get; set; }

        [Column("Required Response Detail")]
        public string RequiredResponseDetail { get; set; }

        [Column("Response Requirement Reason")]
        public string ResponseRequirementReason { get; set; }

        [Column("Consent Form")]
        public string ConsentForm { get; set; }

        [Column("Explicit Consent")]
        public bool? ExplicitConsent { get; set; }

        [Column("Implicit Consent")]
        public bool? ImplicitConsent { get; set; }

        [Column("Participatory Consent")]
        public bool? ParticipatoryConsent { get; set; }

        [Column("Consent Not Applicable")]
        public bool? ConsentNotApplicable { get; set; }

        [Column("FIC Language")]
        public string FICLanguage { get; set; }

        [Column("FIC Language Locations")]
        public string FICLanguageLocations { get; set; }

        [Column("VC Language IC")]
        public string VCLanguageIC { get; set; }

        [Column("VC Language Letters")]
        public string VCLanguageLetters { get; set; }

        [Column("VC Language FAQ")]
        public string VCLanguageFaq { get; set; }

        [Column("VC Language Brochure")]
        public string VCLanguageBrochure { get; set; }

        [Column("Confidentiality Law Cited")]
        [MaxLength(255)]
        public string ConfidentialityLawCited { get; set; }

        [Column("PRA Statement")]
        public string PRAStatement { get; set; }

        [Column("PRA Statement Locations")]
        public string PRAStatementLocations { get; set; }

        [Column("Respondent Age 0-2")]
        public bool? Age0_2 { get; set; }

        [Column("Respondent Age 3-5")]
        public bool? Age3_5 { get; set; }

        [Column("Respondent Age 6-21")]
        public bool? Age6_21 { get; set; }

        [Column("Respondent Age Older Than 21")]
        public bool? AgeOlderThan21 { get; set; }

        [Column("Respondent Age NA")]
        public bool? AgeNa { get; set; }

        [Column("Respondent Pre-K")]
        public bool? PreK { get; set; }

        [Column("Respondent Elementary")]
        public bool? ElementarySchool { get; set; }

        [Column("Respondent Middle")]
        public bool? MiddleSchool { get; set; }

        [Column("Respondent High School")]
        public bool? HighSchool { get; set; }

        [Column("Respondent Postsecondary")]
        public bool? Postsecondary { get; set; }

        [Column("Respondent Graduate")]
        public bool? Graduate { get; set; }

        [Column("Respondent Continued Technical Ed")]
        public bool? ContinuedTechnicalEd { get; set; }

        [Column("Respondent Adult Education")]
        public bool? AdultEducation { get; set; }

        [Column("Respondent Education Level NA")]
        public bool? EducationLevelNa { get; set; }

        [Column("Respondent Other Population")]
        [MaxLength(255)]
        public string OtherPopulation { get; set; }

        [Column("Respondent Population Detail")]
        public string PopulationDetail { get; set; }

        [Column("Administrative Records")]
        public bool? AdministrativeRecords { get; set; }

        [Column("Address Update")]
        public bool? AddressUpdate { get; set; }

        [Column("List Data")]
        public bool? ListData { get; set; }

        public bool? Recruitment { get; set; }

        [Column("Coordination Help")]
        public bool? CoordinationHelp { get; set; }

        public bool? Screener { get; set; }
        public bool? Assessment { get; set; }
        public bool? Survey { get; set; }

        [Column("Abbreviated Survey")]
        public bool? AbbreviatedSurvey { get; set; }

        [Column("Program Reporting")]
        public bool? ProgramReporting { get; set; }

        [Column("EDFacts")]
        public bool? EDFacts { get; set; }

        [Column("CPS")]
        public bool? CPS { get; set; }

        [Column("NSLDS")]
        public bool? NSLDS { get; set; }

        [Column("Other Response Type")]
        [MaxLength(255)]
        public string OtherResponseType { get; set; }

        [Column("Response Type Detail")]
        public string ResponseTypeDetail { get; set; }

        public int? Paper { get; set; }

        [Column("Phone (Not CATI)")]
        public int? PhoneNotCATI { get; set; }

        [Column("CATI")]
        public int? CATI { get; set; }

        public int? Web { get; set; }
        public int? Email { get; set; }

        [Column("F2F (Not CAPI)")]
        public int? F2FNotCAPI { get; set; }

        [Column("CAPI")]
        public int? CAPI { get; set; }

        public int? Spreadsheet { get; set; }

        [Column("PRS")]
        public int? PRS { get; set; }

        [Column("Other Collection Mode")]
        [MaxLength(255)]
        public string OtherCollectionMode { get; set; }

        [Column("Collection Mode Detail")]
        public string CollectionModeDetail { get; set; }

        [Column("Other Languages")]
        [MaxLength(255)]
        public string OtherLanguages { get; set; }

        [MaxLength(255)]
        public string Interpreters { get; set; }

        [Column("Feasibility Calls")]
        public bool? FeasibilityCalls { get; set; }

        public bool? Pretest { get; set; }

        [Column("Cog Labs")]
        public bool? CogLabs { get; set; }

        [Column("Focus Groups")]
        public bool? FocusGroups { get; set; }

        [Column("PIlot Test")]
        public bool? PilotTest { get; set; }

        [Column("Field Test")]
        public bool? FieldTest { get; set; }

        [Required]
        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [ForeignKey("CollectionId")]
        [InverseProperty("Respondents")]
        public virtual TblCollection Collection { get; set; }
    }
}