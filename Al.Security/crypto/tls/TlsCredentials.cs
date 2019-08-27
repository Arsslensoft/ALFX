using System;

namespace Al.Security.Crypto.Tls
{
	public interface TlsCredentials
	{
		Certificate Certificate { get; }
	}
}
