using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class CollectionViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public StudyStubViewModel Study { get; set; }

        [Display(Name = "Collection Type")]
        public string Type { get; set; }

        [DataType(DataType.Currency)]
        public int? Cost { get; set; }

        [Display(Name = "Cost Years")]
        public string CostYears { get; set; }

        [Display(Name = "Cost Description")]
        public string CostDescription { get; set; }

        [Display(Name = "Estimated Recruitment Start Date")]
        [DataType(DataType.Date)]
        public DateTime? RecruitmentStartDateEstimated { get; set; }

        [Display(Name = "Estimated Collection Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDateEstimated { get; set; }

        [Display(Name = "Collection Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Estimated Collection End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDateEstimated { get; set; }

        [Display(Name = "Collection End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Date Description")]
        public string DateDescription { get; set; }

        [Display(Name = "Data Collection Agent Type")]
        public string DataCollectionAgentType { get; set; }

        [Display(Name = "Primary Data Collection Agent")]
        public string DataCollectionAgentPrimary { get; set; }

        [Display(Name = "Non-Primary Data Collection Agent")]
        public string DataCollectionAgentNonPrimary { get; set; }

        [Display(Name = "Confidentiality Law")]
        public string ConfidentialityLaw { get; set; }

        [Display(Name = "Voluntary & Confidentiality Statement")]
        public string VoluntaryConfidentialityStatement { get; set; }

        [Display(Name = "Respondent Voluntary & Confidentiality Statement")]
        public string VoluntaryConfidentialityStatementRespondent { get; set; }

        [Display(Name = "Experiment Description")]
        public string ExperimentDescription { get; set; }

        [Display(Name = "Experiment Results")]
        public string ExperimentResults { get; set; }

        public IEnumerable<RespondentStubViewModel> Respondents { get; set; }
        public IEnumerable<PackageStubViewModel> Packages { get; set; }

        internal CollectionStubViewModel ToStub() =>
            new CollectionStubViewModel()
            {
                Id = Id,
                Name = Name,
                Study = Study
            };
    }
}