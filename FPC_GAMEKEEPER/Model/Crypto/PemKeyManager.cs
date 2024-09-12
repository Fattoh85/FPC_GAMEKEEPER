using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace FPC.Model.Crypto
{
    public static class PemKeyManager
    {

        // публичный ключ  V1
        static readonly string publicKeyPemOld = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjuhLNBh9GAK4CaLROE0E
iI/ZPjxpzjUDcyVc/m47rjXgxGjqLq3qcX0Y2qBRryoLBV5k2M1c63SE5dcEdnHY
/ZUlqSfHbasd95ByOF//jIbmPenAljclylzuH/KS4t9ZF+OjjubE7m5fxS5y22hW
nA13CWmpnByNnrqkO4XMwwtjIHCw3j8Y566nFo/Tq8wMVaIsAJooZ1v8r99gZ806
2IF5pN1wj4cXnywzS+mlXq8XsQfDmsuSHjW40mX0ch1sBcFT3bw7lTvD0Jk9K2Bp
Bzg9T9i5XiksUz0qrBlT7qB07ION/JywMNHWVbaUHDVUaLIv1SdkjeMIfZrHkAKw
bwIDAQAB
-----END PUBLIC KEY-----";



        // публичный ключ  V1.0.4
        static readonly string publicKeyPem = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA26vT6ZwN6iy3ZmMmpp5F
jmc/vOTHMbND/flWFSJfLSYQkDghkWuzModz4y6c0502qDlPVR3jQp0vrYAiv/Ky
0CkYEksxZKr9sCUvGFXygWzKAwhBv7622nVs9KZ190Y/w8q8flhsmb8Aaj0PNFPd
+BmjumyIQ3pOUQBkAIGd7Bgt1xnQbrFBNscridf3C6i/bix7McsOkAsuHKwtFbEx
ATqlYr1VvDUDs4Z5ju11ymX2dF2KjYRUoVTsUCgmv0uYDK/I+r6Z0XwFxuzkh2bS
2Qb+Q/RrwUa75G9dtKcgV0RDyApe0KtVsWa+k8ut/WS6vinoHQG0i4dFFEIzWLNC
fQIDAQAB
-----END PUBLIC KEY-----";


        public static void SaveKeyPair(AsymmetricCipherKeyPair keyPair, string privateKeyPath, string publicKeyPath)
        {
            using (var privateWriter = new StreamWriter(privateKeyPath))
            {
                var pemWriter = new PemWriter(privateWriter);
                pemWriter.WriteObject(keyPair.Private);
            }

            using (var publicWriter = new StreamWriter(publicKeyPath))
            {
                var pemWriter = new PemWriter(publicWriter);
                pemWriter.WriteObject(keyPair.Public);
            }
        }

        public static AsymmetricKeyParameter LoadKeyPair(string privateKeyPath, string publicKeyPath = null)
        {
            
            AsymmetricKeyParameter publicKey;

            

            if (publicKeyPath != null)
            {
                using (var publicReader = new StreamReader(publicKeyPath))
                {
                    var pemReaderPublic = new PemReader(publicReader);
                    publicKey = (AsymmetricKeyParameter)pemReaderPublic.ReadObject();
                    //throw new InvalidOperationException("Public key file in project folder");
                }
            }
            else
            {
                publicKey = LoadPublicKey(publicKeyPem);
            }


            return publicKey;
        }

        public static AsymmetricKeyParameter LoadPublicKey(string publicKeyPem)
        {
            var pemReader = new PemReader(new StringReader(publicKeyPem));
            var keyPair = (AsymmetricKeyParameter)pemReader.ReadObject();
            return keyPair;
        }

    }
}
