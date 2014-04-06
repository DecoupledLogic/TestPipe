//******************************************************************
//* SymetricEncryption - From Payformance \lib\PFCUtils\SymetricEncryption.cs
//*
//* Supports both TripleDes and Rijindael Symetric Encryption.
//* Key and Intial Vector values are stored in the config.xsl file.
//*
//* The rijindael will add extra security by salting the keys...
//******************************************************************

namespace TestPipe.Email.Authentication
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    // Encryption Type
    public enum EncryptionAlgorithm
    {
        Rijindael,
        TripleDes
    }

    public class SymetricEncryption
    {
        private string tripleDesIVAsString = "Lq2HPyd9";
        private string tripleDesKeyAsString = "Ui8HJke5N0uPkSSe";

        public SymetricEncryption()
        {
        }

        public byte[] TripleDesIVAsBytes
        {
            get
            {
                return Encoding.ASCII.GetBytes(this.tripleDesIVAsString);
            }
        }

        public byte[] TripleDesKeyAsBytes
        {
            get
            {
                return Encoding.ASCII.GetBytes(this.tripleDesKeyAsString);
            }
        }

        public byte[] Decrypt(byte[] dataToEncrypt, EncryptionAlgorithm encryptType)
        {
            return this.Transform(dataToEncrypt, encryptType, false);
        }

        public string DecryptFromBase64(string dataToDecrypt, EncryptionAlgorithm encryptType)
        {
            byte[] bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);
            return Encoding.ASCII.GetString(this.Transform(bytesToDecrypt, encryptType, false));
        }

        public byte[] Encrypt(byte[] dataToEncrypt, EncryptionAlgorithm encryptType)
        {
            return this.Transform(dataToEncrypt, encryptType, true);
        }

        public string EncryptToBase64(string dataToEncrypt, EncryptionAlgorithm encryptType)
        {
            byte[] bytesToEncrypt = Encoding.Default.GetBytes(dataToEncrypt);
            return Convert.ToBase64String(this.Transform(bytesToEncrypt, encryptType, true));
        }

        private ICryptoTransform GetCryptoServiceProvider(EncryptionAlgorithm encryptType, bool encrypt)
        {
            switch (encryptType)
            {
                case EncryptionAlgorithm.TripleDes:
                    {
                        TripleDES des3 = new TripleDESCryptoServiceProvider();
                        des3.Mode = CipherMode.CBC;
                        if (encrypt)
                            return des3.CreateEncryptor(this.TripleDesKeyAsBytes, this.TripleDesIVAsBytes);
                        else
                            return des3.CreateDecryptor(this.TripleDesKeyAsBytes, this.TripleDesIVAsBytes);
                    }

                default:
                    {
                        throw new CryptographicException("Invalid Encryption Algorithm");
                    }
            }
        }

        private byte[] Transform(byte[] dataToTransform, EncryptionAlgorithm encryptType, bool encrypt)
        {
            MemoryStream streamTransformedData = new MemoryStream();
            ICryptoTransform transformer = this.GetCryptoServiceProvider(encryptType, encrypt);
            CryptoStream cryptedStream = new CryptoStream(streamTransformedData, transformer, CryptoStreamMode.Write);

            try
            {
                cryptedStream.Write(dataToTransform, 0, dataToTransform.Length);
                cryptedStream.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                if (encrypt)
                    throw new Exception("Error during data encryption:  " + ex.ToString());
                else
                    throw new Exception("Error during data decryption:  " + ex.ToString());
            }

            return streamTransformedData.ToArray();
        }
    }
}