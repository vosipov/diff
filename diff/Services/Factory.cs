namespace diff
{
    /// <summary>
    /// Simple configuration class to respect dependency injection without abusing container frameworks.
    /// First time we face different implementation requirement we have to rewrite this using dependency injection container
    /// </summary>
    public static class Factory
    {
        public static IDiffObjectRepository DiffObjectRepository = new DiffObjectRepository();

        public static IDiffer Differ = new Differ();
    }
}