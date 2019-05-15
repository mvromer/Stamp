using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace SystemEnvironment.Abstractions.Tests
{
    public class SystemEnvironmentTests
    {
        [Fact]
        public void ItCanGetFolderPathWithDefaultOption()
        {
            var expected = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile,
                Environment.SpecialFolderOption.None );
            var actual = new SystemEnvironment().GetFolderPath( Environment.SpecialFolder.UserProfile,
                Environment.SpecialFolderOption.None );

            actual.Should().Be( expected );
        }

        [Fact]
        public void ItCanGetFolderPathWithNoVerifyOption()
        {
            var expected = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.DoNotVerify );
            var actual = new SystemEnvironment().GetFolderPath( Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.DoNotVerify );

            actual.Should().Be( expected );
        }

        [Fact]
        public void ItCanGetFolderPathWithCreateOption()
        {
            var expected = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile,
                Environment.SpecialFolderOption.Create );
            var actual = new SystemEnvironment().GetFolderPath( Environment.SpecialFolder.UserProfile,
                Environment.SpecialFolderOption.Create );

            actual.Should().Be( expected );
        }

        [Fact]
        public void ItCanMockGetFolderPath()
        {
            const string mockedUserProfile = "/home/Environment.Abstractions";
            var mockEnvironment = Mock.Of<ISystemEnvironment>( e =>
                e.GetFolderPath( Environment.SpecialFolder.UserProfile,
                    Environment.SpecialFolderOption.Create ) == mockedUserProfile );

            var actual = mockEnvironment.GetFolderPath( Environment.SpecialFolder.UserProfile,
                Environment.SpecialFolderOption.Create );

            actual.Should().Be( mockedUserProfile );
        }
    }
}
