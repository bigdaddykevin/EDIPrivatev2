using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblEVLink$")]
    public partial class TblEVLink
    {
        [Column("EV Link ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("FE Link ID")]
        public Guid FELinkId { get; set; }

        [Column("Value ID")]
        public Guid ValueId { get; set; }
    }
}