using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace RSAEncryption
{
    public class RSAEncryptor
    {
        static private RSACryptoServiceProvider crypty = new RSACryptoServiceProvider(2048, new CspParameters(1, "Satan", "hehe progs go brrrrr"));
        static private CspParameters parameters = new CspParameters();

        public RSAEncryptor()
        {

        }


        public void Encrypt(Stream data, string fileName)
        {
            byte[] encrypted = new byte[256];

            using (FileStream file = File.Create(fileName))
            {
                using (BinaryReader reader = new BinaryReader(data))
                {
                    int count = reader.Read(encrypted, 0, encrypted.Length);
                    while (count > 0)
                    {
                        byte[] result = crypty.Encrypt(encrypted, false);
                        file.Write(result, 0, result.Length);
                        count = reader.Read(encrypted, 0, encrypted.Length);
                    }
                }
            }
                        
        }


        public void Decrypt(Stream data, string fileName)
        {
            byte[] decrypted = new byte[256];

            using (FileStream file = File.OpenRead(fileName))
            {
                BinaryWriter writer = new BinaryWriter(data);
                int count = file.Read(decrypted, 0, decrypted.Length);
                while (count > 0)
                {
                    byte[] result = crypty.Decrypt(decrypted, false);
                    writer.Write(result, 0, result.Length);
                    count = file.Read(decrypted, 0, decrypted.Length);
                }
                writer.Seek(0, SeekOrigin.Begin);

            }

        }
    }
}
