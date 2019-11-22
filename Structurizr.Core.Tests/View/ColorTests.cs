
using Xunit;

namespace Structurizr.Core.Tests
{

    
    public class ColorTests
    {

        [Fact]
        public void Test_IsHexColorCode_ReturnsFalse_WhenPassedNull()
        {
            Assert.False(Color.IsHexColorCode(null));
        }

        [Fact]
        public void Test_IsHexColorCode_ReturnsFalse_WhenPassedAnEmptyString()
        {
            Assert.False(Color.IsHexColorCode(""));
        }

        [Fact]
        public void Test_IsHexColorCode_ReturnsFalse_WhenPassedAnInvalidString()
        {
            Assert.False(Color.IsHexColorCode("ffffff"));
            Assert.False(Color.IsHexColorCode("#fffff"));
            Assert.False(Color.IsHexColorCode("#gggggg"));
        }

        [Fact]
        public void Test_IsHexColorCode_ReturnsTrue_WhenPassedAnValidString()
        {
            Assert.True(Color.IsHexColorCode("#abcdef"));
            Assert.True(Color.IsHexColorCode("#ABCDEF"));
            Assert.True(Color.IsHexColorCode("#123456"));
        }

    }

}
