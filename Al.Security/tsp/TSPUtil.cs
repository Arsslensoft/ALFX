using System;
using System.Collections;
using System.IO;

using Al.Security.Asn1;
using Al.Security.Asn1.CryptoPro;
using Al.Security.Asn1.Nist;
using Al.Security.Asn1.Oiw;
using Al.Security.Asn1.Pkcs;
using Al.Security.Asn1.TeleTrust;
using Al.Security.Asn1.X509;
using Al.Security.Cms;
using Al.Security.Crypto;
using Al.Security.Security;
using Al.Security.Utilities;
using Al.Security.Utilities.Collections;
using Al.Security.X509;

namespace Al.Security.Tsp
{
	public class TspUtil
	{
		//private static ISet EmptySet = CollectionUtilities.ReadOnly(new HashSet());
		//private static IList EmptyList = CollectionUtilities.ReadOnly(Platform.CreateArrayList());

		private static readonly IDictionary digestLengths = Platform.CreateHashtable();
        private static readonly IDictionary digestNames = Platform.CreateHashtable();

		static TspUtil()
		{
			digestLengths.Add(PkcsObjectIdentifiers.MD5.Id, 16);
			digestLengths.Add(OiwObjectIdentifiers.IdSha1.Id, 20);
			digestLengths.Add(NistObjectIdentifiers.IdSha224.Id, 28);
			digestLengths.Add(NistObjectIdentifiers.IdSha256.Id, 32);
			digestLengths.Add(NistObjectIdentifiers.IdSha384.Id, 48);
			digestLengths.Add(NistObjectIdentifiers.IdSha512.Id, 64);
			digestLengths.Add(TeleTrusTObjectIdentifiers.RipeMD128.Id, 16);
			digestLengths.Add(TeleTrusTObjectIdentifiers.RipeMD160.Id, 20);
			digestLengths.Add(TeleTrusTObjectIdentifiers.RipeMD256.Id, 32);
			digestLengths.Add(CryptoProObjectIdentifiers.GostR3411.Id, 32);

			digestNames.Add(PkcsObjectIdentifiers.MD5.Id, "MD5");
			digestNames.Add(OiwObjectIdentifiers.IdSha1.Id, "SHA1");
			digestNames.Add(NistObjectIdentifiers.IdSha224.Id, "SHA224");
			digestNames.Add(NistObjectIdentifiers.IdSha256.Id, "SHA256");
			digestNames.Add(NistObjectIdentifiers.IdSha384.Id, "SHA384");
			digestNames.Add(NistObjectIdentifiers.IdSha512.Id, "SHA512");
			digestNames.Add(PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id, "SHA1");
			digestNames.Add(PkcsObjectIdentifiers.Sha224WithRsaEncryption.Id, "SHA224");
			digestNames.Add(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id, "SHA256");
			digestNames.Add(PkcsObjectIdentifiers.Sha384WithRsaEncryption.Id, "SHA384");
			digestNames.Add(PkcsObjectIdentifiers.Sha512WithRsaEncryption.Id, "SHA512");
			digestNames.Add(TeleTrusTObjectIdentifiers.RipeMD128.Id, "RIPEMD128");
			digestNames.Add(TeleTrusTObjectIdentifiers.RipeMD160.Id, "RIPEMD160");
			digestNames.Add(TeleTrusTObjectIdentifiers.RipeMD256.Id, "RIPEMD256");
			digestNames.Add(CryptoProObjectIdentifiers.GostR3411.Id, "GOST3411");
		}


	    /**
	     * Fetches the signature time-stamp attributes from a SignerInformation object.
	     * Checks that the MessageImprint for each time-stamp matches the signature field.
	     * (see RFC 3161 Appendix A).
	     *
	     * @param signerInfo a SignerInformation to search for time-stamps
	     * @return a collection of TimeStampToken objects
	     * @throws TSPValidationException
	     */
		public static ICollection GetSignatureTimestamps(
			SignerInformation signerInfo)
		{
			IList timestamps = Platform.CreateArrayList();

			Asn1.Cms.AttributeTable unsignedAttrs = signerInfo.UnsignedAttributes;
			if (unsignedAttrs != null)
			{
				foreach (Asn1.Cms.Attribute tsAttr in unsignedAttrs.GetAll(
					PkcsObjectIdentifiers.IdAASignatureTimeStampToken))
				{
					foreach (Asn1Encodable asn1 in tsAttr.AttrValues)
					{
						try
						{
							Asn1.Cms.ContentInfo contentInfo = Asn1.Cms.ContentInfo.GetInstance(
								asn1.ToAsn1Object());
							TimeStampToken timeStampToken = new TimeStampToken(contentInfo);
							TimeStampTokenInfo tstInfo = timeStampToken.TimeStampInfo;

							byte[] expectedDigest = DigestUtilities.CalculateDigest(
								GetDigestAlgName(tstInfo.MessageImprintAlgOid),
							    signerInfo.GetSignature());

							if (!Arrays.ConstantTimeAreEqual(expectedDigest, tstInfo.GetMessageImprintDigest()))
								throw new TspValidationException("Incorrect digest in message imprint");

							timestamps.Add(timeStampToken);
						}
						catch (SecurityUtilityException)
						{
							throw new TspValidationException("Unknown hash algorithm specified in timestamp");
						}
						catch (Exception)
						{
							throw new TspValidationException("Timestamp could not be parsed");
						}
					}
				}
			}

			return timestamps;
		}

		/**
		 * Validate the passed in certificate as being of the correct type to be used
		 * for time stamping. To be valid it must have an ExtendedKeyUsage extension
		 * which has a key purpose identifier of id-kp-timeStamping.
		 *
		 * @param cert the certificate of interest.
		 * @throws TspValidationException if the certicate fails on one of the check points.
		 */
		public static void ValidateCertificate(
			X509Certificate cert)
		{
			if (cert.Version != 3)
				throw new ArgumentException("Certificate must have an ExtendedKeyUsage extension.");

			Asn1OctetString ext = cert.GetExtensionValue(X509Extensions.ExtendedKeyUsage);
			if (ext == null)
				throw new TspValidationException("Certificate must have an ExtendedKeyUsage extension.");

			if (!cert.GetCriticalExtensionOids().Contains(X509Extensions.ExtendedKeyUsage.Id))
				throw new TspValidationException("Certificate must have an ExtendedKeyUsage extension marked as critical.");

			try
			{
				ExtendedKeyUsage extKey = ExtendedKeyUsage.GetInstance(
					Asn1Object.FromByteArray(ext.GetOctets()));

				if (!extKey.HasKeyPurposeId(KeyPurposeID.IdKPTimeStamping) || extKey.Count != 1)
					throw new TspValidationException("ExtendedKeyUsage not solely time stamping.");
			}
			catch (IOException)
			{
				throw new TspValidationException("cannot process ExtendedKeyUsage extension");
			}
		}

		/// <summary>
		/// Return the digest algorithm using one of the standard JCA string
		/// representations rather than the algorithm identifier (if possible).
		/// </summary>
		internal static string GetDigestAlgName(
			string digestAlgOID)
		{
			string digestName = (string) digestNames[digestAlgOID];

			return digestName != null ? digestName : digestAlgOID;
		}

		internal static int GetDigestLength(
			string digestAlgOID)
		{
			try
			{
				if (digestLengths.Contains(digestAlgOID))
				{
					return (int) digestLengths[digestAlgOID];
				}

				return CreateDigestInstance(digestAlgOID).GetDigestSize();
			}
			catch (SecurityUtilityException e)
			{
				throw new TspException("digest algorithm cannot be found.", e);
			}
		}

		internal static IDigest CreateDigestInstance(
			String digestAlgOID)
		{
	        string digestName = GetDigestAlgName(digestAlgOID);

			return DigestUtilities.GetDigest(digestName);
		}
	}
}
