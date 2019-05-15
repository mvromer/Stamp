using PathLib;
using System.IO.Abstractions;

namespace Stamp.CLI.Template
{
    class TemplateDirectoryValidator : ITemplateDirectoryValidator
    {
        public TemplateDirectoryValidator( IFileSystem fileSystem,
            ITemplateLoader templateLoader,
            ITemplateValidator templateValidator )
        {
            this.FileSystem = fileSystem;
            this.TemplateLoader = templateLoader;
            this.TemplateValidator = templateValidator;
        }

        public void Validate( IPurePath templateDir )
        {
            var manifestPath = templateDir.WithFilename( TemplateConstants.ManifestFileName );
            var template = this.TemplateLoader.LoadFromManifest( manifestPath );

            this.TemplateValidator.ValidateName( template.Name );

            // Now need to validate version number. That entails searching for the named template in
            // a repo that might be already synced locally.
        }

        private IFileSystem FileSystem { get; }
        private ITemplateLoader TemplateLoader { get; }
        private ITemplateValidator TemplateValidator { get; }
    }
}
