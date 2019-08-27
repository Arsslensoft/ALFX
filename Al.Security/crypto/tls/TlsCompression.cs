using System;
using System.IO;

namespace Al.Security.Crypto.Tls
{
	public interface TlsCompression
	{
		Stream Compress(Stream output);

		Stream Decompress(Stream output);
	}
}
