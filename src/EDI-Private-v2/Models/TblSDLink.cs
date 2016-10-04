using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblSDLink$")]
    public partial class TblSDLink
    {
        [Column("SD Link ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("Study ID")]
        public int StudyId { get; set; }

        [Column("Dataset ID")]
        public Guid DatasetId { get; set; }
    }
}