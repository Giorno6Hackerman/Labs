using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace AesEncryption
{
    public class AesEncryptor
    {
        static private Aes crypty = Aes.Create();
        static private ICryptoTransform encryptor = crypty.CreateEncryptor();
        static private ICryptoTransform decryptor = crypty.CreateDecryptor();

        public AesEncryptor()
        {
            
        }



        public void Encrypt(Stream data, string fileName)
        {
            //FileStream file = File.Create(fileName);
            byte[] encrypted = new byte[256];

            using (FileStream file = File.Create(fileName))
            {
                using (BinaryReader reader = new BinaryReader(data))
                {
                    int count = reader.Read(encrypted, 0, encrypted.Length);
                    while (count > 0)
                    {
                        byte[] result = encryptor.TransformFinalBlock(encrypted, 0, count);
                        file.Write(result, 0, result.Length);
                        count = reader.Read(encrypted, 0, encrypted.Length);
                    }
                }
            }
        }


        public void Decrypt(Stream data, string fileName)
        {
            //StreamReader reader = new StreamReader(fileName);
            byte[] decrypted = new byte[256];

            using (FileStream file = File.OpenRead(fileName))
            {
                BinaryWriter writer = new BinaryWriter(data);
                int count = file.Read(decrypted, 0, decrypted.Length);
                while (count > 0)
                {
                    byte[] result = decryptor.TransformFinalBlock(decrypted, 0, count);
                    writer.Write(result, 0, result.Length);
                    count = file.Read(decrypted, 0, decrypted.Length);
                }
                writer.Seek(0, SeekOrigin.Begin);

            }
        }
    }
}
