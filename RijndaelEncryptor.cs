using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SymEncryption
{
    public class RijndaelEncryptor
    {
        private Rijndael crypty;

        public RijndaelEncryptor()
        {
            crypty = Rijndael.Create();
        }


        public void Encrypt(Stream data)
        {
            StreamWriter writer = new StreamWriter(data);
            byte[] encrypted;
            ICryptoTransform encryptor = crypty.CreateEncryptor(crypty.Key, crypty.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(data);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            data.Write(encrypted, 0, encrypted.Length);
            writer.Close();
        }


        public void Decrypt(Stream data)
        {
            StreamReader reader = new StreamReader(data);
            byte[] decrypted;
            ICryptoTransform decryptor = crypty.CreateEncryptor(crypty.Key, crypty.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Encoding.UTF8.GetBytes(reader.ReadToEnd())))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        decrypted = Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
                    }
                }
            }

            data.Write(decrypted, 0, decrypted.Length);
            reader.Close();
        }
    }
}
