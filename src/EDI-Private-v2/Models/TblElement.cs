using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblElement$")]
    public partial class TblElement
    {
        [Column("Element ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("Element Name")]
        public string Name { get; set; }

        [Column("Element Label")]
        public string Label { get; set; }

        [Column("Element Extended Definition")]
        public string ExtendedDefinition { get; set; }

        [Column("Element Question")]
        public string Question { get; set; }

        [Column("Element Type")]
        public string Type { get; set; }

        public bool? Vector { get; set; }

        [Column("EDFacts")]
        public bool? EDFacts { get; set; }

        [Column("IDEA")]
        public bool? IDEA { get; set; }
    }
}