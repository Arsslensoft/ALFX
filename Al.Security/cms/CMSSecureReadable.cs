using System;

using Al.Security.Asn1.X509;
using Al.Security.Crypto.Parameters;

namespace Al.Security.Cms
{
	internal interface CmsSecureReadable
	{
		AlgorithmIdentifier Algorithm { get; }
		object CryptoObject { get; }
		CmsReadable GetReadable(KeyParameter key);
	}
}
