using System;

namespace SystemEnvironment.Abstractions
{
    public interface ISystemEnvironment
    {
        string GetFolderPath( Environment.SpecialFolder folder, Environment.SpecialFolderOption option );
    }
}
