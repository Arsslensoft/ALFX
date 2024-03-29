using System;
using System.IO;

using Al.Security.Utilities.Zlib;

namespace Al.Security.Crypto.Tls
{
	public class TlsDeflateCompression
		: TlsCompression
	{
		protected ZStream zIn, zOut;

		public TlsDeflateCompression()
		{
			this.zIn = new ZStream();
			this.zIn.inflateInit();

			this.zOut = new ZStream();
			// TODO Allow custom setting
			this.zOut.deflateInit(JZlib.Z_DEFAULT_COMPRESSION);
		}

		public virtual Stream Compress(Stream output)
		{
			return new DeflateOutputStream(output, zOut, true);
		}

		public virtual Stream Decompress(Stream output)
		{
			return new DeflateOutputStream(output, zIn, false);
		}

		protected class DeflateOutputStream : ZOutputStream
		{
			public DeflateOutputStream(Stream output, ZStream z, bool compress)
				: base(output)
			{
				this.z = z;
				this.compress = compress;
				this.FlushMode = JZlib.Z_PARTIAL_FLUSH;
			}
		}
	}
}
