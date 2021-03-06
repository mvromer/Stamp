using PathLib;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Config
{
    interface IStampConfig
    {
        IPurePath RootPath { get; }

        IPurePath GetRepositoryPath( string repoName );

        IPurePath GetRepositoryPath( string repoName, bool createMissing );

        IPurePath GetLocalRepositoryPath();
    }
}
