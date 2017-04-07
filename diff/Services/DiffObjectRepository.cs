using System;
using System.Collections.Concurrent;

namespace diff
{
    /// <summary>
    /// InMemory storage implementation. This implementation won't survive server restart but is sufficient for demo project
    /// </summary>
    /// <seealso cref="diff.IDiffObjectRepository" />
    public class DiffObjectRepository : IDiffObjectRepository
    {
        private ConcurrentDictionary<int, DiffObject> diffObjects = new ConcurrentDictionary<int, DiffObject>();

        public bool TryGet(int id, out DiffObject diffObject)
        {
            return diffObjects.TryGetValue(id, out diffObject);
        }

        public DiffObject Get(int id)
        {
            return diffObjects[id];
        }

        /// <summary>
        /// Adds diff object if it does not exists, or updated if it exists
        /// </summary>
        /// <param name="diffObject">The diff object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">diffObject</exception>
        public DiffObject AddOrUpdate(DiffObject diffObject)
        {
            if (diffObject == null)
            {
                throw new ArgumentNullException(nameof(diffObject));
            }

            return diffObjects.AddOrUpdate(diffObject.Id, diffObject, (key, existingDiffObject) =>
            {
                //null is marker for no value
                if (diffObject.Left != null)
                {
                    existingDiffObject.Left = diffObject.Left;
                }

                //null is marker for no value
                if (diffObject.Right != null)
                {
                    existingDiffObject.Right = diffObject.Right;
                }

                return existingDiffObject;
            });
        }

        public void Clear()
        {
            diffObjects.Clear();
        }
    }


}