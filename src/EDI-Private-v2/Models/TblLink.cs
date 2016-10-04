using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblLink")]
    public partial class TblLink
    {
        [Column("Link ID")]
        [Key]
        public int Id { get; set; }

        [Column("Collection ID")]
        public int CollectionId { get; set; }

        [Column("Package ID")]
        public int PackageId { get; set; }

        [ForeignKey("CollectionId")]
        [InverseProperty("Links")]
        public virtual TblCollection Collection { get; set; }
    }
}