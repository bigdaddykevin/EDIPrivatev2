using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class PackageStubViewModel
    {
        [Key]
        public int Id { get; set; }

        public string ReferenceNumber { get; set; }
        public IEnumerable<CollectionStubViewModel> Collections { get; set; }
    }

    internal sealed class PackageStubViewModelComparer : IEqualityComparer<PackageStubViewModel>
    {
        public bool Equals(PackageStubViewModel x, PackageStubViewModel y) =>
            x.GetHashCode() == y.GetHashCode();

        public int GetHashCode(PackageStubViewModel obj) =>
            obj.Id.GetHashCode() ^ obj.ReferenceNumber.GetHashCode() ^ obj.Collections.GetHashCode();
    }
}