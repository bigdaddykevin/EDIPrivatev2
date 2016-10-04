using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblFile$")]
    public partial class TblFile
    {
        [Column("File ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("File Name")]
        public string Name { get; set; }

        [Column("File Format")]
        [MaxLength(255)]
        public string Format { get; set; }
    }
}