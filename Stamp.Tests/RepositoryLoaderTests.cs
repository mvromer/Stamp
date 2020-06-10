using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PathLib;
using Semver;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

using Stamp.CLI.Config;
using Stamp.CLI.Repository;
using Stamp.CLI.Template;

namespace Stamp.Tests
{
    public class RepositoryLoaderTests
    {
        [Fact]
        public void ItCanLoadLocalRepositoryWithTemplates()
        {
            var logger = NullLogger<RepositoryLoader>.Instance;
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory( "/opt/stamp/repos/.local/templates/TestTemplate@1" );

            var stampConfig = Mock.Of<IStampConfig>( config =>
                config.GetLocalRepositoryPath() == PurePath.Create( "/opt/stamp/repos/.local" )
            );

            var expectedTemplateName = "TestTemplate";
            var expectedTemplateVersion = SemVersion.Parse( "1.0.0" );
            var templateLoader = Mock.Of<ITemplateLoader>( loader =>
                loader.LoadFromTemplateDirectory( It.IsAny<IPurePath>() ) == Mock.Of<ITemplate>( template =>
                    template.Name == expectedTemplateName &&
                    template.Version == expectedTemplateVersion
                )
            );

            var repositoryLoader = new RepositoryLoader( fileSystem, templateLoader, stampConfig, logger );
            var repository = repositoryLoader.LoadLocalRepository();
            repository.Should().NotBeNull();
            repository.Name.Should().Be( ".local" );
            repository.Description.Should().Be( "Local repository" );
            repository.Templates.Count.Should().Be( 1 );
            repository.Templates[0].Name.Should().Be( expectedTemplateName );
            repository.Templates[0].Version.Should().Be( expectedTemplateVersion );
        }
    }
}
