using System;
using System.IO;

namespace Al.Security.Security
{
	public class EncryptionException
		: IOException
	{
		public EncryptionException(
			string message)
			: base(message)
		{
		}

		public EncryptionException(
			string		message,
			Exception	exception)
			: base(message, exception)
		{
		}
	}
}
