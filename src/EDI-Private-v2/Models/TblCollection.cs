using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblCollection")]
    public partial class TblCollection
    {
        public TblCollection()
        {
            Links = new HashSet<TblLink>();
            Respondents = new HashSet<TblRespondent>();
        }

        [Column("Collection ID")]
        [Key]
        public int Id { get; set; }

        [Column("Detail ID")]
        public int? DetailId { get; set; }

        [Required]
        [Column("Collection Name")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Column("Collection Type")]
        [MaxLength(255)]
        public string Type { get; set; }

        public int? Cost { get; set; }

        [Column("Collection Cost Years")]
        [MaxLength(255)]
        public string CostYears { get; set; }

        [Column("Cost Details")]
        public string CostDetails { get; set; }

        [Column("Experiments Description")]
        public string ExperimentsDescription { get; set; }

        [Column("Experiment Results")]
        public string ExperimentResults { get; set; }

        [Column("Recruitment Start Date", TypeName = "date")]
        public DateTime? RecruitmentStartDate { get; set; }

        [Column("Collection Start Date (Planned)", TypeName = "date")]
        public DateTime? StartDatePlanned { get; set; }

        [Column("Collection Start Date (Actual)", TypeName = "date")]
        public DateTime? StartDateActual { get; set; }

        [Column("Collection End Date (Actual)", TypeName = "date")]
        public DateTime? EndDatePlanned { get; set; }

        [Column("Collection End Date (Planned)", TypeName = "date")]
        public DateTime? EndDateActual { get; set; }

        [Column("Date Detail")]
        public string DateDetail { get; set; }

        [Column("Collection Type")]
        [MaxLength(255)]
        public string CollectorType { get; set; }

        [Column("Collectors Names")]
        [MaxLength(255)]
        public string CollectorsNames { get; set; }

        [Column("Subcollectors Names")]
        [MaxLength(255)]
        public string SubcollectorsNames { get; set; }

        [Column("VC Language A10")]
        public string VCLanguageA10 { get; set; }

        [Column("Confidentiality Language")]
        public string ConfidentialityLanguage { get; set; }

        [Column("Relevant Conf Law")]
        [MaxLength(255)]
        public string RelevantConfidentialityLaw { get; set; }

        [Column("Survey FAQ")]
        public bool? SurveyFaq { get; set; }

        [Column("Survey Brochure")]
        public bool? SurveyBrochure { get; set; }

        [Column("Total # Variables")]
        public int? TotalVariables { get; set; }

        [Column("# Variables w/o Replicate Weights & Imputation Flags")]
        public int? VariablesWithoutReplicateWeightsAndImputationFlags { get; set; }

        [Required]
        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [InverseProperty("Collection")]
        public virtual ICollection<TblLink> Links { get; set; }

        [InverseProperty("Collection")]
        public virtual ICollection<TblRespondent> Respondents { get; set; }
    }
}