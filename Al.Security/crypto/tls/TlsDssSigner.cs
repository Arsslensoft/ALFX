using System;

using Al.Security.Crypto.Parameters;
using Al.Security.Crypto.Signers;

namespace Al.Security.Crypto.Tls
{
	internal class TlsDssSigner
		: TlsDsaSigner
	{
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is DsaPublicKeyParameters;
		}

	    protected override IDsa CreateDsaImpl()
	    {
			return new DsaSigner();
	    }
	}
}
