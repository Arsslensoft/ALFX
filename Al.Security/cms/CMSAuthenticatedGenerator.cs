using System;
using System.IO;

using Al.Security.Asn1;
using Al.Security.Asn1.X509;
using Al.Security.Crypto;
using Al.Security.Crypto.Parameters;
using Al.Security.Security;
using Al.Security.Utilities.Date;
using Al.Security.Utilities.IO;

namespace Al.Security.Cms
{
	public class CmsAuthenticatedGenerator
		: CmsEnvelopedGenerator
	{
		/**
		* base constructor
		*/
		public CmsAuthenticatedGenerator()
		{
		}

		/**
		* constructor allowing specific source of randomness
		*
		* @param rand instance of SecureRandom to use
		*/
		public CmsAuthenticatedGenerator(
			SecureRandom rand)
			: base(rand)
		{
		}
	}
}
