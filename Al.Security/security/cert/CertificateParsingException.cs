using System;

namespace Al.Security.Security.Certificates
{
	public class CertificateParsingException : CertificateException
	{
		public CertificateParsingException() : base() { }
		public CertificateParsingException(string message) : base(message) { }
		public CertificateParsingException(string message, Exception exception) : base(message, exception) { }
	}
}
