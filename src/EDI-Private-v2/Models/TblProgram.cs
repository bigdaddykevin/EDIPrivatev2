using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblProgram")]
    public partial class TblProgram
    {
        public TblProgram()
        {
            Studies = new HashSet<TblStudy>();
        }

        [Column("Program ID")]
        [Key]
        public int Id { get; set; }

        [Column("Division ID")]
        public int DivisionId { get; set; }

        [Column("Preceding Program ID")]
        public int? PrecedingId { get; set; }

        [Column("Following Program ID")]
        public int? FollowingId { get; set; }

        [Required]
        [Column("Program Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Column("Program Abbreviation")]
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        [Column("Program Old Name 1")]
        [MaxLength(255)]
        public string OldName1 { get; set; }

        [Column("Program Old Name 1 Abbr")]
        [MaxLength(255)]
        public string OldName1Abbreviation { get; set; }

        [Column("Program Old Name 1 Duration")]
        [MaxLength(255)]
        public string OldName1Duration { get; set; }

        [Column("Program Old Name 2")]
        [MaxLength(255)]
        public string OldName2 { get; set; }

        [Column("Program Old Name 2 Abbr")]
        [MaxLength(255)]
        public string OldName2Abbreviation { get; set; }

        [Column("Program Old Name 2 Duration")]
        [MaxLength(255)]
        public string OldName2Duration { get; set; }

        [Column("Program Description")]
        public string Description { get; set; }

        [Column("Primary Contact ID")]
        public int? PrimaryContactId { get; set; }

        [Column("2nd Contact ID")]
        public int? SecondContactId { get; set; }

        [Column("3rd Contact ID")]
        public int? ThirdContactId { get; set; }

        [Column("Supervisor ID")]
        public int? SupervisorId { get; set; }

        [Required]
        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [InverseProperty("Program")]
        public virtual ICollection<TblStudy> Studies { get; set; }

        [ForeignKey("DivisionId")]
        [InverseProperty("Programs")]
        public virtual TblDivision Division { get; set; }
    }
}