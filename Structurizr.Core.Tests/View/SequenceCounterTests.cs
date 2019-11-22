using Xunit;

namespace Structurizr.Core.Tests
{
    
    public class SequenceCounterTests
    {

        [Fact]
        public void Test_increment_IncrementsTheCounter()
        {
            SequenceCounter counter = new SequenceCounter();
            Assert.Equal("0", counter.AsString());

            counter.Increment();
            Assert.Equal("1", counter.AsString());

            counter.Increment();
            Assert.Equal("2", counter.AsString());
        }

    }
}
