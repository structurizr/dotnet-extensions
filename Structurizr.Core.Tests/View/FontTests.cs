using System;
using Xunit;

namespace Structurizr.Core.Tests.View
{

    
    public class FontTests
    {

        private Font _font;

        [Fact]
        public void construction_WithANameOnly()
        {
            this._font = new Font("Times New Roman");
            Assert.Equal("Times New Roman", _font.Name);
        }

        [Fact]
        public void construction_WithANameAndUrl()
        {
            this._font = new Font("Open Sans", "https://fonts.googleapis.com/css?family=Open+Sans:400,700");
            Assert.Equal("Open Sans", _font.Name);
            Assert.Equal("https://fonts.googleapis.com/css?family=Open+Sans:400,700", _font.Url);
        }

        [Fact]
        public void test_setUrl_WithAUrl()
        {
            _font = new Font();
            _font.Url = "https://fonts.googleapis.com/css?family=Open+Sans:400,700";
            Assert.Equal("https://fonts.googleapis.com/css?family=Open+Sans:400,700", _font.Url);
        }

        [Fact]
        public void test_setUrl_ThrowsAnArgumentException_WhenAnInvalidUrlIsSpecified()
        {
            _font = new Font();
            Assert.Throws<ArgumentException>(() =>
                _font.Url = "www.google.com"
            );
        }

        [Fact]
        public void test_setUrl_DoesNothing_WhenANullUrlIsSpecified()
        {
            _font = new Font();
            _font.Url = null;
            Assert.Null(_font.Url);
        }

        [Fact]
        public void test_setUrl_DoesNothing_WhenAnEmptyUrlIsSpecified()
        {
            _font = new Font();
            _font.Url = " ";
            Assert.Null(_font.Url);
        }

    }

}