using System;
using Xunit;

namespace Structurizr.Core.Tests.View
{

    public class RelationshipStyleTests
    {

        [Fact]
        public void Test_Position()
        {
            RelationshipStyle style = new RelationshipStyle();
            Assert.Null(style.Position);

            style.Position = -1;
            Assert.Equal(0, style.Position);

            style.Position = 0;
            Assert.Equal(0, style.Position);

            style.Position = 50;
            Assert.Equal(50, style.Position);

            style.Position = 100;
            Assert.Equal(100, style.Position);

            style.Position = 101;
            Assert.Equal(100, style.Position);
        }

        [Fact]
        public void Test_Opacity()
        {
            RelationshipStyle style = new RelationshipStyle();
            Assert.Null(style.Opacity);

            style.Opacity = -1;
            Assert.Equal(0, style.Opacity);

            style.Opacity = 0;
            Assert.Equal(0, style.Opacity);

            style.Opacity = 50;
            Assert.Equal(50, style.Opacity);

            style.Opacity = 100;
            Assert.Equal(100, style.Opacity);

            style.Opacity = 101;
            Assert.Equal(100, style.Opacity);
        }

        [Fact]
        public void Test_Color_SetsTheColorProperty_WhenAValidHexColorCodeIsSpecified()
        {
            RelationshipStyle style = new RelationshipStyle();
            style.Color = "#ffffff";
            Assert.Equal("#ffffff", style.Color);

            style.Color = "#FFFFFF";
            Assert.Equal("#ffffff", style.Color);

            style.Color = "#123456";
            Assert.Equal("#123456", style.Color);
        }

        [Fact]
        public void Test_Color_ThrowsAnException_WhenAnInvalidHexColorCodeIsSpecified()
        {
            try
            {
                RelationshipStyle style = new RelationshipStyle();
                style.Color = "white";
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("'white' is not a valid hex color code.", ae.Message);
            }
        }

    }
}
