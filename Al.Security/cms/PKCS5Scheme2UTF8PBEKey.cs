using System;

using Al.Security.Asn1.Pkcs;
using Al.Security.Asn1.X509;
using Al.Security.Crypto;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.Parameters;

namespace Al.Security.Cms
{
	/**
	 * PKCS5 scheme-2 - password converted to bytes using UTF-8.
	 */
	public class Pkcs5Scheme2Utf8PbeKey
		: CmsPbeKey
	{
		[Obsolete("Use version taking 'char[]' instead")]
		public Pkcs5Scheme2Utf8PbeKey(
			string	password,
			byte[]	salt,
			int		iterationCount)
			: this(password.ToCharArray(), salt, iterationCount)
		{
		}

		[Obsolete("Use version taking 'char[]' instead")]
		public Pkcs5Scheme2Utf8PbeKey(
			string				password,
			AlgorithmIdentifier keyDerivationAlgorithm)
			: this(password.ToCharArray(), keyDerivationAlgorithm)
		{
		}

		public Pkcs5Scheme2Utf8PbeKey(
			char[]	password,
			byte[]	salt,
			int		iterationCount)
			: base(password, salt, iterationCount)
		{
		}

		public Pkcs5Scheme2Utf8PbeKey(
			char[]				password,
			AlgorithmIdentifier keyDerivationAlgorithm)
			: base(password, keyDerivationAlgorithm)
		{
		}

		internal override KeyParameter GetEncoded(
			string algorithmOid)
		{
			Pkcs5S2ParametersGenerator gen = new Pkcs5S2ParametersGenerator();

			gen.Init(
				PbeParametersGenerator.Pkcs5PasswordToUtf8Bytes(password),
				salt,
				iterationCount);

			return (KeyParameter) gen.GenerateDerivedParameters(
				algorithmOid,
				CmsEnvelopedHelper.Instance.GetKeySize(algorithmOid));
		}
	}
}
