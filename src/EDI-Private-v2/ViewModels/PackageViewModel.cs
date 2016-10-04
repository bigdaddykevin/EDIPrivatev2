using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EDIPrivate.ViewModels
{
    public sealed class PackageViewModel
    {
        [Key]
        public int Id { get; set; }

        public string ICRAS { get; set; }
        public string EDICS { get; set; }

        [Display(Name = "ICR Reference Number")]
        public string ICRReferenceNumber { get; set; }

        [Display(Name = "OMB Control Number")]
        public string OMBControlNumber { get; set; }

        public string CFDA { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Abstract { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        public DateTime? IssueDate { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "Notice Type")]
        public string NoticeType { get; set; }

        [Display(Name = "Terms of Clearance")]
        public string TermsOfClearance { get; set; }

        [Display(Name = "Number of Respondents")]
        public int? NumberRespondents { get; set; }

        [Display(Name = "Number of Responses")]
        public int? NumberResponses { get; set; }

        [Display(Name = "Percent Collected Electronically")]
        public double? PercentCollectedElectronically { get; set; }

        [Display(Name = "Total Burden")]
        public int? BurdenTotal { get; set; }

        [Display(Name = "Burden Change")]
        public int? BurdenChange { get; set; }

        [Display(Name = "Burden Adjustment")]
        public int? BurdenAdjustment { get; set; }

        [Display(Name = "Burden Explanation")]
        public string BurdenExplanation { get; set; }

        [Display(Name = "Public Comment Document")]
        public string PublicComment { get; set; }

        [Display(Name = "Public Comment Response Document")]
        public string PublicCommentResponse { get; set; }

        [Display(Name = "OMB Passback Document")]
        public string OMBPassback { get; set; }

        [Display(Name = "Authorizing Law Cited")]
        public string AuthorizingLawCited { get; set; }

        [Display(Name = "Authorizing Law Text")]
        public string AuthorizingLawText { get; set; }

        [Display(Name = "Contractor Confidentiality Form Location")]
        public string ContractorConfidentialityFormLocation { get; set; }

        public IEnumerable<CollectionStubViewModel> Collections { get; set; }

        internal PackageStubViewModel ToStub() =>
            new PackageStubViewModel()
            {
                Id = Id,
                ReferenceNumber =
                    new[] {
                        ICRAS,
                        EDICS,
                        ICRReferenceNumber,
                        OMBControlNumber
                    }.FirstOrDefault(str => !string.IsNullOrWhiteSpace(str)),
                Collections = Collections
            };
    }
}