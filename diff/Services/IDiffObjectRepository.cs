namespace diff
{
    public interface IDiffObjectRepository
    {
        bool TryGet(int id, out DiffObject diffObject);

        DiffObject Get(int id);

        DiffObject AddOrUpdate(DiffObject diffObject);

        void Clear();
    }
}