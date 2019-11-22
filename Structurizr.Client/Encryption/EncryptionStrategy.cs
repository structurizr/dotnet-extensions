using System.Runtime.Serialization;

namespace Structurizr.Encryption
{

    [DataContract]
    public abstract class EncryptionStrategy
    {

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public abstract string Type { get; }

        public string Passphrase { get; set; }

        [DataMember(Name = "location", EmitDefaultValue = false)]
        public string Location
        {
            get
            {
                return "Client";
            }
        }

        public EncryptionStrategy() { }

        public EncryptionStrategy(string passphrase)
        {
            this.Passphrase = passphrase;
        }

        public abstract string Encrypt(string plaintext);
        public abstract string Decrypt(string ciphertext);

    }
}
