using System;

using Al.Security.Crypto;
using Al.Security.Utilities.IO;

namespace Al.Security.Cms
{
	internal class MacOutputStream
		: BaseOutputStream
	{
		private readonly IMac mac;

		internal MacOutputStream(IMac mac)
		{
			this.mac = mac;
		}

		public override void Write(byte[] b, int off, int len)
		{
			mac.BlockUpdate(b, off, len);
		}

		public override void WriteByte(byte b)
		{
			mac.Update(b);
		}
	}
}
