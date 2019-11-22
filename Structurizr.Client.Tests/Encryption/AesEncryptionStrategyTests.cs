using Xunit;
using Structurizr.Encryption;
using System.Security.Cryptography;
using Structurizr.Api.Tests;

namespace Structurizr.Api.Encryption.Tests
{
    
    public class AesEncryptionStrategyTests
    {

        private AesEncryptionStrategy strategy;

        [Fact]
        public void Test_Encrypt_EncryptsPlaintext()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "06DC30A48ADEEE72D98E33C2CEAEAD3E", "ED124530AF64A5CAD8EF463CF5628434", "password");

            string ciphertext = strategy.Encrypt("Hello world");
            Assert.Equal("A/DzjV17WVS6ZAKsLOaC/Q==", ciphertext);
        }

        [Fact]
        public void Test_Decrypt_DecryptsTheCiphertext_WhenTheSameStrategyInstanceIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");

            string ciphertext = strategy.Encrypt("Hello world");
            Assert.Equal("Hello world", strategy.Decrypt(ciphertext));
        }

        [Fact]
        public void Test_Decrypt_DecryptsTheCiphertext_WhenTheSameConfigurationIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");

            string ciphertext = strategy.Encrypt("Hello world");

            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, strategy.Salt, strategy.Iv, "password");
            Assert.Equal("Hello world", strategy.Decrypt(ciphertext));
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectKeySizeIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(256, strategy.IterationCount, strategy.Salt, strategy.Iv, "password");

            try
            {
                strategy.Decrypt(ciphertext);
                throw new TestFailedException();
            }
            catch (CryptographicException ce)
            {
                // this is expected
            }
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectIterationCountIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, 2000, strategy.Salt, strategy.Iv, "password");

            try
            {
                strategy.Decrypt(ciphertext);
                throw new TestFailedException();
            }
            catch (CryptographicException ce)
            {
                // this is expected
            }
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectSaltIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, "133D30C2A658B3081279A97FD3B1F7CDE10C4FB61D39EEA8", strategy.Iv, "password");

            try
            {
                strategy.Decrypt(ciphertext);
                throw new TestFailedException();
            }
            catch (CryptographicException ce)
            {
                // this is expected
            }
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectIvIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, strategy.Salt, "1DED89E4FB15F61DC6433E3BADA4A891", "password");

            try
            {
                strategy.Decrypt(ciphertext);
                throw new TestFailedException();
            }
            catch (CryptographicException ce)
            {
                // this is expected
            }
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectPassphraseIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, strategy.Salt, strategy.Iv, "The Wrong Password");

            try
            {
                strategy.Decrypt(ciphertext);
                throw new TestFailedException();
            }
            catch (CryptographicException ce)
            {
                // this is expected
            }
        }

    }

}
