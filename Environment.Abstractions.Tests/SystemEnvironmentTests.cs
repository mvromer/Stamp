using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace Environment.Abstractions.Tests
{
    public class SystemEnvironmentTests
    {
        [Fact]
        public void ItCanGetFolderPathWithDefaultOption()
        {
            var expected = System.Environment.GetFolderPath( System.Environment.SpecialFolder.UserProfile,
                System.Environment.SpecialFolderOption.None );
            var actual = new SystemEnvironment().GetFolderPath( System.Environment.SpecialFolder.UserProfile,
                System.Environment.SpecialFolderOption.None );

            actual.Should().Be( expected );
        }

        [Fact]
        public void ItCanGetFolderPathWithNoVerifyOption()
        {
            var expected = System.Environment.GetFolderPath( System.Environment.SpecialFolder.ApplicationData,
                System.Environment.SpecialFolderOption.DoNotVerify );
            var actual = new SystemEnvironment().GetFolderPath( System.Environment.SpecialFolder.ApplicationData,
                System.Environment.SpecialFolderOption.DoNotVerify );

            actual.Should().Be( expected );
        }

        [Fact]
        public void ItCanGetFolderPathWithCreateOption()
        {
            var expected = System.Environment.GetFolderPath( System.Environment.SpecialFolder.UserProfile,
                System.Environment.SpecialFolderOption.Create );
            var actual = new SystemEnvironment().GetFolderPath( System.Environment.SpecialFolder.UserProfile,
                System.Environment.SpecialFolderOption.Create );

            actual.Should().Be( expected );
        }

        [Fact]
        public void ItCanMockGetFolderPath()
        {
            const string mockedUserProfile = "/home/Environment.Abstractions";
            var mockEnvironment = Mock.Of<ISystemEnvironment>( e =>
                e.GetFolderPath( System.Environment.SpecialFolder.UserProfile,
                    System.Environment.SpecialFolderOption.Create ) == mockedUserProfile );

            var actual = mockEnvironment.GetFolderPath( System.Environment.SpecialFolder.UserProfile,
                System.Environment.SpecialFolderOption.Create );

            actual.Should().Be( mockedUserProfile );
        }
    }
}
