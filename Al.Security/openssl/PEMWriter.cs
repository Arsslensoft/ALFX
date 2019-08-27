using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

using Al.Security.Asn1;
using Al.Security.Asn1.CryptoPro;
using Al.Security.Asn1.Pkcs;
using Al.Security.Asn1.X509;
using Al.Security.Asn1.X9;
using Al.Security.Crypto;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.Parameters;
using Al.Security.Math;
using Al.Security.Pkcs;
using Al.Security.Security;
using Al.Security.Security.Certificates;
using Al.Security.Utilities.Encoders;
using Al.Security.Utilities.IO.Pem;
using Al.Security.X509;

namespace Al.Security.OpenSsl
{
	/// <remarks>General purpose writer for OpenSSL PEM objects.</remarks>
	public class PemWriter
		: Al.Security.Utilities.IO.Pem.PemWriter
	{
		/// <param name="writer">The TextWriter object to write the output to.</param>
		public PemWriter(
			TextWriter writer)
			: base(writer)
		{
		}

		public void WriteObject(
			object obj) 
		{
			try
			{
				base.WriteObject(new MiscPemGenerator(obj));
			}
			catch (PemGenerationException e)
			{
				if (e.InnerException is IOException)
					throw (IOException)e.InnerException;

				throw e;
			}
		}

		public void WriteObject(
			object			obj,
			string			algorithm,
			char[]			password,
			SecureRandom	random)
		{
			base.WriteObject(new MiscPemGenerator(obj, algorithm, password, random));
		}
	}
}
