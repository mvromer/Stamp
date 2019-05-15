using System;

namespace Environment.Abstractions
{
    public class SystemEnvironment : ISystemEnvironment
    {
        public string GetFolderPath( System.Environment.SpecialFolder folder,
            System.Environment.SpecialFolderOption option )
        {
            return System.Environment.GetFolderPath( folder, option );
        }
    }
}
