using System;
using Xunit;

namespace Structurizr.Core.Tests
{

    
    public class CodeElementTests
    {

        private readonly CodeElement _codeElement = new CodeElement("Wibble.Wobble, Foo.Bar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=xyz");

        [Fact]
        public void Test_Construction_WhenAFullyQualifiedTypeIsSpecified()
        {
            Assert.Equal("Wobble", _codeElement.Name);
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenANullUrlIsSpecified()
        {
            _codeElement.Url = null;
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAnEmptyUrlIsSpecified()
        {
            _codeElement.Url = "";
        }

        [Fact]
        public void Test_SetUrl_ThrowsAnException_WhenAnInvalidUrlIsSpecified()
        {
            try
            {
                _codeElement.Url = "www.somedomain.com";
                throw new TestFailedException();
            }
            catch (Exception e)
            {
                Assert.Equal("www.somedomain.com is not a valid URL.", e.Message);
            }
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAValidUrlIsSpecified()
        {
            _codeElement.Url = "http://www.somedomain.com";
            Assert.Equal("http://www.somedomain.com", _codeElement.Url);
        }

    }

}
