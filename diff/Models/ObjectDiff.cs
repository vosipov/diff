using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace diff
{
    public class ObjectDiff
    {
        public DiffResultType diffResultType { get; set; }
        public List<ContentDiff> diffs { get; set; }
    }

    public class ContentDiff
    {
        public long offset { get; set; }
        public long length { get; set; }
    }

    public enum DiffResultType
    {
        Equals,
        SizeDoNotMatch,
        ContentDoNotMatch
    }
}