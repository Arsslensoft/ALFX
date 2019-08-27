using System;
using System.Collections;
using System.IO;

using Al.Security.Asn1;
using Al.Security.Asn1.Ocsp;
using Al.Security.Asn1.X509;
using Al.Security.X509;

namespace Al.Security.Ocsp
{
	public class Req
		: X509ExtensionBase
	{
		private Request req;

		public Req(
			Request req)
		{
			this.req = req;
		}

		public CertificateID GetCertID()
		{
			return new CertificateID(req.ReqCert);
		}

		public X509Extensions SingleRequestExtensions
		{
			get { return req.SingleRequestExtensions; }
		}

		protected override X509Extensions GetX509Extensions()
		{
			return SingleRequestExtensions;
		}
	}
}
