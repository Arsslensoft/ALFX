using System;
using System.Collections.Generic;
using System.Text;


   public struct hex
    {
       string hexa;
       public hex(string val)
       {
           hexa = val;
       }

       public override string ToString()
       {
           return "0x"+hexa;
       }
   
       public static implicit operator hex(int val)
       {
           return (uint)val;
       }
       public static implicit operator hex(short val)
       {
           return (ushort)val;
       }
       public static implicit operator hex(long val)
       {
           return (ulong)val;
       }
       public static implicit operator hex(sbyte val)
       {
           return (byte)val;
       }
       public static implicit operator hex(byte val)
       {
           string h = val.ToString("X2");
           return new hex(h);
       }
       public static implicit operator hex(ushort val)
       {
           string h = val.ToString("X2");

           return new hex(h);
       }
       public static implicit operator hex(uint val)
       {
           string h =val.ToString("X2");
                   return new hex(h);
       }
       public static implicit operator hex(ulong val)
       {
         string h =val.ToString("X2");
    
           return new hex(h);
       }
       public static implicit operator hex(double val)
       {
           return (ulong)BitConverter.DoubleToInt64Bits(val);
       }

       public static implicit operator uint(hex val)
       {
           return Convert.ToUInt32(val.hexa, 16);
       }
       public static implicit operator ushort(hex val)
       {
            return Convert.ToUInt16(val.hexa, 16);
       }
       public static implicit operator ulong(hex val)
       {
           return Convert.ToUInt64(val.hexa, 16);
       }
       public static implicit operator int(hex val)
       {
           return Convert.ToInt32(val.hexa, 16);
       }
       public static implicit operator short(hex val)
       {
           return Convert.ToInt16(val.hexa, 16);
       }
       public static implicit operator long(hex val)
       {
           return Convert.ToInt64(val.hexa, 16);
       }
       public static implicit operator byte(hex val)
       {
           return Convert.ToByte(val.hexa, 16);
       }
       public static implicit operator sbyte(hex val)
       {
           return Convert.ToSByte(val.hexa, 16);
       }
       public static implicit operator double(hex val)
       {
           return BitConverter.Int64BitsToDouble((long)Convert.ToUInt64(val.hexa, 16));
       }

       public static bool operator ==(hex a, hex b)
       {
           return (a.hexa == b.hexa);
       }
       public static bool operator !=(hex a, hex b)
       {
           return (a.hexa != b.hexa);
       }
   
   }

