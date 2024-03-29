using System;

namespace Al.Security.Asn1
{
    public class DerBoolean
        : Asn1Object
    {
        private readonly byte value;

		public static readonly DerBoolean False = new DerBoolean(false);
        public static readonly DerBoolean True  = new DerBoolean(true);

		/**
         * return a bool from the passed in object.
         *
         * @exception ArgumentException if the object cannot be converted.
         */
        public static DerBoolean GetInstance(
            object obj)
        {
            if (obj == null || obj is DerBoolean)
            {
                return (DerBoolean) obj;
            }

			throw new ArgumentException("illegal object in GetInstance: " + obj.GetType().Name);
        }

		/**
         * return a DerBoolean from the passed in bool.
         */
        public static DerBoolean GetInstance(
            bool value)
        {
            return value ? True : False;
        }

		/**
         * return a Boolean from a tagged object.
         *
         * @param obj the tagged object holding the object we want
         * @param explicitly true if the object is meant to be explicitly
         *              tagged false otherwise.
         * @exception ArgumentException if the tagged object cannot
         *               be converted.
         */
        public static DerBoolean GetInstance(
            Asn1TaggedObject	obj,
            bool				isExplicit)
        {
			Asn1Object o = obj.GetObject();

			if (isExplicit || o is DerBoolean)
			{
				return GetInstance(o);
			}

			return new DerBoolean(((Asn1OctetString)o).GetOctets());
        }

		public DerBoolean(
            byte[] val)
        {
			if (val.Length != 1)
				throw new ArgumentException("byte value should have 1 byte in it", "val");

			// TODO Are there any constraints on the possible byte values?
            this.value = val[0];
        }

		private DerBoolean(
            bool value)
        {
            this.value = value ? (byte)0xff : (byte)0;
        }

		public bool IsTrue
		{
			get { return value != 0; }
		}

		internal override void Encode(
            DerOutputStream derOut)
        {
			// TODO Should we make sure the byte value is one of '0' or '0xff' here?
			derOut.WriteEncoded(Asn1Tags.Boolean, new byte[]{ value });
        }

		protected override bool Asn1Equals(
			Asn1Object asn1Object)
        {
			DerBoolean other = asn1Object as DerBoolean;

			if (other == null)
				return false;

			return IsTrue == other.IsTrue;
        }

		protected override int Asn1GetHashCode()
		{
			return IsTrue.GetHashCode();
        }

		public override string ToString()
		{
			return IsTrue ? "TRUE" : "FALSE";
		}
	}
}
