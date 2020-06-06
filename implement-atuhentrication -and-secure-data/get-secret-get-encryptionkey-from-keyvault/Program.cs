using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System;
using System.Text;

namespace KeyVault
{
    class Program
    {
        static void Main(string[] args)
        {
            GetSecretFromKeyVault();
            GetEncyptionKeysFromKeyVault();
        }

        private static void GetEncyptionKeysFromKeyVault()
        {
            string keyVaultUrl = "https://securedatakeyvault01.vault.azure.net/";
            var client = new KeyClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());

            // Getting the Encryption key from the key vault
            KeyVaultKey key = client.GetKey("newkey");

            var cryptoClient = new CryptographyClient(keyId: key.Id, credential: new DefaultAzureCredential());
            byte[] plaintext = Encoding.UTF8.GetBytes("This is sensitive data");

            // Encrypting data
            EncryptResult encryptResult = cryptoClient.Encrypt(EncryptionAlgorithm.RsaOaep256, plaintext);

            //Decrypting data
            DecryptResult decryptResult = cryptoClient.Decrypt(EncryptionAlgorithm.RsaOaep256, encryptResult.Ciphertext);
            Console.WriteLine("Plain text");

            string txt = Encoding.UTF8.GetString(decryptResult.Plaintext);
            Console.WriteLine(txt);
            Console.ReadLine();
        }

        private static void GetSecretFromKeyVault()
        {
            string keyVaultUrl = "https://securedatakeyvault01.vault.azure.net/";
            var client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());

            KeyVaultSecret secret = client.GetSecret("dbpassword");
            Console.WriteLine(secret.Value);
            Console.ReadKey();
        }
    }
}
