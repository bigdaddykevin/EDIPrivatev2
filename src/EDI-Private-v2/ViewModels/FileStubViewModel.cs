using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDIPrivate.ViewModels
{
    public sealed class FileStubViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Format { get; set; }
        public string Restriction { get; set; }
        public IEnumerable<StudyStubViewModel> Studies { get; set; }
    }

    internal sealed class FileStubViewModelComparer : IEqualityComparer<FileStubViewModel>
    {
        public bool Equals(FileStubViewModel x, FileStubViewModel y) =>
            x.GetHashCode() == y.GetHashCode();

        public int GetHashCode(FileStubViewModel obj) =>
            obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Format.GetHashCode() ^ obj.Restriction.GetHashCode();
    }
}