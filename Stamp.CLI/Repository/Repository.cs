namespace Stamp.CLI.Repository
{
    class Repository : IRepository
    {
        public string Name { get; }

        public string Description { get; }

        internal static IRepository LocalInstance { get; } =
            new Repository( name: ".local", description: "Local repository" );

        internal Repository( string name, string description )
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
