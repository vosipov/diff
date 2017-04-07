using System.Collections.Generic;

namespace diff
{
    public class Differ : IDiffer
    {
        /// <summary>
        /// Gets the difference.
        /// </summary>
        /// <returns>ObjectDiff representing difference between left and right</returns>
        public ObjectDiff GetDiff(byte[] left, byte[] right)
        {
            //edge case left is null or empty
            if (left == null || left.Length == 0)
            {
                if (right == null || right.Length == 0)
                {
                    return new ObjectDiff { diffResultType = DiffResultType.Equals };
                }
                return new ObjectDiff { diffResultType = DiffResultType.SizeDoNotMatch };
            }

            //edge case right is null or empty but left is not empty
            if (right == null || right.Length == 0)
            {
                return new ObjectDiff { diffResultType = DiffResultType.SizeDoNotMatch };
            }

            //both left and right has lenghth > 0, test if lengthes are equal
            if (left.Length != right.Length)
            {
                return new ObjectDiff { diffResultType = DiffResultType.SizeDoNotMatch };
            }

            //left and right has equal length, compare content
            List<ContentDiff> contentDiffs = new List<ContentDiff>();
            int offset = 0;
            int length = 0;

            //scan through to find all diff sequences
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    //test if it is a beginning of new diff sequence
                    if (length == 0)
                    {
                        //mark where it starts
                        offset = i;
                    }
                    //increase the diff length
                    length++;
                }
                else if (length > 0) //test if diff sequence ends here
                {
                    //report new diff sequence
                    contentDiffs.Add(new ContentDiff
                    {
                        offset = offset,
                        length = length
                    });
                    //reset diff length
                    length = 0;
                }
            }

            //every byte different
            if (length > 0)
            {
                contentDiffs.Add(new ContentDiff
                {
                    offset = offset,
                    length = length
                });
            }

            ObjectDiff objectDiff = new ObjectDiff
            {
                diffResultType = DiffResultType.Equals,
            };

            //if there were different sequences mark result as ContentDoNotMatch
            if (contentDiffs.Count > 0)
            {
                objectDiff.diffResultType = DiffResultType.ContentDoNotMatch;
                objectDiff.diffs = contentDiffs;
            }

            return objectDiff;
        }
    }
}