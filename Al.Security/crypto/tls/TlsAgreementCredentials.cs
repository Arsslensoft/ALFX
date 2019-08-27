using System;
using System.IO;

namespace Al.Security.Crypto.Tls
{
	public interface TlsAgreementCredentials : TlsCredentials
	{
		/// <exception cref="IOException"></exception>
		byte[] GenerateAgreement(AsymmetricKeyParameter serverPublicKey);
	}
}
