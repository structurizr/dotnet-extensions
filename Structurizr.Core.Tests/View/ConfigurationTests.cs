using Xunit;

namespace Structurizr.Core.Tests
{

    
    public class ViewConfigurationTests : AbstractTestBase
    {

        [Fact]
        public void test_defaultView_DoesNothing_WhenPassedNull()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.SetDefaultView(null);
            Assert.Null(configuration.DefaultView);
        }

        [Fact]
        public void test_defaultView()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("key", "Description");
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.SetDefaultView(view);
            Assert.Equal("key", configuration.DefaultView);
        }

        [Fact]
        public void test_copyConfigurationFrom()
        {
            ViewConfiguration source = new ViewConfiguration();
            source.LastSavedView = "someKey";

            ViewConfiguration destination = new ViewConfiguration();
            destination.CopyConfigurationFrom(source);
            Assert.Equal("someKey", destination.LastSavedView);
        }

    }

}
