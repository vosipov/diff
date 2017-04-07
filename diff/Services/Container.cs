namespace diff
{
    /// <summary>
    /// Simple configuration class, stub for dependency injection
    /// First time we face different implementation requirement we have to rewrite this using dependency injection container
    /// </summary>
    public static class Container
    {
        public static IDiffObjectRepository DiffObjectRepository = new DiffObjectRepository();

        public static IDiffer Differ = new Differ();
    }
}