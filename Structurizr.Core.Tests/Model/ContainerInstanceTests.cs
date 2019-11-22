using System;
using Xunit;

namespace Structurizr.Core.Tests
{
    
    public class ContainerInstanceTests : AbstractTestBase
    {

        private SoftwareSystem _softwareSystem;
        private Container _database;
        private DeploymentNode _deploymentNode;

        public ContainerInstanceTests()
        {
            _softwareSystem = Model.AddSoftwareSystem(Location.External, "System", "Description");
            _database = _softwareSystem.AddContainer("Database Schema", "Stores data", "MySQL");
            _deploymentNode = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
        }
        
        [Fact]
        public void test_construction() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.Same(_database, containerInstance.Container);
            Assert.Equal(_database.Id, containerInstance.ContainerId);
            Assert.Equal(1, containerInstance.InstanceId);
        }

        [Fact]
        public void test_getContainerId() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.Equal(_database.Id, containerInstance.ContainerId);
            containerInstance.Container = null;
            containerInstance.ContainerId = "1234";
            Assert.Equal("1234", containerInstance.ContainerId);
        }

        [Fact]
        public void test_getName() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.Null(containerInstance.Name);
    
            containerInstance.Name = "foo";
            Assert.Null(containerInstance.Name);
        }
    
        [Fact]
        public void test_getCanonicalName() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.Equal("/System/Database Schema[1]", containerInstance.CanonicalName);
        }
    
        [Fact]
        public void test_getParent_ReturnsTheParentSoftwareSystem() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.Equal(_softwareSystem, containerInstance.Parent);
        }
    
        [Fact]
        public void test_getRequiredTags() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.Equal(0, containerInstance.GetRequiredTags().Count);
        }
    
        [Fact]
        public void test_getTags() {
            _database.AddTags("Database");
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
            containerInstance.AddTags("Primary Instance");
    
            Assert.Equal("Container Instance,Primary Instance", containerInstance.Tags);
        }
    
        [Fact]
        public void test_removeTags_DoesNotRemoveRequiredTags() {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
    
            Assert.True(containerInstance.Tags.Contains(Tags.ContainerInstance));
    
            containerInstance.RemoveTag(Tags.ContainerInstance);
    
            Assert.True(containerInstance.Tags.Contains(Tags.ContainerInstance));
        }
    
        [Fact]
        public void test_AddHealthCheck()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);
            Assert.Equal(0, containerInstance.HealthChecks.Count);

            HttpHealthCheck healthCheck = containerInstance.AddHealthCheck("Test web application is working", "http://localhost:8080");
            Assert.Equal("Test web application is working", healthCheck.Name);
            Assert.Equal("http://localhost:8080", healthCheck.Url);
            Assert.Equal(60, healthCheck.Interval);
            Assert.Equal(0, healthCheck.Timeout);
            Assert.Equal(1, containerInstance.HealthChecks.Count);
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheNameIsNull()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck(null, "http://localhost");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The name must not be null or empty.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheNameIsEmpty()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck(" ", "http://localhost");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The name must not be null or empty.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheUrlIsNull()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck("Name", null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The URL must not be null or empty.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheUrlIsEmpty()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck("Name", " ");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The URL must not be null or empty.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheUrlIsInvalid()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck("Name", "localhost");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("localhost is not a valid URL.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheIntervalIsLessThanZero()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck("Name", "https://localhost", -1, 0);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The polling interval must be zero or a positive integer.", ae.Message);
            }
        }

        [Fact]
        public void test_AddHealthCheck_ThrowsAnException_WhenTheTimeoutIsLessThanZero()
        {
            ContainerInstance containerInstance = Model.AddContainerInstance(_deploymentNode, _database);

            try
            {
                containerInstance.AddHealthCheck("Name", "https://localhost", 60, -1);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The timeout must be zero or a positive integer.", ae.Message);
            }
        }

    }

}