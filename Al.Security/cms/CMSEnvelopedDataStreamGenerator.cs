using System;
using System.Collections;
using System.IO;

using Al.Security.Asn1;
using Al.Security.Asn1.Cms;
using Al.Security.Asn1.Nist;
using Al.Security.Asn1.X509;
using Al.Security.Crypto;
using Al.Security.Crypto.Engines;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.IO;
using Al.Security.Crypto.Parameters;
using Al.Security.Security;
using Al.Security.Security.Certificates;
using Al.Security.Utilities;
using Al.Security.Utilities.IO;
using Al.Security.X509;

namespace Al.Security.Cms
{
	/**
	* General class for generating a CMS enveloped-data message stream.
	* <p>
	* A simple example of usage.
	* <pre>
	*      CmsEnvelopedDataStreamGenerator edGen = new CmsEnvelopedDataStreamGenerator();
	*
	*      edGen.AddKeyTransRecipient(cert);
	*
	*      MemoryStream  bOut = new MemoryStream();
	*
	*      Stream out = edGen.Open(
	*                              bOut, CMSEnvelopedDataGenerator.AES128_CBC);*
	*      out.Write(data);
	*
	*      out.Close();
	* </pre>
	* </p>
	*/
	public class CmsEnvelopedDataStreamGenerator
		: CmsEnvelopedGenerator
	{
		private object	_originatorInfo = null;
		private object	_unprotectedAttributes = null;
		private int		_bufferSize;
		private bool	_berEncodeRecipientSet;

		public CmsEnvelopedDataStreamGenerator()
		{
		}

		/// <summary>Constructor allowing specific source of randomness</summary>
		/// <param name="rand">Instance of <c>SecureRandom</c> to use.</param>
		public CmsEnvelopedDataStreamGenerator(
			SecureRandom rand)
			: base(rand)
		{
		}

		/// <summary>Set the underlying string size for encapsulated data.</summary>
		/// <param name="bufferSize">Length of octet strings to buffer the data.</param>
		public void SetBufferSize(
			int bufferSize)
		{
			_bufferSize = bufferSize;
		}

		/// <summary>Use a BER Set to store the recipient information.</summary>
		public void SetBerEncodeRecipients(
			bool berEncodeRecipientSet)
		{
			_berEncodeRecipientSet = berEncodeRecipientSet;
		}

		private DerInteger Version
		{
			get
			{
				int version = (_originatorInfo != null || _unprotectedAttributes != null)
					?	2
					:	0;

				return new DerInteger(version);
			}
		}

		/// <summary>
		/// Generate an enveloped object that contains an CMS Enveloped Data
		/// object using the passed in key generator.
		/// </summary>
		private Stream Open(
			Stream				outStream,
			string				encryptionOid,
			CipherKeyGenerator	keyGen)
		{
			byte[] encKeyBytes = keyGen.GenerateKey();
			KeyParameter encKey = ParameterUtilities.CreateKeyParameter(encryptionOid, encKeyBytes);

			Asn1Encodable asn1Params = GenerateAsn1Parameters(encryptionOid, encKeyBytes);

			ICipherParameters cipherParameters;
			AlgorithmIdentifier encAlgID = GetAlgorithmIdentifier(
				encryptionOid, encKey, asn1Params, out cipherParameters);

			Asn1EncodableVector recipientInfos = new Asn1EncodableVector();

			foreach (RecipientInfoGenerator rig in recipientInfoGenerators)
			{
				try
				{
					recipientInfos.Add(rig.Generate(encKey, rand));
				}
				catch (InvalidKeyException e)
				{
					throw new CmsException("key inappropriate for algorithm.", e);
				}
				catch (GeneralSecurityException e)
				{
					throw new CmsException("error making encrypted content.", e);
				}
			}

			return Open(outStream, encAlgID, cipherParameters, recipientInfos);
		}

		private Stream Open(
			Stream				outStream,
			AlgorithmIdentifier	encAlgID,
			ICipherParameters	cipherParameters,
			Asn1EncodableVector	recipientInfos)
		{
			try
			{
				//
				// ContentInfo
				//
				BerSequenceGenerator cGen = new BerSequenceGenerator(outStream);

				cGen.AddObject(CmsObjectIdentifiers.EnvelopedData);

				//
				// Encrypted Data
				//
				BerSequenceGenerator envGen = new BerSequenceGenerator(
					cGen.GetRawOutputStream(), 0, true);

				envGen.AddObject(this.Version);

				Stream envRaw = envGen.GetRawOutputStream();
				Asn1Generator recipGen = _berEncodeRecipientSet
					?	(Asn1Generator) new BerSetGenerator(envRaw)
					:	new DerSetGenerator(envRaw);

				foreach (Asn1Encodable ae in recipientInfos)
				{
					recipGen.AddObject(ae);
				}

				recipGen.Close();

				BerSequenceGenerator eiGen = new BerSequenceGenerator(envRaw);
				eiGen.AddObject(CmsObjectIdentifiers.Data);
				eiGen.AddObject(encAlgID);

				Stream octetOutputStream = CmsUtilities.CreateBerOctetOutputStream(
					eiGen.GetRawOutputStream(), 0, false, _bufferSize);

				IBufferedCipher cipher = CipherUtilities.GetCipher(encAlgID.ObjectID);
				cipher.Init(true, new ParametersWithRandom(cipherParameters, rand));
				CipherStream cOut = new CipherStream(octetOutputStream, null, cipher);

				return new CmsEnvelopedDataOutputStream(this, cOut, cGen, envGen, eiGen);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e)
			{
				throw new CmsException("key invalid in message.", e);
			}
			catch (IOException e)
			{
				throw new CmsException("exception decoding algorithm parameters.", e);
			}
		}

		/**
		* generate an enveloped object that contains an CMS Enveloped Data object
		* @throws IOException
		*/
		public Stream Open(
			Stream	outStream,
			string	encryptionOid)
		{
			CipherKeyGenerator keyGen = GeneratorUtilities.GetKeyGenerator(encryptionOid);

			keyGen.Init(new KeyGenerationParameters(rand, keyGen.DefaultStrength));

			return Open(outStream, encryptionOid, keyGen);
		}

		/**
		* generate an enveloped object that contains an CMS Enveloped Data object
		* @throws IOException
		*/
		public Stream Open(
			Stream	outStream,
			string	encryptionOid,
			int		keySize)
		{
			CipherKeyGenerator keyGen = GeneratorUtilities.GetKeyGenerator(encryptionOid);

			keyGen.Init(new KeyGenerationParameters(rand, keySize));

			return Open(outStream, encryptionOid, keyGen);
		}

		private class CmsEnvelopedDataOutputStream
			: BaseOutputStream
		{
            private readonly CmsEnvelopedGenerator _outer;

			private readonly CipherStream			_out;
			private readonly BerSequenceGenerator	_cGen;
			private readonly BerSequenceGenerator	_envGen;
			private readonly BerSequenceGenerator	_eiGen;

			public CmsEnvelopedDataOutputStream(
				CmsEnvelopedGenerator	outer,
				CipherStream			outStream,
				BerSequenceGenerator	cGen,
				BerSequenceGenerator	envGen,
				BerSequenceGenerator	eiGen)
			{
				_outer = outer;
				_out = outStream;
				_cGen = cGen;
				_envGen = envGen;
				_eiGen = eiGen;
			}

			public override void WriteByte(
				byte b)
			{
				_out.WriteByte(b);
			}

			public override void Write(
				byte[]	bytes,
				int		off,
				int		len)
			{
				_out.Write(bytes, off, len);
			}

			public override void Close()
			{
				_out.Close();

				// TODO Parent context(s) should really be be closed explicitly

				_eiGen.Close();

                if (_outer.unprotectedAttributeGenerator != null)
                {
                    Asn1.Cms.AttributeTable attrTable = _outer.unprotectedAttributeGenerator.GetAttributes(Platform.CreateHashtable());

                    Asn1Set unprotectedAttrs = new BerSet(attrTable.ToAsn1EncodableVector());

                    _envGen.AddObject(new DerTaggedObject(false, 1, unprotectedAttrs));
                }

				_envGen.Close();
				_cGen.Close();
				base.Close();
			}
		}
	}
}
