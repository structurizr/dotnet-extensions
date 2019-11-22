using System;
using Xunit;

namespace Structurizr.Core.Tests.View
{

    public class StyleTests
    {

        private Styles _styles = new Styles();

        [Fact]
        public void Test_AddElementStyle_ThrowsAnException_WhenAStyleWithTheSameTagExistsAlready()
        {
            try
            {
                _styles.Add(new ElementStyle(Tags.SoftwareSystem) { Color = "#ff0000" });
                _styles.Add(new ElementStyle(Tags.SoftwareSystem) { Color = "#ff0000" });

                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("An element style for the tag \"Software System\" already exists.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddRelationshipStyle_ThrowsAnException_WhenAStyleWithTheSameTagExistsAlready()
        {
            try
            {
                _styles.Add(new RelationshipStyle(Tags.Relationship) { Color = "#ff0000" });
                _styles.Add(new RelationshipStyle(Tags.Relationship) { Color = "#ff0000" });

                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A relationship style for the tag \"Relationship\" already exists.", ae.Message);
            }
        }

        [Fact]
        public void Test_ClearElementStyles_RemovesAllElementStyles()
        {
            _styles.Add(new ElementStyle(Tags.SoftwareSystem) { Color = "#ff0000" });
            Assert.Equal(1, _styles.Elements.Count);

            _styles.ClearElementStyles();
            Assert.Equal(0, _styles.Elements.Count);
        }

        [Fact]
        public void Test_ClearRelationshipStyles_RemovesAllRelationshipStyles()
        {
            _styles.Add(new RelationshipStyle(Tags.Relationship) { Color = "#ff0000" });
            Assert.Equal(1, _styles.Relationships.Count);

            _styles.ClearRelationshipStyles();
            Assert.Equal(0, _styles.Relationships.Count);
        }

    }

}