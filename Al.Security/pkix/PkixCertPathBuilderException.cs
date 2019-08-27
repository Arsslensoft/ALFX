using System;

using Al.Security.Security;

namespace Al.Security.Pkix
{
	/// <summary>
	/// Summary description for PkixCertPathBuilderException.
	/// </summary>
	public class PkixCertPathBuilderException : GeneralSecurityException
	{
		public PkixCertPathBuilderException() : base() { }
		
		public PkixCertPathBuilderException(string message) : base(message)	{ }  

		public PkixCertPathBuilderException(string message, Exception exception) : base(message, exception) { }
		
	}
}
