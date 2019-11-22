using System;
using Structurizr.Core.Util;
using Xunit;

namespace Structurizr.Core.Tests
{
    
    public class DeploymentNodeTests : AbstractTestBase
    {
    
    [Fact]
    public void test_getCanonicalName_WhenTheDeploymentNodeHasNoParent()
    {
        DeploymentNode deploymentNode = new DeploymentNode();
        deploymentNode.Name = "Ubuntu Server";

        Assert.Equal("/Deployment/Default/Ubuntu Server", deploymentNode.CanonicalName);
    }

    [Fact]
    public void test_getCanonicalName_WhenTheDeploymentNodeHasAParent()
    {
        DeploymentNode parent = new DeploymentNode();
        parent.Name = "Ubuntu Server";

        DeploymentNode child = new DeploymentNode();
        child.Name = "Apache Tomcat";
        child.Parent = parent;

        Assert.Equal("/Deployment/Default/Ubuntu Server/Apache Tomcat", child.CanonicalName);
    }

    [Fact]
    public void test_getParent_ReturnsTheParentDeploymentNode()
    {
        DeploymentNode parent = new DeploymentNode();
        Assert.Null(parent.Parent);

        DeploymentNode child = new DeploymentNode();
        child.Parent = parent;
        Assert.Same(parent, child.Parent);
    }

    [Fact]
    public void test_getRequiredTags()
    {
        DeploymentNode deploymentNode = new DeploymentNode();
        Assert.Equal(0, deploymentNode.GetRequiredTags().Count);
    }

    [Fact]
    public void test_getTags()
    {
        DeploymentNode deploymentNode = new DeploymentNode();
        Assert.Equal("", deploymentNode.Tags);
    }

    [Fact]
    public void test_add_ThrowsAnException_WhenAContainerIsNotSpecified()
    {
        try {
            DeploymentNode deploymentNode = new DeploymentNode();
            deploymentNode.Add(null);
            throw new TestFailedException();
        } catch (ArgumentException ae) {
            Assert.Equal("A container must be specified.", ae.Message);
        }
    }

    [Fact]
    public void test_add_AddsAContainerInstance_WhenAContainerIsSpecified()
    {
        SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "");
        Container container = softwareSystem.AddContainer("Container", "", "");
        DeploymentNode deploymentNode = Model.AddDeploymentNode("Deployment Node", "", "");
        ContainerInstance containerInstance = deploymentNode.Add(container);

        Assert.NotNull(containerInstance);
        Assert.Same(container, containerInstance.Container);
        Assert.True(deploymentNode.ContainerInstances.Contains(containerInstance));
    }

    [Fact]
    public void test_addDeploymentNode_ThrowsAnException_WhenANameIsNotSpecified()
    {
        try {
            DeploymentNode parent = Model.AddDeploymentNode("Parent", "", "");
            parent.AddDeploymentNode(null, "", "");
            throw new TestFailedException();
        } catch (ArgumentException ae) {
            Assert.Equal("A name must be specified.", ae.Message);
        }
    }

    [Fact]
    public void test_addDeploymentNode_AddsAChildDeploymentNode_WhenANameIsSpecified()
    {
        DeploymentNode parent = Model.AddDeploymentNode("Parent", "", "");

        DeploymentNode child = parent.AddDeploymentNode("Child 1", "Description", "Technology");
        Assert.NotNull(child);
        Assert.Equal("Child 1", child.Name);
        Assert.Equal("Description", child.Description);
        Assert.Equal("Technology", child.Technology);
        Assert.Equal(1, child.Instances);
        Assert.Equal(0, child.Properties.Count);
        Assert.True(parent.Children.Contains(child));

        child = parent.AddDeploymentNode("Child 2", "Description", "Technology", 4);
        Assert.NotNull(child);
        Assert.Equal("Child 2", child.Name);
        Assert.Equal("Description", child.Description);
        Assert.Equal("Technology", child.Technology);
        Assert.Equal(4, child.Instances);
        Assert.Equal(0, child.Properties.Count);
        Assert.True(parent.Children.Contains(child));

        child = parent.AddDeploymentNode("Child 3", "Description", "Technology", 4, DictionaryUtils.Create("name=value"));
        Assert.NotNull(child);
        Assert.Equal("Child 3", child.Name);
        Assert.Equal("Description", child.Description);
        Assert.Equal("Technology", child.Technology);
        Assert.Equal(4, child.Instances);
        Assert.Equal(1, child.Properties.Count);
        Assert.Equal("value", child.Properties["name"]);
        Assert.True(parent.Children.Contains(child));
    }

    [Fact]
    public void test_uses_ThrowsAnException_WhenANullDestinationIsSpecified() {
        try {
            DeploymentNode deploymentNode = Model.AddDeploymentNode("Deployment Node", "", "");
            deploymentNode.Uses(null, "", "");
            throw new TestFailedException();
        } catch (ArgumentException ae) {
            Assert.Equal("The destination must be specified.", ae.Message);
        }
    }

    [Fact]
    public void test_uses_AddsARelationship() {
        DeploymentNode primaryNode = Model.AddDeploymentNode("MySQL - Primary", "", "");
        DeploymentNode secondaryNode = Model.AddDeploymentNode("MySQL - Secondary", "", "");
        Relationship relationship = primaryNode.Uses(secondaryNode, "Replicates data to", "Some technology");

        Assert.NotNull(relationship);
        Assert.Same(primaryNode, relationship.Source);
        Assert.Same(secondaryNode, relationship.Destination);
        Assert.Equal("Replicates data to", relationship.Description);
        Assert.Equal("Some technology", relationship.Technology);
    }

    [Fact]
    public void test_getDeploymentNodeWithName_ThrowsAnException_WhenANameIsNotSpecified() {
        try {
            DeploymentNode deploymentNode = new DeploymentNode();
            deploymentNode.GetDeploymentNodeWithName(null);
            throw new TestFailedException();
        } catch (ArgumentException ae) {
            Assert.Equal("A name must be specified.", ae.Message);
        }
    }

    [Fact]
    public void test_getDeploymentNodeWithName_ReturnsNull_WhenThereIsNoDeploymentWithTheSpecifiedName() {
        DeploymentNode deploymentNode = new DeploymentNode();
        Assert.Null(deploymentNode.GetDeploymentNodeWithName("foo"));
    }

    [Fact]
    public void test_getDeploymentNodeWithName_ReturnsTheNamedDeploymentNode_WhenThereIsADeploymentWithTheSpecifiedName() {
        DeploymentNode parent = Model.AddDeploymentNode("parent", "", "");
        DeploymentNode child = parent.AddDeploymentNode("child", "", "");
        Assert.Same(child, parent.GetDeploymentNodeWithName("child"));
    }
        
    }
    
}