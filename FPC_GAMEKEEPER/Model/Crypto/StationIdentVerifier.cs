using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto;

namespace FPC.Model.Crypto
{
    public static class StationIdentVerifier
    {
        public static bool VerifyStationIdent(string macAddress, byte[] signature, AsymmetricKeyParameter publicKey)
        {
            var signer = new PssSigner(new RsaEngine(), new Sha256Digest(), 20);
            signer.Init(false, publicKey);

            var data = Encoding.UTF8.GetBytes(macAddress);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.VerifySignature(signature);
        }
    }
}
