using System.Collections.Generic;
using System.Collections.ObjectModel;
using Semver;

namespace Stamp.CLI.Template
{
    class Template : ITemplate
    {
        public string Name { get; }

        public SemVersion Version { get; }

        public IReadOnlyCollection<IParameter> Parameters { get; }

        public IReadOnlyCollection<IFile> Files { get; }

        internal Template( string name, SemVersion version, IList<IParameter> parameters, IList<IFile> files )
        {
            this.Name = name;
            this.Version = version;
            this.Parameters = new ReadOnlyCollection<IParameter>( parameters );
            this.Files = new ReadOnlyCollection<IFile>( files );
        }
    }
}
