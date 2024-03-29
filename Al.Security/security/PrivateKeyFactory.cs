using System;
using System.Collections;
using System.IO;
using System.Text;

using Al.Security.Asn1;
using Al.Security.Asn1.CryptoPro;
using Al.Security.Asn1.Oiw;
using Al.Security.Asn1.Pkcs;
using Al.Security.Asn1.Sec;
using Al.Security.Asn1.X509;
using Al.Security.Asn1.X9;
using Al.Security.Crypto;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.Parameters;
using Al.Security.Math;
using Al.Security.Pkcs;

namespace Al.Security.Security
{
    public sealed class PrivateKeyFactory
    {
        private PrivateKeyFactory()
        {
        }

		public static AsymmetricKeyParameter CreateKey(
			byte[] privateKeyInfoData)
		{
			return CreateKey(
				PrivateKeyInfo.GetInstance(
					Asn1Object.FromByteArray(privateKeyInfoData)));
		}

		public static AsymmetricKeyParameter CreateKey(
			Stream inStr)
		{
			return CreateKey(
				PrivateKeyInfo.GetInstance(
					Asn1Object.FromStream(inStr)));
		}

		public static AsymmetricKeyParameter CreateKey(
			PrivateKeyInfo keyInfo)
        {
            AlgorithmIdentifier algID = keyInfo.AlgorithmID;
			DerObjectIdentifier algOid = algID.ObjectID;

			// TODO See RSAUtil.isRsaOid in Java build
			if (algOid.Equals(PkcsObjectIdentifiers.RsaEncryption)
				|| algOid.Equals(X509ObjectIdentifiers.IdEARsa)
				|| algOid.Equals(PkcsObjectIdentifiers.IdRsassaPss)
				|| algOid.Equals(PkcsObjectIdentifiers.IdRsaesOaep))
			{
				RsaPrivateKeyStructure keyStructure = new RsaPrivateKeyStructure(
					Asn1Sequence.GetInstance(keyInfo.PrivateKey));

				return new RsaPrivateCrtKeyParameters(
					keyStructure.Modulus,
					keyStructure.PublicExponent,
					keyStructure.PrivateExponent,
					keyStructure.Prime1,
					keyStructure.Prime2,
					keyStructure.Exponent1,
					keyStructure.Exponent2,
					keyStructure.Coefficient);
			}
			// TODO?
//			else if (algOid.Equals(X9ObjectIdentifiers.DHPublicNumber))
			else if (algOid.Equals(PkcsObjectIdentifiers.DhKeyAgreement))
			{
				DHParameter para = new DHParameter(
					Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));
				DerInteger derX = (DerInteger)keyInfo.PrivateKey;

				BigInteger lVal = para.L;
				int l = lVal == null ? 0 : lVal.IntValue;
				DHParameters dhParams = new DHParameters(para.P, para.G, null, l);

				return new DHPrivateKeyParameters(derX.Value, dhParams, algOid);
			}
			else if (algOid.Equals(OiwObjectIdentifiers.ElGamalAlgorithm))
			{
				ElGamalParameter  para = new ElGamalParameter(
					Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));
				DerInteger derX = (DerInteger)keyInfo.PrivateKey;

				return new ElGamalPrivateKeyParameters(
					derX.Value,
					new ElGamalParameters(para.P, para.G));
			}
			else if (algOid.Equals(X9ObjectIdentifiers.IdDsa))
			{
				DerInteger derX = (DerInteger) keyInfo.PrivateKey;
				Asn1Encodable ae = algID.Parameters;

				DsaParameters parameters = null;
				if (ae != null)
				{
					DsaParameter para = DsaParameter.GetInstance(ae.ToAsn1Object());
					parameters = new DsaParameters(para.P, para.Q, para.G);
				}

				return new DsaPrivateKeyParameters(derX.Value, parameters);
			}
			else if (algOid.Equals(X9ObjectIdentifiers.IdECPublicKey))
			{
				X962Parameters para = new X962Parameters(algID.Parameters.ToAsn1Object());
				X9ECParameters ecP;

				if (para.IsNamedCurve)
				{
					ecP = ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier) para.Parameters);
				}
				else
				{
					ecP = new X9ECParameters((Asn1Sequence) para.Parameters);
				}

				ECDomainParameters dParams = new ECDomainParameters(
					ecP.Curve,
					ecP.G,
					ecP.N,
					ecP.H,
					ecP.GetSeed());

				ECPrivateKeyStructure ec = new ECPrivateKeyStructure(
					Asn1Sequence.GetInstance(keyInfo.PrivateKey));

				return new ECPrivateKeyParameters(ec.GetKey(), dParams);
			}
			else if (algOid.Equals(CryptoProObjectIdentifiers.GostR3410x2001))
			{
				Gost3410PublicKeyAlgParameters gostParams = new Gost3410PublicKeyAlgParameters(
					Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));

				ECPrivateKeyStructure ec = new ECPrivateKeyStructure(
					Asn1Sequence.GetInstance(keyInfo.PrivateKey));

				ECDomainParameters ecP = ECGost3410NamedCurves.GetByOid(gostParams.PublicKeyParamSet);

				if (ecP == null)
					return null;

				return new ECPrivateKeyParameters("ECGOST3410", ec.GetKey(), gostParams.PublicKeyParamSet);
			}
			else if (algOid.Equals(CryptoProObjectIdentifiers.GostR3410x94))
			{
				Gost3410PublicKeyAlgParameters gostParams = new Gost3410PublicKeyAlgParameters(
					Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));

				DerOctetString derX = (DerOctetString) keyInfo.PrivateKey;
				byte[] keyEnc = derX.GetOctets();
				byte[] keyBytes = new byte[keyEnc.Length];

				for (int i = 0; i != keyEnc.Length; i++)
				{
					keyBytes[i] = keyEnc[keyEnc.Length - 1 - i]; // was little endian
				}

				BigInteger x = new BigInteger(1, keyBytes);

				return new Gost3410PrivateKeyParameters(x, gostParams.PublicKeyParamSet);
			}
			else
			{
				throw new SecurityUtilityException("algorithm identifier in key not recognised");
			}
        }

		public static AsymmetricKeyParameter DecryptKey(
			char[]					passPhrase,
			EncryptedPrivateKeyInfo	encInfo)
		{
			return CreateKey(PrivateKeyInfoFactory.CreatePrivateKeyInfo(passPhrase, encInfo));
		}

		public static AsymmetricKeyParameter DecryptKey(
			char[]	passPhrase,
			byte[]	encryptedPrivateKeyInfoData)
		{
			return DecryptKey(passPhrase, Asn1Object.FromByteArray(encryptedPrivateKeyInfoData));
		}

		public static AsymmetricKeyParameter DecryptKey(
			char[]	passPhrase,
			Stream	encryptedPrivateKeyInfoStream)
		{
			return DecryptKey(passPhrase, Asn1Object.FromStream(encryptedPrivateKeyInfoStream));
		}

		private static AsymmetricKeyParameter DecryptKey(
			char[]		passPhrase,
			Asn1Object	asn1Object)
		{
			return DecryptKey(passPhrase, EncryptedPrivateKeyInfo.GetInstance(asn1Object));
		}

        public static byte[] EncryptKey(
            DerObjectIdentifier		algorithm,
            char[]					passPhrase,
            byte[]					salt,
            int						iterationCount,
            AsymmetricKeyParameter	key)
        {
			return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(
				algorithm, passPhrase, salt, iterationCount, key).GetEncoded();
        }

		public static byte[] EncryptKey(
			string					algorithm,
            char[]					passPhrase,
            byte[]					salt,
            int						iterationCount,
            AsymmetricKeyParameter	key)
        {
			return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(
				algorithm, passPhrase, salt, iterationCount, key).GetEncoded();
        }
	}
}
