using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MttoApi
{
    public class Metodos
    {
        //SE DEFINEN LAS PROPIEDADES "SecretKey" y "PublicKey"
        private string SecretKey { get { return "12345678"; } }
        private string PublicKey { get { return "98765432"; } }
        public string EncryptString(string text2Encrypt)
        {
            //SE CREAN E INICIALIZAN LAS VARIABLES LOCALES USADAS
            string ToReturn = string.Empty;
            byte[] secretkeyByte = { };
            byte[] publickeybyte = { };
            byte[] inputbyteArray = { };
            MemoryStream ms = null;
            CryptoStream cs = null;

            //SE INICIA EL CICLO TRY...CATCH
            try
            {
                //TRANSFORMAMOS/CONVERTIMOS LAS CLAVES PUBLICAS Y PRIVADAS, ADEMAS
                //DEL TEXTO A ENCRIPTAR, A SU EQUIVALENTE EN VECTOR DE BYTES
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(SecretKey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(PublicKey);
                inputbyteArray = System.Text.Encoding.UTF8.GetBytes(text2Encrypt);

                //PROCESO DE ENCRIPTACION
                using (DESCryptoServiceProvider enc = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, enc.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
            }
            //SI OCURRE ALGUNA EXCEPCION EN ALGUN PROCESO O LLAMADO DE PROCESO EN EL SEGMENTO
            //TRY... EL SEGMENTO CATCH CAPTURARA ESTAS SITACIONES
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
            //SE CREAN E INICIALIZAN LAS VARIABLES LOCALES USADAS
            string ToReturn = string.Empty;
            byte[] secretkeyByte = { };
            byte[] publickeybyte = { };
            byte[] inputbyteArray = { };
            MemoryStream ms = null;
            CryptoStream cs = null;

            //SE INICIA EL CICLO TRY...CATCH
            try
            {
                //TRANSFORMAMOS/CONVERTIMOS LAS CLAVES PUBLICAS Y PRIVADAS, ADEMAS
                //DEL TEXTO A ENCRIPTAR, A SU EQUIVALENTE EN VECTOR DE BYTES
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(SecretKey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(PublicKey);
                inputbyteArray = new byte[text2Decrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(text2Decrypt.Replace(" ", "+"));

                //PROCESO DE DESENCRIPTACION
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
            //SI OCURRE ALGUNA EXCEPCION EN ALGUN PROCESO O LLAMADO DE PROCESO EN EL SEGMENTO
            //TRY... EL SEGMENTO CATCH CAPTURARA ESTAS SITACIONES
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