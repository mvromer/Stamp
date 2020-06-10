using System;
using System.ComponentModel.DataAnnotations;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PathLib;
using Semver;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

using Stamp.CLI.Repository;
using Stamp.CLI.Template;

namespace Stamp.Tests
{
    public class RepositoryTests
    {
        public RepositoryTests()
        {
            this.Logger = NullLogger<RepositoryLoader>.Instance;
            this.FileSystem = new MockFileSystem();
            this.RepositoryRootPath = PurePath.Create( "/opt/stamp/repos/TestRepo" );
            this.RepositoryName = "TestRepo";
            this.RepositoryDescription = "Test repository";
            this.TemplateName = "TestTemplate";
            this.TemplateVersion = SemVersion.Parse( "1.0.0" );
        }

        private ILogger<RepositoryLoader> Logger { get; }
        private MockFileSystem FileSystem { get; }

        private IPurePath RepositoryRootPath { get; }
        private string RepositoryName { get; }
        private string RepositoryDescription { get; }
        private string TemplateName { get; }
        private SemVersion TemplateVersion { get; }

        private IPurePath AddTestTemplatePath( string templateDirectoryName )
        {
            var testTemplatePath = this.RepositoryRootPath.Join( "templates", templateDirectoryName );
            this.FileSystem.AddDirectory( testTemplatePath.ToString() );
            return testTemplatePath;
        }

        private ITemplateLoader BuildTemplateLoader( IPurePath testTemplatePath )
        {
            return Mock.Of<ITemplateLoader>( loader =>
                loader.LoadFromTemplateDirectory( It.Is<IPurePath>( path => path.Basename == testTemplatePath.Basename ) ) == Mock.Of<ITemplate>( template =>
                    template.Name == this.TemplateName &&
                    template.Version == this.TemplateVersion
                )
            );
        }

        [Fact]
        public void ItCanLoadRepositoryTemplates()
        {
            var testTemplatePath = AddTestTemplatePath( templateDirectoryName: "TestTemplate@1" );
            var templateLoader = BuildTemplateLoader( testTemplatePath );

            var repository = new Repository( this.RepositoryName, this.RepositoryDescription, this.RepositoryRootPath,
                this.FileSystem, templateLoader, this.Logger );
            repository.Name.Should().Be( this.RepositoryName );
            repository.Description.Should().Be( this.RepositoryDescription );
            repository.RootPath.Should().Be( this.RepositoryRootPath );
            repository.Templates.Count.Should().Be( 1 );
            repository.Templates[0].Name.Should().Be( this.TemplateName );
            repository.Templates[0].Version.Should().Be( this.TemplateVersion );
        }

        [Fact]
        public void ItFailsToLoadRepositoryTemplateWithInvalidTemplateFolderName()
        {
            var testTemplatePath = AddTestTemplatePath( templateDirectoryName: "Test+Template@1" );
            var templateLoader = BuildTemplateLoader( testTemplatePath );

            var repository = new Repository( this.RepositoryName, this.RepositoryDescription, this.RepositoryRootPath,
                this.FileSystem, templateLoader, this.Logger );

            Action act = () => { var templates = repository.Templates; };
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void ItFailsToLoadRepositoryTemplateWithTemplateFolderMissingVersion()
        {
            var testTemplatePath = AddTestTemplatePath( templateDirectoryName: "TestTemplate" );
            var templateLoader = BuildTemplateLoader( testTemplatePath );

            var repository = new Repository( this.RepositoryName, this.RepositoryDescription, this.RepositoryRootPath,
                this.FileSystem, templateLoader, this.Logger );

            Action act = () => { var templates = repository.Templates; };
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void ItFailsToLoadRepositoryTemplateWithMismatchingName()
        {
            var testTemplatePath = AddTestTemplatePath( templateDirectoryName: "MismatchedTestTemplate@1" );
            var templateLoader = BuildTemplateLoader( testTemplatePath );

            var repository = new Repository( this.RepositoryName, this.RepositoryDescription, this.RepositoryRootPath,
                this.FileSystem, templateLoader, this.Logger );

            Action act = () => { var templates = repository.Templates; };
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void ItFailsToLoadRepositoryTemplateWithMismatchingVersion()
        {
            var testTemplatePath = AddTestTemplatePath( templateDirectoryName: "TestTemplate@100" );
            var templateLoader = BuildTemplateLoader( testTemplatePath );

            var repository = new Repository( this.RepositoryName, this.RepositoryDescription, this.RepositoryRootPath,
                this.FileSystem, templateLoader, this.Logger );

            Action act = () => { var templates = repository.Templates; };
            act.Should().Throw<ValidationException>();
        }
    }
}
