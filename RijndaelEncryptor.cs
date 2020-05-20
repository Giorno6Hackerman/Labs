using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SymEncryption
{
    public class RijndaelEncryptor
    {
        static private RijndaelManaged crypty = new RijndaelManaged();
        static private ICryptoTransform encryptor = crypty.CreateEncryptor();
        static private ICryptoTransform decryptor = crypty.CreateDecryptor();

        public RijndaelEncryptor()
        {
            crypty.BlockSize = 256;
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

            FileStream str = File.Create("a2.dat");


            using (FileStream file = File.OpenRead(fileName))
            {
                using (BinaryWriter writer = new BinaryWriter(data))
                {
                    int count = file.Read(decrypted, 0, decrypted.Length);
                    while (count > 0)
                    {
                        byte[] result = decryptor.TransformFinalBlock(decrypted, 0, count);
                        writer.Write(result, 0, result.Length);
                        str.Write(result, 0, result.Length);
                        count = file.Read(decrypted, 0, decrypted.Length);
                    }
                    writer.Seek(0, SeekOrigin.Begin);
                }
            }

            str.Close();
        }
    }
}
