using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblDivision")]
    public partial class TblDivision
    {
        public TblDivision()
        {
            Programs = new HashSet<TblProgram>();
        }

        [Column("Division ID")]
        [Key]
        public int Id { get; set; }

        [Column("Unit ID")]
        public int UnitId { get; set; }

        [Required]
        [Column("Division Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Column("Division Abbreviation")]
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        [Column("Division Old Name 1")]
        [MaxLength(255)]
        public string OldName1 { get; set; }

        [Column("Division Old Name 1 Abbr")]
        [MaxLength(255)]
        public string OldName1Abbreviation { get; set; }

        [Column("Division Old Name 1 Duration")]
        [MaxLength(255)]
        public string OldName1Duration { get; set; }

        [Column("Division Old Name 2")]
        [MaxLength(255)]
        public string OldName2 { get; set; }

        [Column("Division Old Name 2 Abbr")]
        [MaxLength(255)]
        public string OldName2Abbreviation { get; set; }

        [Column("Division Old Name 2 Duration")]
        [MaxLength(255)]
        public string OldName2Duration { get; set; }

        [Column("Division Description")]
        public string Description { get; set; }

        [Column("SSMA_TimeStamp", TypeName = "timestamp")]
        public byte[] SsmaTimeStamp { get; set; }

        [InverseProperty("Division")]
        public virtual ICollection<TblProgram> Programs { get; set; }

        [ForeignKey("UnitId")]
        [InverseProperty("Divisions")]
        public virtual TblUnit Unit { get; set; }
    }
}