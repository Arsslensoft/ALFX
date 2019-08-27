using System;
using System.IO;

namespace Al.Security.Crypto.Tls
{
	public class TlsNullCompression
		: TlsCompression
	{
		public virtual Stream Compress(Stream output)
		{
			return output;
		}

		public virtual Stream Decompress(Stream output)
		{
			return output;
		}
	}
}
