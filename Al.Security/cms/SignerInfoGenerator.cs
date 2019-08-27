using System;

using Al.Security.Asn1;
using Al.Security.Asn1.Cms;
using Al.Security.Asn1.X509;

namespace Al.Security.Cms
{
	internal interface SignerInfoGenerator
	{
		SignerInfo Generate(DerObjectIdentifier contentType, AlgorithmIdentifier digestAlgorithm,
        	byte[] calculatedDigest);
	}
}
