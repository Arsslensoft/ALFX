using System;
using System.IO;

namespace Al.Security.Crypto.Tls
{
	public class TlsFatalAlert
		: IOException
	{
		private readonly AlertDescription alertDescription;

		public TlsFatalAlert(AlertDescription alertDescription)
		{
			this.alertDescription = alertDescription;
		}

		public AlertDescription AlertDescription
		{
			get { return alertDescription; }
		}
	}
}
