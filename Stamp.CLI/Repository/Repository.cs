namespace Stamp.CLI.Repository
{
    class Repository : IRepository
    {
        public string Name { get; }

        public string Description { get; }

        internal Repository( string name, string description )
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
