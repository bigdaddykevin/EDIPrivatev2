using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblPrincipalOffice")]
    public partial class TblPrincipalOffice
    {
        public TblPrincipalOffice()
        {
            Units = new HashSet<TblUnit>();
        }

        [Column("Principal Office ID")]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Principal Office Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Column("Principal Office Abbreviation")]
        [MaxLength(255)]
        public string Abbreviation { get; set; }

        [Column("Principal Office Old Name 1")]
        [MaxLength(255)]
        public string OldName1 { get; set; }

        [Column("Principal Office Old Name 1 Abbr")]
        [MaxLength(255)]
        public string OldName1Abbreviatoin { get; set; }

        [Column("Principal Office Old Name 1 Duration")]
        [MaxLength(255)]
        public string OldName1Duration { get; set; }

        [Column("Principal Office Old Name 2")]
        [MaxLength(255)]
        public string OldName2 { get; set; }

        [Column("Principal Office Old Name 2 Abbr")]
        [MaxLength(255)]
        public string OldName2Abbreviation { get; set; }

        [Column("Principal Office Old Name 2 Duration")]
        [MaxLength(255)]
        public string OldName2Duration { get; set; }

        [InverseProperty("PrincipalOffice")]
        public virtual ICollection<TblUnit> Units { get; set; }
    }
}