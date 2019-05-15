using PathLib;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Config
{
    interface IStampConfig
    {
        IPurePath RootDir { get; }
    }
}
