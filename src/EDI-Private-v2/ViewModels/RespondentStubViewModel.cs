using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class RespondentStubViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public CollectionStubViewModel Collection { get; set; }
    }

    internal sealed class RespondentStubViewModelComparer : IEqualityComparer<RespondentStubViewModel>
    {
        public bool Equals(RespondentStubViewModel x, RespondentStubViewModel y) =>
            x.GetHashCode() == y.GetHashCode();

        public int GetHashCode(RespondentStubViewModel obj) =>
            obj.Id.GetHashCode() ^ obj.Description.GetHashCode() ^ obj.Collection.GetHashCode();
    }
}