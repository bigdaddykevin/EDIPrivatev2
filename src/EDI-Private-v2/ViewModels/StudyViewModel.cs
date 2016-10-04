using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class StudyViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Symbol { get; set; }

        [Display(Name = "Old Name 1")]
        public string OldName1 { get; set; }

        [Display(Name = "Old Name 1 Symbol")]
        public string OldName1Symbol { get; set; }

        [Display(Name = "Old Name 1 Duration")]
        public string OldName1Duration { get; set; }

        [Display(Name = "Old Name 2")]
        public string OldName2 { get; set; }

        [Display(Name = "Old Name 2 Symbol")]
        public string OldName2Symbol { get; set; }

        [Display(Name = "Old Name 2 Duration")]
        public string OldName2Duration { get; set; }

        public SeriesStubViewModel Series { get; set; }
        public string Investigator { get; set; }

        [Display(Name = "Bureau Code")]
        public string BureauCode { get; set; }

        [Display(Name = "Program Code")]
        public string ProgramCode { get; set; }

        [DataType(DataType.Url)]
        public Uri Website { get; set; }

        public string Keywords { get; set; }
        public string Description { get; set; }

        [Display(Name = "Authorizing Law")]
        public string AuthorizingLaw { get; set; }

        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public int? TotalCost { get; set; }

        [Display(Name = "Total Cost Years")]
        public string TotalCostYears { get; set; }

        [Display(Name = "Total Cost Description")]
        public string TotalCostDescription { get; set; }

        public bool Universe { get; set; }

        public bool Sample { get; set; }

        public bool Longitudinal { get; set; }

        [Display(Name = "Cross-Sectional")]
        public bool CrossSectional { get; set; }

        [Display(Name = "Program Monitoring")]
        public bool ProgramMonitoring { get; set; }

        [Display(Name = "Grantee Reporting")]
        public bool GranteeReporting { get; set; }

        public bool Voluntary { get; set; }

        public bool Mandatory { get; set; }

        [Display(Name = "Required for Benefits")]
        public bool RequiredForBenefits { get; set; }

        [Display(Name = "Collection Requirement Description")]
        public string RequirementDescription { get; set; }

        [Display(Name = "Collection Requirement Reason")]
        public string RequirementReason { get; set; }

        public string SORN { get; set; }

        [Display(Name = "SORN URL")]
        [DataType(DataType.Url)]
        public Uri SORNUrl { get; set; }

        [Display(Name = "Confidentiality Restrictions")]
        public string ConfidentialityRestrictions { get; set; }

        [Display(Name = "PII-DI")]
        public bool PII_DI { get; set; }

        public bool PIA { get; set; }

        [Display(Name = "Data Could Be Public")]
        public bool DataCouldBePublic { get; set; }

        [Display(Name = "Statistics Publication Type")]
        public string PublicationStatisticsType { get; set; }

        [Display(Name = "Statistics Publication URL")]
        [DataType(DataType.Url)]
        public Uri PublicationStatisticsUrl { get; set; }

        [Display(Name = "Statistics Publication Date")]
        [DataType(DataType.Date)]
        public DateTime? PublicationStatisticsDate { get; set; }

        [Display(Name = "Data Publication URL")]
        [DataType(DataType.Url)]
        public Uri PublicationDataUrl { get; set; }

        [Display(Name = "Data Publication Date")]
        [DataType(DataType.Date)]
        public DateTime? PublicationDataDate { get; set; }

        [Display(Name = "Restricted-Use Data Publication Date")]
        [DataType(DataType.Date)]
        public DateTime? PublicationRestrictedUseDataDate { get; set; }

        [Display(Name = "Subject Population")]
        public string SubjectPopulation { get; set; }

        [Display(Name = "Subject Population Description")]
        public string SubjectPopulationDescription { get; set; }

        [Display(Name = "Data Levels Available")]
        public string DataLevelsAvailable { get; set; }

        [Display(Name = "Public Data Level")]
        public string DataLevelPublic { get; set; }

        [Display(Name = "Data Level Description")]
        public string DataLevelDescription { get; set; }

        [Display(Name = "Previous Study")]
        public StudyStubViewModel PreviousStudy { get; set; }

        public IEnumerable<ContactViewModel> Contacts { get; set; }
        public IEnumerable<CollectionStubViewModel> Collections { get; set; }
        public IEnumerable<FileStubViewModel> Files { get; set; }

        internal StudyStubViewModel ToStub() =>
            new StudyStubViewModel()
            {
                Id = Id,
                Name = Name,
                Series = Series
            };
    }
}