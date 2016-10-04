using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class SeriesStubViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    internal sealed class SeriesStubViewModelComparer : IEqualityComparer<SeriesStubViewModel>
    {
        public bool Equals(SeriesStubViewModel x, SeriesStubViewModel y) =>
            x.GetHashCode() == y.GetHashCode();

        public int GetHashCode(SeriesStubViewModel obj) =>
            obj.Id.GetHashCode() ^ obj.Name.GetHashCode();
    }
}