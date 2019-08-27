using System;
using System.IO;

namespace Al.Security.Crypto.Tls
{
	public interface TlsCipherFactory
	{
		/// <exception cref="IOException"></exception>
		TlsCipher CreateCipher(TlsClientContext context, EncryptionAlgorithm encryptionAlgorithm,
			DigestAlgorithm digestAlgorithm);
	}
}
