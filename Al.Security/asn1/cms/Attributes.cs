using System;

namespace Al.Security.Asn1.Cms
{
	public class Attributes
		: Asn1Encodable
	{
		private readonly Asn1Set attributes;

		private Attributes(Asn1Set attributes)
		{
			this.attributes = attributes;
		}

		public Attributes(Asn1EncodableVector v)
		{
			attributes = new BerSet(v);
		}

		public static Attributes GetInstance(object obj)
		{
			if (obj is Attributes)
				return (Attributes)obj;

			if (obj != null)
				return new Attributes(Asn1Set.GetInstance(obj));

			return null;
		}

		/**
		 * <pre>
		 * Attributes ::=
		 *   SET SIZE(1..MAX) OF Attribute -- according to RFC 5652
		 * </pre>
		 * @return
		 */
		public override Asn1Object ToAsn1Object()
		{
			return attributes;
		}
	}
}
