using System;

using Al.Security.Asn1;
using Al.Security.Asn1.Pkcs;

namespace Al.Security.Pkcs
{
	public class Pkcs12StoreBuilder
	{
		private DerObjectIdentifier	keyAlgorithm = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc;
		private DerObjectIdentifier	certAlgorithm = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc;
		private bool useDerEncoding = false;

		public Pkcs12StoreBuilder()
		{
		}

		public Pkcs12Store Build()
		{
			return new Pkcs12Store(keyAlgorithm, certAlgorithm, useDerEncoding);
		}

		public Pkcs12StoreBuilder SetCertAlgorithm(DerObjectIdentifier certAlgorithm)
		{
			this.certAlgorithm = certAlgorithm;
			return this;
		}

		public Pkcs12StoreBuilder SetKeyAlgorithm(DerObjectIdentifier keyAlgorithm)
		{
			this.keyAlgorithm = keyAlgorithm;
			return this;
		}

		public Pkcs12StoreBuilder SetUseDerEncoding(bool useDerEncoding)
		{
			this.useDerEncoding = useDerEncoding;
			return this;
		}
	}
}
