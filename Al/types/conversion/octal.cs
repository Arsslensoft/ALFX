using System;
using System.Collections.Generic;
using System.Text;

public struct octal
{
    string octa;
    public octal(string val)
    {
        octa = val;
    }

    public override string ToString()
    {
        return octa;
    }

    public static implicit operator octal(int val)
    {
        return (uint)val;
    }
    public static implicit operator octal(short val)
    {
        return (ushort)val;
    }
    public static implicit operator octal(long val)
    {
        return (ulong)val;
    }
    public static implicit operator octal(sbyte val)
    {
        return (byte)val;
    }

    public static implicit operator octal(byte val)
    {
        string h =  Convert.ToString(val,8);
        return new octal(h);
    }
    public static implicit operator octal(ushort val)
    {
        string h = Convert.ToString(val, 8);
        return new octal(h);
    }
    public static implicit operator octal(uint val)
    {
        string h = Convert.ToString(val, 8);
        return new octal(h);
    }
    public static implicit operator octal(ulong val)
    {
        string h = Convert.ToString((long)val, 8);
        return new octal(h);
    }
    public static implicit operator octal(double val)
    {
        return (ulong)BitConverter.DoubleToInt64Bits(val);
    }


    public static implicit operator uint(octal val)
    {
        return Convert.ToUInt32(val.octa, 8);
    }
    public static implicit operator ushort(octal val)
    {
        return Convert.ToUInt16(val.octa, 8);
    }
    public static implicit operator ulong(octal val)
    {
        return Convert.ToUInt64(val.octa, 8);
    }
    public static implicit operator int(octal val)
    {
        return Convert.ToInt32(val.octa, 8);
    }
    public static implicit operator short(octal val)
    {
        return Convert.ToInt16(val.octa, 8);
    }
    public static implicit operator long(octal val)
    {
        return Convert.ToInt64(val.octa, 8);
    }
    public static implicit operator byte(octal val)
    {
        return Convert.ToByte(val.octa, 8);
    }
    public static implicit operator sbyte(octal val)
    {
        return Convert.ToSByte(val.octa, 8);
    }
    public static implicit operator double(octal val)
    {
        return BitConverter.Int64BitsToDouble((long)Convert.ToUInt64(val.octa, 8));
    }
    public static bool operator ==(octal a, octal b)
    {
        return (a.octa == b.octa);
    }
    public static bool operator !=(octal a, octal b)
    {
        return (a.octa != b.octa);
    }
}