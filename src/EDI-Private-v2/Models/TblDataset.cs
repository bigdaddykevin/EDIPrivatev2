using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblDataset$")]
    public partial class TblDataset
    {
        [Column("Dataset ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("Dataset Title")]
        public string Title { get; set; }

        [Column("Dataset Location")]
        public string Location { get; set; }

        [Column("Dataset Location Detail")]
        public string LocationDetail { get; set; }

        [Column("Dataset Restriction")]
        public int? Restriction { get; set; }
    }
}