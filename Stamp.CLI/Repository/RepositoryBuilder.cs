using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

using Stamp.CLI.Config;
using Stamp.CLI.Template;

namespace Stamp.CLI.Repository
{
    class RepositoryBuilder
    {
        public string Name { get; set; }

        public string Description { get; set; }

        internal IRepository Build( IFileSystem fileSystem, ITemplateLoader templateLoader, IStampConfig stampConfig,
            ILogger<RepositoryLoader> logger ) =>
            new Repository( this.Name,
                this.Description,
                stampConfig.GetRepositoryPath( this.Name ),
                fileSystem,
                templateLoader,
                logger );
    }
}
