using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblDFLink$")]
    public partial class TblDFLink
    {
        [Column("DF Link ID")]
        [Key]
        public Guid Id { get; set; }

        [Column("SD Link ID")]
        public Guid SDLinkId { get; set; }

        [Column("File ID")]
        public Guid FileId { get; set; }
    }
}