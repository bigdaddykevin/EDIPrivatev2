using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class FileViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Format { get; set; }
        public string Dataset { get; set; }
        public string Restriction { get; set; }
        public string Location { get; set; }

        [Display(Name = "Location Description")]
        public string LocationDescription { get; set; }

        public IEnumerable<StudyStubViewModel> Studies { get; set; }
        public IEnumerable<ElementViewModel> Elements { get; set; }

        internal FileStubViewModel ToStub() =>
            new FileStubViewModel()
            {
                Id = Id,
                Name = Name,
                Format = Format,
                Restriction = Restriction,
                Studies = Studies
            };
    }
}