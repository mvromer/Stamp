using System;
using System.ComponentModel.DataAnnotations;
using PathLib;
using Stamp.CLI.Script;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template.Builders
{
    class FileBuilder
    {
        [Required]
        public string Path { get; set; }

        public bool Computed { get; set; }

        [YamlMember( Alias = "outputDir" )]
        public string OutputDirectory { get; set; }

        public string OutputName { get; set; }

        internal IFile Build()
        {
            // Inside our manifest model we normalize on Posix path syntax so that any manifest
            // errors regarding any file path uses the same syntax used by the manifest file.
            var path = new PurePosixPath( this.Path );

            // If either output directory or name isn't given, derive it from the source path.
            var outputDirectory = this.OutputDirectory ?? path.Directory;
            var outputName = this.OutputName ?? path.Filename;

            // There shouldn't be any directory component in the output name.
            var outputNamePath = new PurePosixPath( outputName );
            if( !string.IsNullOrEmpty( outputNamePath.Directory ) )
                throw new NotSupportedException( $"Output name {outputName} must not specify any directory components." );

            return new File(
                path.ToString(),
                this.Computed,
                new ComputedString( outputDirectory ),
                new ComputedString( outputName )
            );
        }
    }
}
