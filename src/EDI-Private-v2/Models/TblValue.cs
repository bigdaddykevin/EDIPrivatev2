using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblValue$")]
    public partial class TblValue
    {
        [Column("Value ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("Value Option")]
        public string Option { get; set; }

        [Column("Value Label")]
        public string Label { get; set; }
    }
}