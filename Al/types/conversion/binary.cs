using System;
using System.Collections.Generic;
using System.Text;

public struct binary
{
    string bin;
    public binary(string val)
    {
        bin = val;
    }

    public override string ToString()
    {
        return bin;
    }

    public static implicit operator binary(int val)
    {
        return (uint)val;
    }
    public static implicit operator binary(short val)
    {
        return (ushort)val;
    }
    public static implicit operator binary(long val)
    {
        return (ulong)val;
    }
    public static implicit operator binary(sbyte val)
    {
        return (byte)val;
    }

    public static implicit operator binary(byte val)
    {
        string h = Convert.ToString(val, 2);
        return new binary(h);
    }
    public static implicit operator binary(ushort val)
    {
        string h = Convert.ToString(val, 2);
        return new binary(h);
    }
    public static implicit operator binary(uint val)
    {
        string h = Convert.ToString(val, 2);
        return new binary(h);
    }
    public static implicit operator binary(ulong val)
    {
        string h = Convert.ToString((long)val, 2);
        return new binary(h);
    }
    public static implicit operator binary(double val)
    {
        return (ulong)BitConverter.DoubleToInt64Bits(val);
    }


    public static implicit operator uint(binary val)
    {
        return Convert.ToUInt32(val.bin, 2);
    }
    public static implicit operator ushort(binary val)
    {
        return Convert.ToUInt16(val.bin, 2);
    }
    public static implicit operator ulong(binary val)
    {
        return Convert.ToUInt64(val.bin, 2);
    }
    public static implicit operator int(binary val)
    {
        return Convert.ToInt32(val.bin, 2);
    }
    public static implicit operator short(binary val)
    {
        return Convert.ToInt16(val.bin, 2);
    }
    public static implicit operator long(binary val)
    {
        return Convert.ToInt64(val.bin, 2);
    }
    public static implicit operator byte(binary val)
    {
        return Convert.ToByte(val.bin, 2);
    }
    public static implicit operator sbyte(binary val)
    {
        return Convert.ToSByte(val.bin, 2);
    }
    public static implicit operator double(binary val)
    {
        return BitConverter.Int64BitsToDouble(Convert.ToInt64(val.bin, 2));
    }

    public static bool operator ==(binary a, binary b)
    {
        return (a.bin == b.bin);
    }
    public static bool operator !=(binary a, binary b)
    {
        return (a.bin != b.bin);
    }
}