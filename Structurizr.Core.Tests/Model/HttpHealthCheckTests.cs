using System;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class HttpHealthCheckTests
    {

        private HttpHealthCheck _healthCheck;

        [Fact]
        public void test_defaultConstructorExists()
        {
            // the default constructor is used when deserializing from JSON
            _healthCheck = new HttpHealthCheck();
        }

        [Fact]
        public void test_construction()
        {
            _healthCheck = new HttpHealthCheck("Name", "http://localhost", 120, 1000);
            Assert.Equal("Name", _healthCheck.Name);
            Assert.Equal("http://localhost", _healthCheck.Url);
            Assert.Equal(120, _healthCheck.Interval);
            Assert.Equal(1000, _healthCheck.Timeout);
        }

        [Fact]
        public void test_AddHeader()
        {
            _healthCheck = new HttpHealthCheck();
            _healthCheck.AddHeader("Name", "Value");
            Assert.Equal("Value", _healthCheck.Headers["Name"]);
        }

        [Fact]
        public void test_AddHeader_ThrowsAnException_WhenTheHeaderNameIsNull()
        {
            _healthCheck = new HttpHealthCheck();
            try
            {
                _healthCheck.AddHeader(null, "value");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The header name must not be null or empty.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHeader_ThrowsAnException_WhenTheHeaderNameIsEmpty()
        {
            _healthCheck = new HttpHealthCheck();
            try
            {
                _healthCheck.AddHeader(" ", "value");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The header name must not be null or empty.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHeader_ThrowsAnException_WhenTheHeaderValueIsNull()
        {
            _healthCheck = new HttpHealthCheck();
            try
            {
                _healthCheck.AddHeader("Name", null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The header value must not be null.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHeader_DoesNotThrowAnException_WhenTheHeaderValueIsEmpty()
        {
            _healthCheck = new HttpHealthCheck();
            _healthCheck.AddHeader("Name", "");
            Assert.Equal("", _healthCheck.Headers["Name"]);
        }


    }
}
