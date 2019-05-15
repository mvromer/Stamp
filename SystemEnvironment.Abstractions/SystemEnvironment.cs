using System;

namespace SystemEnvironment.Abstractions
{
    public class SystemEnvironment : ISystemEnvironment
    {
        public string GetFolderPath( Environment.SpecialFolder folder, Environment.SpecialFolderOption option )
        {
            return System.Environment.GetFolderPath( folder, option );
        }
    }
}
