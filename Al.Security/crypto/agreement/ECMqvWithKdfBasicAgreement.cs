using System;
using System.Collections;

using Al.Security.Asn1;
using Al.Security.Asn1.Nist;
using Al.Security.Asn1.Pkcs;
using Al.Security.Asn1.X9;
using Al.Security.Crypto.Agreement.Kdf;
using Al.Security.Crypto.Parameters;
using Al.Security.Math;
using Al.Security.Security;

namespace Al.Security.Crypto.Agreement
{
	public class ECMqvWithKdfBasicAgreement
		: ECMqvBasicAgreement
	{
		private readonly string algorithm;
		private readonly IDerivationFunction kdf;

		public ECMqvWithKdfBasicAgreement(
			string				algorithm,
			IDerivationFunction	kdf)
		{
			if (algorithm == null)
				throw new ArgumentNullException("algorithm");
			if (kdf == null)
				throw new ArgumentNullException("kdf");

			this.algorithm = algorithm;
			this.kdf = kdf;
		}

		public override BigInteger CalculateAgreement(
			ICipherParameters pubKey)
		{
			// Note that the ec.KeyAgreement class in JCE only uses kdf in one
			// of the engineGenerateSecret methods.

			BigInteger result = base.CalculateAgreement(pubKey);

			int keySize = GeneratorUtilities.GetDefaultKeySize(algorithm);

			DHKdfParameters dhKdfParams = new DHKdfParameters(
				new DerObjectIdentifier(algorithm),
				keySize,
				bigIntToBytes(result));

			kdf.Init(dhKdfParams);

			byte[] keyBytes = new byte[keySize / 8];
			kdf.GenerateBytes(keyBytes, 0, keyBytes.Length);

			return new BigInteger(1, keyBytes);
		}

		private byte[] bigIntToBytes(
			BigInteger r)
		{
			int byteLength = X9IntegerConverter.GetByteLength(privParams.StaticPrivateKey.Parameters.G.X);
			return X9IntegerConverter.IntegerToBytes(r, byteLength);
		}
	}
}
