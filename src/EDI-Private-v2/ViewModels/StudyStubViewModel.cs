using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class StudyStubViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public SeriesStubViewModel Series { get; set; }
    }

    internal sealed class StudyStubViewModelComparer : IEqualityComparer<StudyStubViewModel>
    {
        public bool Equals(StudyStubViewModel x, StudyStubViewModel y) =>
            x.GetHashCode() == y.GetHashCode();

        public int GetHashCode(StudyStubViewModel obj) =>
            obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Series.GetHashCode();
    }
}