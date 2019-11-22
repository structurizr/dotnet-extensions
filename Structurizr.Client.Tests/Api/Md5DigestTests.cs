using Xunit;

namespace Structurizr.Api.Tests
{
    
    public class Md5DigestTests
    {

        private Md5Digest md5 = new Md5Digest();

        [Fact]
        public void Test_Generate_TreatsNullAsEmptyContent()
        {
            Assert.Equal(md5.Generate(""), md5.Generate(null));
        }

        [Fact]
        public void Test_Generate_WorksForUTF8CharacterEncoding()
        {
            Assert.Equal("0a35e149dbbb2d10d744bf675c7744b1", md5.Generate("è"));
        }

        [Fact]
        public void Test_Generate()
        {
            Assert.Equal("ed076287532e86365e841e92bfc50d8c", md5.Generate("Hello World!"));
            Assert.Equal("d41d8cd98f00b204e9800998ecf8427e", md5.Generate(""));
        }
    }
}
