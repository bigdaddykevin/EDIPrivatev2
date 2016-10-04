using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblStudy")]
    public partial class TblStudy
    {
        public TblStudy()
        {
            FollowUps = new HashSet<TblFollowUp>();
        }

        [Column("Study ID")]
        [Key]
        public int Id { get; set; }

        [Column("Program ID")]
        public int ProgramId { get; set; }

        [Required]
        [Column("Study Name")]
        [MaxLengthAttribute(255)]
        public string Name { get; set; }

        [Required]
        [Column("Study Abbreviation")]
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        [Column("Study Old Name 1")]
        [MaxLength(255)]
        public string OldName1 { get; set; }

        [Column("Study Old Name 1 Abbr")]
        [MaxLength(255)]
        public string OldName1Abbreviation { get; set; }

        [Column("Study Old Name 1 Duration")]
        [MaxLength(255)]
        public string OldName1Duration { get; set; }

        [Column("Study Old Name 2")]
        [MaxLength(255)]
        public string OldName2 { get; set; }

        [Column("Study Old Name 2 Abbr")]
        [MaxLength(255)]
        public string OldName2Abbreviation { get; set; }

        [Column("Study Old Name 2 Duration")]
        [MaxLength(255)]
        public string OldName2Duration { get; set; }

        [Column("JSON Unique Identifier")]
        [MaxLength(255)]
        public string JsonUniqueIdentifier { get; set; }

        [Column("Study Website")]
        public string Website { get; set; }

        [Column("Study Abstract")]
        public string Abstract { get; set; }

        public string Frequency { get; set; }

        [Column("Most Recent Year")]
        [MaxLength(255)]
        public string MostRecentYear { get; set; }

        [Required]
        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [InverseProperty("Study")]
        public virtual ICollection<TblFollowUp> FollowUps { get; set; }

        [ForeignKey("ProgramId")]
        [InverseProperty("Studies")]
        public virtual TblProgram Program { get; set; }
    }
}