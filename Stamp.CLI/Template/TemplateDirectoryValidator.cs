using PathLib;
using System.IO.Abstractions;

namespace Stamp.CLI.Template
{
    class TemplateDirectoryValidator : ITemplateDirectoryValidator
    {
        internal TemplateDirectoryValidator( IFileSystem fileSystem )
        {
            this.FileSystem = fileSystem;
        }

        public void Validate( IPurePath templateDir )
        {
        }

        private IFileSystem FileSystem { get; }
    }
}
