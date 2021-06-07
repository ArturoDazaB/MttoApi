using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MttoApi
{
    public class Metodos
    {
        private string SecretKey { get { return "12345678"; } }
        private string PublicKey { get { return "98765432"; } }

        public string EncryptString(string text2Encrypt)
        {
            string ToReturn = string.Empty;
            byte[] secretkeyByte = { };
            byte[] publickeybyte = { };
            byte[] inputbyteArray = { };
            MemoryStream ms = null;
            CryptoStream cs = null;

            try
            {
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(SecretKey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(PublicKey);
                inputbyteArray = System.Text.Encoding.UTF8.GetBytes(text2Encrypt);

                using (DESCryptoServiceProvider enc = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, enc.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                      ex is CryptographicException ||
                                      ex is CryptographicUnexpectedOperationException)
            {
                ToReturn = "\n\nHa ocurrido un error al intentar encriptar el texto: " + ex.Message + "\n\n";
            }

            return ToReturn;
        }

        public string DecryptString(string text2Decrypt)
        {
            string ToReturn = string.Empty;
            byte[] secretkeyByte = { };
            byte[] publickeybyte = { };
            byte[] inputbyteArray = { };
            MemoryStream ms = null;
            CryptoStream cs = null;

            try
            {
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(SecretKey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(PublicKey);
                inputbyteArray = new byte[text2Decrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(text2Decrypt.Replace(" ", "+"));

                using (DESCryptoServiceProvider dec = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, dec.CreateDecryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is ArgumentException ||
                                       ex is FormatException ||
                                       ex is CryptographicException ||
                                       ex is CryptographicUnexpectedOperationException)
            {
                ToReturn = "\n\nHa ocurrido un error al intentar encriptar el texto: " + ex.Message + "\n\n";
            }

            return ToReturn;
        }
    }
}