using System.IO;

using Al.Security.Asn1.Utilities;

namespace Al.Security.Bcpg.OpenPgp
{
	public class WrappedGeneratorStream
		: FilterStream
	{
		private readonly IStreamGenerator gen;

		public WrappedGeneratorStream(
			IStreamGenerator	gen,
			Stream				str)
			: base(str)
		{
			this.gen = gen;
		}

		public override void Close()
		{
			gen.Close();
		}
	}
}
