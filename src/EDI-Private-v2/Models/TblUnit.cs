using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblUnit")]
    public partial class TblUnit
    {
        public TblUnit()
        {
            Divisions = new HashSet<TblDivision>();
        }

        [Column("Unit ID")]
        [Key]
        public int Id { get; set; }

        [Column("POC ID")]
        public int PrincipalOfficeId { get; set; }

        [Required]
        [Column("Unit Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Column("Unit Abbreviation")]
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        [Column("Unit Old Name 1")]
        [MaxLength(255)]
        public string OldName1 { get; set; }

        [Column("Unit Old Name 1 Abbr")]
        [MaxLength(255)]
        public string OldName1Abbreviation { get; set; }

        [Column("Unit Old Name 1 Duration")]
        [MaxLength(255)]
        public string OldName1Duration { get; set; }

        [Column("Unit Old Name 2")]
        [MaxLength(255)]
        public string OldName2 { get; set; }

        [Column("Unit Old Name 2 Abbr")]
        [MaxLength(255)]
        public string OldName2Abbreviation { get; set; }

        [Column("Unit Old Name 2 Duration")]
        [MaxLength(255)]
        public string OldName2Duration { get; set; }

        [InverseProperty("Unit")]
        public virtual ICollection<TblDivision> Divisions { get; set; }

        [ForeignKey("PrincipalOfficeId")]
        [InverseProperty("Units")]
        public virtual TblPrincipalOffice PrincipalOffice { get; set; }
    }
}