namespace diff
{
    public class DiffObject
    {
        public int Id { get; set; }
        public byte[] Left { get; set; }
        public byte[] Right { get; set; }
    }
}