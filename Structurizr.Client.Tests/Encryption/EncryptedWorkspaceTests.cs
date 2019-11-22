using System.IO;
using System.Linq;
using Structurizr.Config;
using Structurizr.Encryption;
using Structurizr.IO.Json;
using Xunit;

namespace Structurizr.Api.Encryption.Tests
{
    public class EncryptedWorkspaceTests
    {

        private EncryptedWorkspace _encryptedWorkspace;
        private EncryptionStrategy _encryptionStrategy;
        private Workspace _workspace;
        
        public EncryptedWorkspaceTests()
        {
            _workspace = new Workspace("Name", "Description");
            _workspace.Version = "1.2.3";
            _workspace.Revision = 123;
            _workspace.LastModifiedAgent = "Agent";
            _workspace.LastModifiedUser = "User";
            _workspace.Id = 1234;
            _workspace.Configuration.AddUser("user@domain.com", Role.ReadOnly);
            
            _encryptionStrategy = new MockEncryptionStrategy();
        }
        
        [Fact]
        public void Test_Construction()
        {
            _encryptedWorkspace = new EncryptedWorkspace(_workspace, _encryptionStrategy);

            Assert.Equal("Name", _encryptedWorkspace.Name);
            Assert.Equal("Description", _encryptedWorkspace.Description);
            Assert.Equal("1.2.3", _encryptedWorkspace.Version);
            Assert.Equal(123, _encryptedWorkspace.Revision);
            Assert.Equal("Agent", _encryptedWorkspace.LastModifiedAgent);
            Assert.Equal("User", _encryptedWorkspace.LastModifiedUser);
            Assert.Equal(1234, _encryptedWorkspace.Id);
            Assert.Equal("user@domain.com", _encryptedWorkspace.Configuration.Users.First().Username);
            Assert.Null(_workspace.Configuration);

            Assert.Same(_workspace, _encryptedWorkspace.Workspace);
            Assert.Same(_encryptionStrategy, _encryptedWorkspace.EncryptionStrategy);

            JsonWriter jsonWriter = new JsonWriter(false);
            StringWriter stringWriter = new StringWriter();
            jsonWriter.Write(_workspace, stringWriter);
    
            Assert.Equal(stringWriter.ToString(), _encryptedWorkspace.Plaintext);
            Assert.Equal(_encryptionStrategy.Encrypt(stringWriter.ToString()), _encryptedWorkspace.Ciphertext);
        }

        [Fact]
        public void Test_Workspace_ReturnsTheWorkspace_WhenACipherextIsSpecified()
        {
            JsonWriter jsonWriter = new JsonWriter(false);
            StringWriter stringWriter = new StringWriter();
            jsonWriter.Write(_workspace, stringWriter);
            string expected = stringWriter.ToString();
        
            _encryptedWorkspace = new EncryptedWorkspace();
            _encryptedWorkspace.EncryptionStrategy = _encryptionStrategy;
            _encryptedWorkspace.Ciphertext = _encryptionStrategy.Encrypt(expected);
        
            _workspace = _encryptedWorkspace.Workspace;
            Assert.Equal("Name", _workspace.Name);
            stringWriter = new StringWriter();
            jsonWriter.Write(_workspace, stringWriter);
            Assert.Equal(expected, stringWriter.ToString());
        }

    }
}