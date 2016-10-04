using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class SeriesViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Symbol { get; set; }

        [Display(Name = "Old Name 1")]
        public string OldName1 { get; set; }

        [Display(Name = "Old Name 1 Symbol")]
        public string OldName1Symbol { get; set; }

        [Display(Name = "Old Name 1 Duration")]
        public string OldName1Duration { get; set; }

        [Display(Name = "Old Name 2")]
        public string OldName2 { get; set; }

        [Display(Name = "Old Name 2 Symbol")]
        public string OldName2Symbol { get; set; }

        [Display(Name = "Old Name 2 Duration")]
        public string OldName2Duration { get; set; }

        [Display(Name = "Parent Organization")]
        public string ParentOrganization { get; set; }

        public string Description { get; set; }
        public IEnumerable<StudyStubViewModel> Studies { get; set; }

        internal SeriesStubViewModel ToStub() =>
            new SeriesStubViewModel()
            {
                Id = Id,
                Name = Name
            };
    }
}