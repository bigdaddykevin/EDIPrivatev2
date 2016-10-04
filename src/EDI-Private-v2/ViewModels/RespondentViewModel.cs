using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class RespondentViewModel
    {
        [Key]
        public int Id { get; set; }

        public CollectionStubViewModel Collection { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public bool Voluntary { get; set; }

        public bool Mandatory { get; set; }

        [Display(Name = "Required for Benefits")]
        public bool RequiredForBenefits { get; set; }

        [Display(Name = "Response Requirement Description")]
        public string RequirementDescription { get; set; }

        [Display(Name = "Response Requirement Reason")]
        public string RequirementReason { get; set; }

        [Display(Name = "Estimated Population Size")]
        public int? PopulationSizeEstimated { get; set; }

        [Display(Name = "Population Size")]
        public int? PopulationSize { get; set; }

        [Display(Name = "Population Size Description")]
        public string PopulationSizeDescription { get; set; }

        [Display(Name = "Estimated Response Size")]
        public int? ResponseSizeEstimated { get; set; }

        [Display(Name = "Response Size")]
        public int? ResponseSize { get; set; }

        [Display(Name = "Estimated Response Rate")]
        public double? ResponseRateEstimated { get; set; }

        [Display(Name = "Response Rate")]
        public double? ResponseRate { get; set; }

        [Display(Name = "Response Rate Description")]
        public string ResponseRateDescription { get; set; }

        public int? Burden { get; set; }

        [Display(Name = "Burden Per Respondent")]
        public int? BurdenPerRespondent { get; set; }

        [Display(Name = "Survey Burden Per Respondent")]
        public int? BurdenPerRespondentSurvey { get; set; }

        [Display(Name = "Assessment Burden Per Respondent")]
        public int? BurdenPerRespondentAssessment { get; set; }

        [Display(Name = "Explicit")]
        public bool ConsentExplicit { get; set; }

        [Display(Name = "Implicit")]
        public bool ConsentImplicit { get; set; }

        [Display(Name = "Not Applicable")]
        public bool ConsentNotApplicable { get; set; }

        [Display(Name = "Consent Form")]
        public string ConsentForm { get; set; }

        public string Population { get; set; }

        [Display(Name = "Population Description")]
        public string PopulationDescription { get; set; }

        [Display(Name = "Response Type")]
        public string ResponseType { get; set; }

        [Display(Name = "Response Type Description")]
        public string ResponseTypeDescription { get; set; }

        [Display(Name = "Response Mode")]
        public string ResponseMode { get; set; }

        [Display(Name = "Response Mode Description")]
        public string ResponseModeDescription { get; set; }

        [Display(Name = "Instrument Additional Languages")]
        public string AdditionalLanguageInstrument { get; set; }

        [Display(Name = "Interpreter Additional Languages")]
        public string AdditionalLanguageInterpreter { get; set; }

        [Display(Name = "Cash Incentive Value")]
        public string IncentiveCashValue { get; set; }

        [Display(Name = "Cash Incentive Description")]
        public string IncentiveCashDescription { get; set; }

        [Display(Name = "Non-Cash Incentive Value")]
        public string IncentiveNonCashValue { get; set; }

        [Display(Name = "Non-Cash Incentive Description")]
        public string IncentiveNonCashDescription { get; set; }

        [Display(Name = "Incentive Description")]
        public string IncentiveJustification { get; set; }

        [Display(Name = "Confidentiality Law")]
        public string ConfidentialityLaw { get; set; }

        [Display(Name = "Instrument Voluntary & Confidentiality Statement")]
        public string VoluntaryConfidentialityStatementInstrument { get; set; }

        [Display(Name = "Contact Material Voluntary & Confidentiality Statement")]
        public string VoluntaryConfidentialityStatementContactMaterial { get; set; }

        [Display(Name = "FAQ Voluntary & Confidentiality Statement")]
        public string VoluntaryConfidentialityStatementFaq { get; set; }

        [Display(Name = "Brochure Voluntary & Confidentiality Statement")]
        public string VoluntaryConfidentialityStatementBrochure { get; set; }

        [Display(Name = "Follow-Up Informed Consent Statement")]
        public string FollowUpInformedConsentStatement { get; set; }

        [Display(Name = "Follow-Up Informed Consent Statement Location")]
        public string FollowUpInformedConsentLocation { get; set; }

        [Display(Name = "Paperwork Reduction Act Statement")]
        public string PaperworkReductionActStatement { get; set; }

        [Display(Name = "Paperwork Reduction Act Statement Location")]
        public string PaperworkReductionActLocation { get; set; }

        internal RespondentStubViewModel ToStub(bool forSearch = false) =>
            new RespondentStubViewModel()
            {
                Id = Id,
                Description = forSearch ? $"{Collection.Name} – {Description}" : Description,
                Collection = Collection
            };
    }
}