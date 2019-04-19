using System;
using PathLib;

namespace Stamp.CLI.Utility
{
    static class PathLibUtility
    {
        internal static PathFactory Factory
        {
            get { return m_pathFactory.Value; }
        }

        private static Lazy<PathFactory> m_pathFactory = new Lazy<PathFactory>( () => new PathFactory() );
    }
}
