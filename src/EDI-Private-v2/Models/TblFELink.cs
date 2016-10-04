using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblFELink$")]
    public partial class TblFELink
    {
        [Column("FE Link ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("DF Link ID")]
        public Guid DFLinkId { get; set; }

        [Column("Element ID")]
        public Guid ElementId { get; set; }
    }
}