namespace diff
{
    public interface IDiffer
    {
        ObjectDiff GetDiff(byte[] left, byte[] right);
    }
}