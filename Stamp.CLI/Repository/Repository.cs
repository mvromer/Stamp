namespace Stamp.CLI.Repository
{
    class Repository : IRepository
    {
        internal const string LocalRepositoryName = ".local";

        public string Name { get; }

        internal Repository( string name )
        {
            this.Name = name;
        }
    }
}
