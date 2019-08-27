using Al.Security.Asn1;
using Al.Security.Asn1.X509;

namespace Al.Security.Asn1.Pkcs
{
	public class KeyDerivationFunc
		: AlgorithmIdentifier
	{
		internal KeyDerivationFunc(Asn1Sequence seq)
			: base(seq)
		{
		}

		public KeyDerivationFunc(
			DerObjectIdentifier	id,
			Asn1Encodable		parameters)
			: base(id, parameters)
		{
		}
	}
}