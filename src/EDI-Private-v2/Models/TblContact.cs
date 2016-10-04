using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIPrivate.Models
{
    [Table("tblContact")]
    public partial class TblContact
    {
        [Column("Contact ID")]
        [Key]
        public int Id { get; set; }

        [Column("Contact First Name")]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Column("Contact Last Name")]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Column("Contact Email")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Column("Contact Phone")]
        [MaxLength(255)]
        public string Phone { get; set; }
    }
}