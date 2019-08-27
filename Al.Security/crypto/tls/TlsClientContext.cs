using System;

using Al.Security.Security;

namespace Al.Security.Crypto.Tls
{
	public interface TlsClientContext
	{
		SecureRandom SecureRandom { get; }

		SecurityParameters SecurityParameters { get; }

		object UserObject { get; set; }
	}
}
