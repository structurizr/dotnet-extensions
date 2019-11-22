using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Structurizr.Encryption
{
    [DataContract]
    public class AesEncryptionStrategy : EncryptionStrategy
    {

        private const int InitializationVectorSizeInBytes = 16;

        public override string Type
        {
            get
            {
                return "aes";
            }
        }

        [DataMember(Name = "keySize", EmitDefaultValue = false)]
        public int KeySize { get; private set; }

        [DataMember(Name = "iterationCount", EmitDefaultValue = false)]
        public int IterationCount { get; private set; }

        [DataMember(Name = "salt", EmitDefaultValue = false)]
        public string Salt { get; private set; }

        [DataMember(Name = "iv", EmitDefaultValue = false)]
        public string Iv { get; private set; }

        public AesEncryptionStrategy() { }

        public AesEncryptionStrategy(string passphrase) : this(128, 1000, passphrase) { }

        public AesEncryptionStrategy(int keySize, int iterationCount, string passphrase) : base(passphrase)
        {
            KeySize = keySize;
            IterationCount = iterationCount;

            // create a random salt
            byte[] saltAsBytes = CreateRandomBytes(keySize / 8);
            Salt = BitConverter.ToString(saltAsBytes).Replace("-", "");

            byte[] ivAsBytes = CreateRandomBytes(InitializationVectorSizeInBytes);
            Iv = BitConverter.ToString(ivAsBytes).Replace("-", "");
        }
        
        private byte[] CreateRandomBytes(int bits)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[bits];
                random.GetBytes(bytes);

                return bytes;
            }
        }

        public AesEncryptionStrategy(int keySize, int iterationCount, string salt, string iv, string passphrase) : base(passphrase)
        {
            this.KeySize = keySize;
            this.IterationCount = iterationCount;
            this.Salt = salt;
            this.Iv = iv;
        }

        public override string Decrypt(string ciphertext)
        {
            string plaintext;
            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(
                        Encoding.UTF8.GetBytes(Passphrase),
                        hexStringToByteArray(Salt),
                        IterationCount);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = hexStringToByteArray(Iv);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] bytesToBeDecrypted = Convert.FromBase64String(ciphertext);
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    }

                    decryptedBytes = ms.ToArray();
                    plaintext = Encoding.UTF8.GetString(decryptedBytes);
                }
            }

            return plaintext;
        }

        public override string Encrypt(string plaintext)
        {
            string ciphertext = null;
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(
                        Encoding.UTF8.GetBytes(Passphrase),
                        hexStringToByteArray(Salt),
                        IterationCount);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = hexStringToByteArray(Iv);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plaintextAsBytes = Encoding.UTF8.GetBytes(plaintext);
                        cs.Write(plaintextAsBytes, 0, plaintextAsBytes.Length);
                    }
                    encryptedBytes = ms.ToArray();
                    ciphertext = Convert.ToBase64String(encryptedBytes);
                }
            }

            return ciphertext;
        }

        private byte[] hexStringToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteValue = hex.Substring(i * 2, 2);
                bytes[i] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return bytes;
        }

    }
}