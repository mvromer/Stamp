using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using PathLib;

using Stamp.CLI.Template;

namespace Stamp.CLI.Repository
{
    class Repository : IRepository
    {
        public string Name { get; }

        public string Description { get; }

        public IPurePath RootPath { get; }

        public ImmutableList<ITemplate> Templates => _templates.Value;
        private Lazy<ImmutableList<ITemplate>> _templates;

        internal Repository( string name, string description, IPurePath rootPath, IFileSystem fileSystem,
            ITemplateLoader templateLoader, ILogger<RepositoryLoader> logger )
        {
            this.Name = name;
            this.Description = description;
            this.RootPath = rootPath;
            _templates = new Lazy<ImmutableList<ITemplate>>( () => LoadTemplates( fileSystem, templateLoader, logger ) );
        }

        private ImmutableList<ITemplate> LoadTemplates( IFileSystem fileSystem, ITemplateLoader templateLoader,
            ILogger<RepositoryLoader> logger )
        {
            var templates = ImmutableList.CreateBuilder<ITemplate>();
            var templatePaths = fileSystem.Directory.
                EnumerateDirectories( this.RootPath.Join( "templates" ).ToString() ).
                Select( path => PurePath.Create( path ) );

            foreach( var templatePath in templatePaths )
            {
                try
                {
                    var templateFolderName = templatePath.Basename;
                    ValidateTemplateFolderName( templateFolderName );

                    var p = PurePath.Create( "/opt/stamp/repos/TestRepo/templates/TestTemplate@1" );
                    var template = templateLoader.LoadFromTemplateDirectory( templatePath );
                    ValidateTemplateAgainstTemplateFolderName( template, templateFolderName );
                    templates.Add( template );
                }
                catch( Exception ex )
                {
                    logger.LogWarning( ex, $"Failed to load template from {templatePath}." );
                    throw;
                }
            }

            return templates.ToImmutable();
        }

        /// <summary>
        /// Ensures the template folder name at the given path is of the form
        /// TemplateName@MajorVersion.
        /// </summary>
        /// <param name="templateFolderName">Template folder name to validate.</param>
        /// <exception cref="ValidationException">
        /// If the template folder name is invalid.
        /// </exception>
        private void ValidateTemplateFolderName( string templateFolderName )
        {
            if( !Repository.TemplateFolderNamePattern.IsMatch( templateFolderName ) )
            {
                throw new ValidationException( $"Invalid template folder name {templateFolderName} found for " +
                    $"repository {this.Name}. Folder name must follow pattern <template name>@<major version>." );
            }
        }

        /// <summary>
        /// Ensures the template name and major version match those in the template folder name.
        /// </summary>
        /// <param name="template">Template object whose name and major version to check.</param>
        /// <param name="templateFolderName">Template folder name to validate against.</param>
        /// <exception cref="ValidationException">
        /// If either the template name or template major version does not match the corresponding
        /// components in the template folder name.
        /// </exception>
        private void ValidateTemplateAgainstTemplateFolderName( ITemplate template, string templateFolderName )
        {
            var match = Repository.TemplateFolderNamePattern.Matches( templateFolderName ).First();

            if( !string.Equals( template.Name, match.Groups["name"].Value, StringComparison.OrdinalIgnoreCase ) )
            {
                throw new ValidationException( $"Template name {template.Name} does not match name in template " +
                    $"folder name {templateFolderName}." );
            }

            if( template.Version.Major != int.Parse( match.Groups["majorVersion"].Value ) )
            {
                throw new ValidationException( $"Template major version {template.Version.Major} does not match " +
                    $"major version in template folder name {templateFolderName}." );
            }
        }

        // Template folder name is a template name followed by an at-sign (@) followed by a
        // nonnegative number representing the major version, e.g., MyTemplate@10.
        private static Regex TemplateFolderNamePattern { get; } =
            new Regex( $@"^(?<name>{TemplateValidationConstants.TemplateNamePatternString})@(?<majorVersion>0|(?:[1-9][0-9]*))$" );
    }
}
