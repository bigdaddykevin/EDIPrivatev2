using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblPackage")]
    public partial class TblPackage
    {
        [Column("Package ID")]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Package Title")]
        public string Title { get; set; }

        [Required]
        [Column("EDICS #")]
        public string EDICSNumber { get; set; }

        [Column("ICR ID Main")]
        public string ICRIdMain { get; set; }

        [Column("ICR ID Version")]
        public string ICRIdVersion { get; set; }

        [Column("ICR Ref No")]
        public string ICRReferenceNumber { get; set; }

        [Column("Office OMB #")]
        public string OfficeOMBNumber { get; set; }

        [Column("Study OMB #")]
        public string StudyOMBNumber { get; set; }

        [Column("Version OMB #")]
        public string VersionOMBNumber { get; set; }

        [Column("CFDA #")]
        public string CFDANumber { get; set; }

        [Column("Package Abstract")]
        public string Abstract { get; set; }

        [Column("Package Purpose")]
        public string Purpose { get; set; }

        [Column("Package Keywords")]
        public string Keywords { get; set; }

        [Column("Generic Clearance")]
        public bool? GenericClearance { get; set; }

        [Column("# Respondents")]
        public int? NumberRespondents { get; set; }

        [Column("# Responses")]
        public int? NumberResponses { get; set; }

        [Column("Percent Collected Electronically")]
        public double PercentCollectedElectronically { get; set; }

        [Column("Total Burden Hours")]
        public int? TotalBurdenHours { get; set; }

        [Column("Burden Hours Change")]
        public int? BurdenHoursChange { get; set; }

        [Column("Burden Hours Adjustment")]
        public int? BurdenHoursAdjustment { get; set; }

        [Column("Change Explanation")]
        public string ChangeExplanation { get; set; }

        public string Product { get; set; }

        [Column("Authorizing Law Cited")]
        public string AuthorizingLawCited { get; set; }

        [Column("Authorizing Law Text")]
        public string AuthorizingLawText { get; set; }

        [Column("Contractor Confidentiality Form")]
        public string ContractorConfidentialityForm { get; set; }

        [Column("Response to Public")]
        public bool? ResponseToPublic { get; set; }

        [Column("Public Comment Doc")]
        public string PublicCommentDocument { get; set; }

        [Column("Public Response Doc")]
        public string PublicResponseDocument { get; set; }

        [Column("OMB Passback")]
        public bool? OMBPassback { get; set; }

        [Column("OMB Response Doc")]
        public string OMBResponseDocument { get; set; }

        [Column("Type of Notice")]
        public string TypeOfNotice { get; set; }

        [Column("Date Notice Issued", TypeName = "date")]
        public DateTime? DateNoticeIssued { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Expiration { get; set; }

        public string TOC { get; set; }

        public string Status { get; set; }

        [Column("OA Flag")]
        public bool? OAFlag { get; set; }

        [Required]
        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }
    }
}