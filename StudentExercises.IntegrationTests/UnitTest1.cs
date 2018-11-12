using System;
using Xunit;

namespace StudentExercises.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void TrueIsTrue()
        {
            // Arrange
            int one = 1;
            int two = 2;
            int three = 3;

            // Act
            int val = one + two;

            // Assert
            Assert.Equal(three, val);
        }

        [Fact]
        public void FalseIsNotTrue()
        {
            Assert.False(!true != false);
        }
    }
}
