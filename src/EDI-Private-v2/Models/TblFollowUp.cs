using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblFollow-Up")]
    public partial class TblFollowUp
    {
        public TblFollowUp()
        {
            Details = new HashSet<TblDetail>();
        }

        [Column("Follow-Up ID")]
        [Key]
        public int Id { get; set; }

        [Column("Study ID")]
        public int StudyId { get; set; }

        [Required]
        [Column("Follow-Up Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Column("Follow-Up Abbreviation")]
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        [Column("Follow-up Abstract")]
        public string Abstract { get; set; }

        [Column("Longitudinal Follow-Up")]
        public int? Longitudinal { get; set; }

        [Column("Preceding Follow-Up ID")]
        public int? PrecedingId { get; set; }

        [Required]
        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [InverseProperty("FollowUp")]
        public virtual ICollection<TblDetail> Details { get; set; }

        [ForeignKey("StudyId")]
        [InverseProperty("FollowUps")]
        public virtual TblStudy Study { get; set; }
    }
}