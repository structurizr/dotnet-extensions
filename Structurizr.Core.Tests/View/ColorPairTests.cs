using System;
using Xunit;

namespace Structurizr.Core.Tests.View
{

    
    public class ColorPairTests
    {

        [Fact]
        public void test_construction()
        {
            ColorPair colorPair = new ColorPair("#ffffff", "#000000");
            Assert.Equal("#ffffff", colorPair.Background);
            Assert.Equal("#000000", colorPair.Foreground);
        }

        [Fact]
        public void test_setBackground_WithAValidHtmlColorCode()
        {
            ColorPair colorPair = new ColorPair();
            colorPair.Background = "#ffffff";
            Assert.Equal("#ffffff", colorPair.Background);
        }

        [Fact]
        public void test_setBackground_ThrowsAnException_WhenANullHtmlColorCodeIsSpecified()
        {
            try
            {
                ColorPair colorPair = new ColorPair();
                colorPair.Background = null;
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("'' is not a valid hex color code.", iae.Message);
            }
        }

        [Fact]
        public void test_setBackground_ThrowsAnException_WhenAnEmptyHtmlColorCodeIsSpecified()
        {
            try
            {
                ColorPair colorPair = new ColorPair();
                colorPair.Background = "";
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("'' is not a valid hex color code.", iae.Message);
            }
        }

        [Fact]
        public void test_setBackground_ThrowsAnException_WhenAnInvalidHtmlColorCodeIsSpecified()
        {
            try
            {
                ColorPair colorPair = new ColorPair();
                colorPair.Background = "ffffff";
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("'ffffff' is not a valid hex color code.", iae.Message);
            }
        }

        [Fact]
        public void test_setForeground_WithAValidHtmlColorCode()
        {
            ColorPair colorPair = new ColorPair();
            colorPair.Foreground = "#000000";
            Assert.Equal("#000000", colorPair.Foreground);
        }

        [Fact]
        public void test_setForeground_ThrowsAnException_WhenANullHtmlColorCodeIsSpecified()
        {
            try
            {
                ColorPair colorPair = new ColorPair();
                colorPair.Foreground = null;
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("'' is not a valid hex color code.", iae.Message);
            }
        }

        [Fact]
        public void test_setForeground_ThrowsAnException_WhenAnEmptyHtmlColorCodeIsSpecified()
        {
            try
            {
                ColorPair colorPair = new ColorPair();
                colorPair.Foreground = "";
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("'' is not a valid hex color code.", iae.Message);
            }
        }

        [Fact]
        public void test_setForeground_ThrowsAnException_WhenAnInvalidHtmlColorCodeIsSpecified()
        {
            try
            {
                ColorPair colorPair = new ColorPair();
                colorPair.Foreground = "000000";
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("'000000' is not a valid hex color code.", iae.Message);
            }
        }

    }

}
