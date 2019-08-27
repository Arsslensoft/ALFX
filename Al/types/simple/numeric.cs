using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>
    /// Represents a numeric type
    /// </summary>
    public struct numeric
    {
        string strval;
        public numeric(string val)
        {
            bool v = true;
            for (int i = 0; i < val.Length; i++)
                if (!char.IsDigit(val[i]))
                    v = false;
            

            if (v)
                strval = val;
            else
                throw new ArgumentException("value should contain only digits");

        }

        public override string ToString()
        {
            return strval;
        }
        public static implicit operator numeric(uint val)
        {
            return new numeric(val.ToString());
        }
        public static implicit operator numeric(ushort val)
        {
            return new numeric(val.ToString());
        }
        public static implicit operator numeric(ulong val)
        {
            return new numeric(val.ToString());
        }
      
        public static implicit operator numeric(string val)
        {
            return new numeric(val);
        }
        public static implicit operator string(numeric val)
        {
            return val.strval;
        }
        
        public static implicit operator ulong(numeric val)
        {
            return ulong.Parse(val.strval);
        }

    }

