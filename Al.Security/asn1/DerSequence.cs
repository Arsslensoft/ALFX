using System.Collections;
using System.IO;

namespace Al.Security.Asn1
{
	public class DerSequence
		: Asn1Sequence
	{
		public static readonly DerSequence Empty = new DerSequence();

		public static DerSequence FromVector(
			Asn1EncodableVector v)
		{
			return v.Count < 1 ? Empty : new DerSequence(v);
		}

		/**
		 * create an empty sequence
		 */
		public DerSequence()
			: base(0)
		{
		}

		/**
		 * create a sequence containing one object
		 */
		public DerSequence(
			Asn1Encodable obj)
			: base(1)
		{
			AddObject(obj);
		}

		public DerSequence(
			params Asn1Encodable[] v)
			: base(v.Length)
		{
			foreach (Asn1Encodable ae in v)
			{
				AddObject(ae);
			}
		}

		/**
		 * create a sequence containing a vector of objects.
		 */
		public DerSequence(
			Asn1EncodableVector v)
			: base(v.Count)
		{
			foreach (Asn1Encodable ae in v)
			{
				AddObject(ae);
			}
		}

		/*
		 * A note on the implementation:
		 * <p>
		 * As Der requires the constructed, definite-length model to
		 * be used for structured types, this varies slightly from the
		 * ASN.1 descriptions given. Rather than just outputing Sequence,
		 * we also have to specify Constructed, and the objects length.
		 */
		internal override void Encode(
			DerOutputStream derOut)
		{
			// TODO Intermediate buffer could be avoided if we could calculate expected length
			MemoryStream bOut = new MemoryStream();
			DerOutputStream dOut = new DerOutputStream(bOut);

			foreach (Asn1Encodable obj in this)
			{
				dOut.WriteObject(obj);
			}

			dOut.Close();

			byte[] bytes = bOut.ToArray();

			derOut.WriteEncoded(Asn1Tags.Sequence | Asn1Tags.Constructed, bytes);
		}
	}

    public class DerSequenceOf
    : Asn1Sequence
    {
        public static readonly DerSequenceOf Empty = new DerSequenceOf();

        public static DerSequenceOf FromVector(
            Asn1EncodableVector v)
        {
            return v.Count < 1 ? Empty : new DerSequenceOf(v);
        }

        /**
         * create an empty sequence
         */
        public DerSequenceOf()
            : base(0)
        {
        }

        /**
         * create a sequence containing one object
         */
        public DerSequenceOf(
            Asn1Encodable obj)
            : base(1)
        {
            AddObject(obj);
        }

        public DerSequenceOf(
            params Asn1Encodable[] v)
            : base(v.Length)
        {
            foreach (Asn1Encodable ae in v)
            {
                AddObject(ae);
            }
        }

        /**
         * create a sequence containing a vector of objects.
         */
        public DerSequenceOf(
            Asn1EncodableVector v)
            : base(v.Count)
        {
            foreach (Asn1Encodable ae in v)
            {
                AddObject(ae);
            }
        }

        /*
         * A note on the implementation:
         * <p>
         * As Der requires the constructed, definite-length model to
         * be used for structured types, this varies slightly from the
         * ASN.1 descriptions given. Rather than just outputing Sequence,
         * we also have to specify Constructed, and the objects length.
         */
        internal override void Encode(
            DerOutputStream derOut)
        {
            // TODO Intermediate buffer could be avoided if we could calculate expected length
            MemoryStream bOut = new MemoryStream();
            DerOutputStream dOut = new DerOutputStream(bOut);

            foreach (Asn1Encodable obj in this)
            {
                dOut.WriteObject(obj);
            }

            dOut.Close();

            byte[] bytes = bOut.ToArray();

            derOut.WriteEncoded(Asn1Tags.SequenceOf , bytes);
        }
    }

}
