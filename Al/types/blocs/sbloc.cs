using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>
    /// Represents an structured bloc
    /// </summary>
public struct sbloc
{
    private sbyte[] data;
    private long sindex;
    /// <summary>
    /// Creates a new structured bloc
    /// </summary>
    /// <param name="sbyteArray">the base sbyte array</param>
    public sbloc(sbyte[] sbyteArray)
    {
        if (sbyteArray == null)
            throw new ArgumentNullException("sbyteArray","Structured bloc could not be null");
        data = sbyteArray;
       sindex = 0;
    }
    public sbloc(byte[] sbyteArray)
    {
        if (sbyteArray == null)
            throw new ArgumentNullException("sbyteArray", "Structured bloc could not be null");
      
        
        sbyte[] res = new sbyte[sbyteArray.LongLength];
        for (long i = 0; i < sbyteArray.LongLength; i++)
            res[i] = (sbyte)sbyteArray[i];

        data = res;
        sindex = 0;
    }
    /// <summary>
    /// Used to extract a structured bloc from another
    /// </summary>
    public long StartIndex
    {

        get { return sindex; }
        set { sindex = value; }
    }
    /// <summary>
    /// Gets the value of the structured bloc
    /// </summary>
    public sbyte[] Value
    {
        get
        {
            return data;
        }
    }

    /// <summary>
    /// Returns the long length of the structured bloc
    /// </summary>
    public long LongLength
    {
        get
        {
            return data.LongLength;
        }
    }
    /// <summary>
    /// Returns the length of the structured bloc
    /// </summary>
    public int Length
    {
        get
        {
            return data.Length;
        }
    }

    /// <summary>
    /// Returns a sbyte by the given index
    /// </summary>
    /// <param name="index">index</param>
    /// <returns>sbyte value</returns>
    public sbyte this[long index]
    {
        get
        {
            return data[index];
        }
    }

    #region sbyte Utils
    byte[] Recast(sbyte[] dat)
    {
        byte[] res = new byte[dat.LongLength];
        for (long i = 0; i < dat.LongLength; i++)
            res[i] = (byte)dat[i];
        return res;
    }
    sbyte[] Cast(byte[] dat)
    {
        sbyte[] res = new sbyte[dat.LongLength];
        for (long i = 0; i < dat.LongLength; i++)
            res[i] = (sbyte)dat[i];
        return res;
    }
    sbyte[] Sym(sbyte[] dat)
    {
        sbyte[] res = new sbyte[dat.LongLength];
        for (long i = 0; i < dat.LongLength; i++)
            res[i] = (sbyte)(-dat[i]);
        return res;
    }
    sbyte[] Extract(sbyte[] data, long offset, long length)
    {
        if (data.LongLength > length && offset >= 0 && offset + length <= data.LongLength)
        {
            sbyte[] d = new sbyte[length];
            for (long i = offset; i < offset + length; i++)
                d[i - offset] = data[i];

            return d;
        }
        else
            return null;
    }
    sbyte[] Append(sbyte[] data, sbyte[] arg)
    {
        sbyte[] a = new sbyte[data.Length + arg.Length];
        for (int i = 0; i < data.Length; i++)
            a[i] = data[i];

        for (int j = 0; j < arg.Length; j++)
            a[data.Length + j] = arg[j];

        return a;
    }
    uint ComputeCRC32(sbyte[] dat)
    {
        byte[] data = Recast(dat);
        uint crc = 0xFFFFFFFF;       // initial contents of LFBSR
        uint poly = 0xEDB88320;   // reverse polynomial
        for (int i = 0; i < data.Length; i++)
        {
            uint temp = (crc ^ data[i]) & 0xff;

            // read 8 bits one at a time
            for (int j = 0; j < 8; j++)
            {
                if ((temp & 1) == 1) temp = (temp >> 1) ^ poly;
                else temp = (temp >> 1);
            }
            crc = (crc >> 8) ^ temp;
        }

        // flip bits
        crc = crc ^ 0xffffffff;

        return crc;
    }
    bool Match(sbyte[] x, sbyte[] y)
    {
        if (x.LongLength != y.LongLength)
            return false;
        else if (x == null || y == null)
            return false;
        else
        {
            bool m = true;
            for (long i = 0; i < x.LongLength; i++)
                if (x[i] != y[i])
                {
                    m = false;
                    break;
                }
            return m;
        }
    }
    long FindFirst(sbyte[] data, sbyte[] pattern)
    {
        if (data.LongLength < pattern.LongLength)
            return -1;
        else
        {
       
            for (long i = 0; i < data.LongLength; i++)
            {
                if (data[i] == pattern[0])
                {
                    // start matching bloc
                        if (Match(Extract(data, i, pattern.LongLength), pattern))
                            return i;
                }
            }
            return -1;
        }

    }

    #endregion

    /// <summary>
    /// Calculates the crc32
    /// </summary>
    public uint Hash
    {
        get
        {
            return ComputeCRC32(data);
        }
    }

    public bool IsValid(uint hash)
    {
        return (ComputeCRC32(data) == hash);
    }

    /// <summary>
    /// Appends a structured bloc to another
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, sbloc b)
    {
        sbyte[] append = a.Append(a.data, b.data);
        return (sbloc)append;
        
    }
    /// <summary>
    /// Appends a sbyte array to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, sbyte[] b)
    {
        sbyte[] append = a.Append(a.data, b);
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a integer to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, int b)
    {
        sbyte[] append = a.Append(a.data, a.Cast(BitConverter.GetBytes(b)));
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a long integer to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, long b)
    {
        sbyte[] append = a.Append(a.data, a.Cast(BitConverter.GetBytes(b)));
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a sbyte to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, char b)
    {
        sbyte[] append = a.Append(a.data, a.Cast(BitConverter.GetBytes(b)));
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a string to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, string b)
    {
        sbyte[] append = a.Append(a.data, a.Cast(Encoding.UTF8.GetBytes(b)));
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a double to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, double b)
    {
        sbyte[] append = a.Append(a.data,a.Cast( BitConverter.GetBytes(b)));
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a float to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, float b)
    {
        sbyte[] append = a.Append(a.data,a.Cast( BitConverter.GetBytes(b)));
        return (sbloc)append;

    }
    /// <summary>
    /// Appends a byte to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static sbloc operator +(sbloc a, sbyte b)
    {
        sbyte[] append = a.Append(a.data, new sbyte[1] { b });
        return (sbloc)append;

    }

    /// <summary>
    /// Deletes a sbyte array from a position
    /// </summary>
    /// <param name="a">structured bloc</param>
    /// <param name="pos">value position</param>
    /// <returns></returns>
    public static sbloc operator -(sbloc a, long pos)
    {
        if (pos < a.LongLength)
        {
            sbyte[] x = a.Extract(a.data, 0,pos);
            sbyte[] y = a.Extract(a.data, pos+1, a.LongLength-pos-1);
            return (sbloc)(a.Append(x,y));
        }
        else return a;
    }
    /// <summary>
    /// Removes a structured bloc b from bloc a
    /// </summary>
    /// <param name="a">structured bloc</param>
    /// <param name="b">structured bloc</param>
    /// <returns></returns>
    public static sbloc operator -(sbloc a, sbloc b)
    {
        if (b.LongLength < a.LongLength)
        {
         
            long f = a.FindFirst(a.data, b.data);
            if (f == -1)
                return a;
            else
            {
                sbloc r = (sbloc)a;
                while (f != -1)
                {
                    sbyte[] x = r.Extract(a.data, 0, f);
                    if (f + b.LongLength >= r.LongLength)
                        r = (sbloc)x;
                    else
                    {
                        sbyte[] y = r.Extract(a.data, f + b.LongLength, a.LongLength - (f + b.LongLength));
                        r = (sbloc)(r.Append(x, y));
                    }
                    f = r.FindFirst(r.data, b.data);
                }
                return r;
            }

        }
        else return a;
    }

    /// <summary>
    /// Ascending sort
    /// </summary>
    /// <param name="a">Structured bloc</param>
    /// <returns>ascending sorted bloc</returns>
    public static sbloc operator ++(sbloc a)
    {
        Array.Sort(a.data);
        return (sbloc)a.data;
    }
    /// <summary>
    /// Descending sort
    /// </summary>
    /// <param name="a">Structured bloc</param>
    /// <returns>descending sorted bloc</returns>
    public static sbloc operator --(sbloc a)
    {
        Array.Reverse(a.data);
        return (sbloc)a.data;
    }
    /// <summary>
    /// Inverse value
    /// </summary>
    /// <param name="a">Structured bloc</param>
    /// <returns>descending sorted bloc</returns>
    public static sbloc operator !(sbloc a)
    {
       
        return (sbloc)a.Sym(a.data);
    }

    /// <summary>
    /// Returns a value from the current position
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>structured bloc</returns>
    public static sbloc operator *(sbloc a, sbloc b)
    {
        if (b.StartIndex < a.LongLength)
        {
            sbyte[] res = a.Extract(a.data, b.StartIndex, b.LongLength);
            if (res == null)
                return b;
            else return (sbloc)res;
        }
        else return b;
    }

    /// <summary>
    /// Split the bloc from the current position
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>2 structured blocs</returns>
    public static sbloc[] operator /(sbloc a, long pos)
    {
        if (pos < a.LongLength)
        {
            sbyte[] x = a.Extract(a.data, 0, pos);
            sbyte[] y = a.Extract(a.data, pos, a.LongLength - pos);
            return new sbloc[2] {(sbloc)x, (sbloc)y };
        }
        else return  new sbloc[1] {a };
    }

    public override string ToString()
    {

        return Encoding.UTF8.GetString(Recast(data));
    }

    public static implicit operator sbloc(byte[] byteArray)
    {
        return new sbloc(byteArray);
    }
    public static implicit operator sbloc(sbyte[] sbyteArray)
    {
        return new sbloc(sbyteArray);
    }
    public static implicit operator sbloc(long val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator sbloc(int val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator sbloc(short val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator sbloc(double val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator sbloc(float val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator sbloc(char val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator sbloc(sbyte val)
    {
        return new sbloc(BitConverter.GetBytes(val));
    }


    public static implicit operator sbyte[](sbloc bloc)
    {
        return bloc.data;
    }
    public static implicit operator int(sbloc bloc)
    {
        return BitConverter.ToInt32(bloc.Recast(bloc.data),0);
    }
    public static implicit operator long(sbloc bloc)
    {
        return BitConverter.ToInt64(bloc.Recast(bloc.data), 0);
    }
    public static implicit operator short(sbloc bloc)
    {
        return BitConverter.ToInt16(bloc.Recast(bloc.data), 0);
    }
    public static implicit operator double(sbloc bloc)
    {
        return BitConverter.ToDouble(bloc.Recast(bloc.data), 0);
    }
    public static implicit operator char(sbloc bloc)
    {
        return BitConverter.ToChar(bloc.Recast(bloc.data), 0);
    }
    public static implicit operator sbyte(sbloc bloc)
    {
        return bloc [0];
    }
    public static implicit operator float(sbloc bloc)
    {
        return BitConverter.ToSingle(bloc.Recast(bloc.data), 0);
    }
    public static implicit operator string(sbloc bloc)
    {
        return Encoding.UTF8.GetString(bloc.Recast(bloc.data));
    }



    /// <summary>
    /// Compares a with b
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>structured bloc</returns>
    public static bool operator ==(sbloc a, sbloc b)
    {
        return a.Match(a.data, b.data);
    }
    /// <summary>
    /// Compares a with b
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>structured bloc</returns>
    public static bool operator !=(sbloc a, sbloc b)
    {
        return !a.Match(a.data, b.data);
    }
}