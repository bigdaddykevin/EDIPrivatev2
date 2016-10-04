using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class CollectionStubViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public StudyStubViewModel Study { get; set; }
    }

    internal sealed class CollectionStubViewModelComparer : IEqualityComparer<CollectionStubViewModel>
    {
        public bool Equals(CollectionStubViewModel x, CollectionStubViewModel y) =>
            x.GetHashCode() == y.GetHashCode();

        public int GetHashCode(CollectionStubViewModel obj) =>
            obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Study.GetHashCode();
    }
}