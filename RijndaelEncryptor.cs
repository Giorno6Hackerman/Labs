using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SymEncryption
{
    public class RijndaelEncryptor
    {
        static private Rijndael crypty = Rijndael.Create();
        static private ICryptoTransform encryptor;

        public RijndaelEncryptor()
        {
            encryptor = crypty.CreateEncryptor(key, iv);
        }

        private byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        private byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };


        public void Encrypt(Stream data, string fileName)
        {
            //FileStream file = File.Create(fileName);
            //byte[] encrypted;

            

            using (FileStream msEncrypt = File.Create(fileName))
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(data);
                    }
                }
            }

        }


        public Stream Decrypt(string fileName)
        {
            //StreamReader reader = new StreamReader(fileName);
            byte[] decrypted = new byte[1024];

            MemoryStream data = new MemoryStream();


            FileStream str = File.Create("a2.dat");
            using (FileStream msDecrypt = File.OpenRead(fileName))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, encryptor, CryptoStreamMode.Read))
                {
                    using (BinaryReader srDecrypt = new BinaryReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        int count = srDecrypt.Read(decrypted, 0, decrypted.Length);
                        do
                        {
                            data.Write(decrypted, 0, count);
                            str.Write(decrypted, 0, count);
                            count = srDecrypt.Read(decrypted, 0, decrypted.Length);
                        }
                        while (count > 0);
                    }
                }
            }

            
            data.Position = 0;
            //reader.Close();

            
            
            str.Close();

            return data;
        }
    }
}
