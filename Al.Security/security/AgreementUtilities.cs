using System.Collections;
using System.Globalization;

using Al.Security.Asn1;
using Al.Security.Asn1.X9;
using Al.Security.Crypto;
using Al.Security.Crypto.Agreement;
using Al.Security.Crypto.Agreement.Kdf;
using Al.Security.Crypto.Digests;
using Al.Security.Utilities;

namespace Al.Security.Security
{
	/// <remarks>
	///  Utility class for creating IBasicAgreement objects from their names/Oids
	/// </remarks>
	public sealed class AgreementUtilities
	{
		private AgreementUtilities()
		{
		}

		private static readonly IDictionary algorithms = Platform.CreateHashtable();
        //private static readonly IDictionary oids = Platform.CreateHashtable();

		static AgreementUtilities()
		{
			//algorithms[X9ObjectIdentifiers.DHSinglePassCofactorDHSha1KdfScheme.Id] = ?;
			algorithms[X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme.Id] = "ECDHWITHSHA1KDF";
			algorithms[X9ObjectIdentifiers.MqvSinglePassSha1KdfScheme.Id] = "ECMQVWITHSHA1KDF";
		}

		public static IBasicAgreement GetBasicAgreement(
			DerObjectIdentifier oid)
		{
			return GetBasicAgreement(oid.Id);
		}

		public static IBasicAgreement GetBasicAgreement(
			string algorithm)
		{
			string upper = algorithm.ToUpper(CultureInfo.InvariantCulture);
			string mechanism = (string) algorithms[upper];

			if (mechanism == null)
			{
				mechanism = upper;
			}

			if (mechanism == "DH" || mechanism == "DIFFIEHELLMAN")
				return new DHBasicAgreement();

			if (mechanism == "ECDH")
				return new ECDHBasicAgreement();

			if (mechanism == "ECDHC")
				return new ECDHCBasicAgreement();

			if (mechanism == "ECMQV")
				return new ECMqvBasicAgreement();

			throw new SecurityUtilityException("Basic Agreement " + algorithm + " not recognised.");
		}

		public static IBasicAgreement GetBasicAgreementWithKdf(
			DerObjectIdentifier oid,
			string				wrapAlgorithm)
		{
			return GetBasicAgreementWithKdf(oid.Id, wrapAlgorithm);
		}

		public static IBasicAgreement GetBasicAgreementWithKdf(
			string agreeAlgorithm,
			string wrapAlgorithm)
		{
			string upper = agreeAlgorithm.ToUpper(CultureInfo.InvariantCulture);
			string mechanism = (string) algorithms[upper];

			if (mechanism == null)
			{
				mechanism = upper;
			}

			// 'DHWITHSHA1KDF' retained for backward compatibility
			if (mechanism == "DHWITHSHA1KDF" || mechanism == "ECDHWITHSHA1KDF")
				return new ECDHWithKdfBasicAgreement(
					wrapAlgorithm,
					new ECDHKekGenerator(
						new Sha1Digest()));

			if (mechanism == "ECMQVWITHSHA1KDF")
				return new ECMqvWithKdfBasicAgreement(
					wrapAlgorithm,
					new ECDHKekGenerator(
						new Sha1Digest()));

			throw new SecurityUtilityException("Basic Agreement (with KDF) " + agreeAlgorithm + " not recognised.");
		}

		public static string GetAlgorithmName(
			DerObjectIdentifier oid)
		{
			return (string) algorithms[oid.Id];
		}
	}
}
