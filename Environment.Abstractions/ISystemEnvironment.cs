using System;

namespace Environment.Abstractions
{
    public interface ISystemEnvironment
    {
        string GetFolderPath( System.Environment.SpecialFolder folder, System.Environment.SpecialFolderOption option );
    }
}
