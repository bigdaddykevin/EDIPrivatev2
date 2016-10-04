using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class ContactViewModel
    {
        [Key]
        public int Id { get; set; }

        public int SortOrder { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }
    }
}