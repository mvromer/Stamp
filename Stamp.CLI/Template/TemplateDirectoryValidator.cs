using PathLib;
using System.IO.Abstractions;

namespace Stamp.CLI.Template
{
    class TemplateDirectoryValidator : ITemplateDirectoryValidator
    {
        internal TemplateDirectoryValidator( IFileSystem fileSystem,
            ITemplateFactory templateFactory,
            ITemplateValidator templateValidator )
        {
            this.FileSystem = fileSystem;
            this.TemplateFactory = templateFactory;
            this.TemplateValidator = templateValidator;
        }

        public void Validate( IPurePath templateDir )
        {
            var manifestPath = templateDir.WithFilename( TemplateConstants.ManifestFileName );
            var template = this.TemplateFactory.CreateFromManifest( manifestPath );

            this.TemplateValidator.ValidateName( template.Name );
        }

        private IFileSystem FileSystem { get; }
        private ITemplateFactory TemplateFactory { get; }
        private ITemplateValidator TemplateValidator { get; }
    }
}
