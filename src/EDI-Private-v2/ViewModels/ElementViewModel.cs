using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class ElementViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }

        [Display(Name = "Extended Label")]
        public string LabelExtended { get; set; }

        public string Question { get; set; }
        public IEnumerable<ValueViewModel> Values { get; set; }
    }
}