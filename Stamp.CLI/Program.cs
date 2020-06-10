using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using McMaster.Extensions.CommandLineUtils;
using System.IO.Abstractions;
using SystemEnvironment.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

[assembly: CLSCompliant( isCompliant: false )]
[assembly: InternalsVisibleTo( "Stamp.Tests" )]
// Needed to make internal interfaces accessible to Moq's dynamic proxy generator.
[assembly: InternalsVisibleTo( "DynamicProxyGenAssembly2" )]

namespace Stamp.CLI
{
    [Command( Name = "stamp", Description = "Stamp template generator." )]
    [Subcommand( typeof(Commands.NewCommand) )]
    [Subcommand( typeof(Commands.ListCommand) )]
    [Subcommand( typeof(Commands.PublishCommand) )]
    [Subcommand( typeof(Commands.RepoCommand) )]
    class Program
    {
        static int Main( string[] args )
        {
            var services = new ServiceCollection()
                .AddLogging( builder => builder.AddConsole() )
                .AddSingleton<IConsole>( PhysicalConsole.Singleton )
                .AddTransient<IFileSystem, FileSystem>()
                .AddTransient<ISystemEnvironment, SystemEnvironment.Abstractions.SystemEnvironment>()
                .AddTransient<Config.IStampConfig, Config.StampConfig>()
                .AddTransient<Repository.IRepositoryLoader, Repository.RepositoryLoader>()
                .AddTransient<Template.ITemplateLoader, Template.TemplateLoader>()
                .AddTransient<IDeserializer>( _ =>
                    new DeserializerBuilder()
                        .WithNamingConvention( CamelCaseNamingConvention.Instance )
                        .WithNodeDeserializer( inner => new Template.ValidatingNodeDeserializer( inner ),
                            s => s.InsteadOf<ObjectNodeDeserializer>() )
                        .WithTypeConverter( new Template.TypeCodeTypeConverter() )
                        .WithTagMapping( Template.Builders.ChoiceValidatorBuilder.Tag,
                            typeof(Template.Builders.ChoiceValidatorBuilder) )
                        .Build() )
                .BuildServiceProvider();

            using( services )
            {
                var app = new CommandLineApplication<Program>();
                app.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection( services );
                return app.Execute( args );
            }
        }

        private void OnExecute( CommandLineApplication app ) => app.ShowHelp();
    }
}
