using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>
    /// Represents an structured bloc
    /// </summary>
public struct usbloc
{
    private byte[] data;
    private long sindex;
    /// <summary>
    /// Creates a new structured bloc
    /// </summary>
    /// <param name="byteArray">the base byte array</param>
    public usbloc(byte[] byteArray)
    {
        if (byteArray == null)
            throw new ArgumentNullException("byteArray","Structured bloc could not be null");
        data = byteArray;
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
    public byte[] Value
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
    /// Returns a byte by the given index
    /// </summary>
    /// <param name="index">index</param>
    /// <returns>byte value</returns>
    public byte this[long index]
    {
        get
        {
            return data[index];
        }
    }

    #region Byte Utils

    byte[] Extract(byte[] data, long offset, long length)
    {
        if (data.LongLength > length && offset >= 0 && offset + length <= data.LongLength)
        {
            byte[] d = new byte[length];
            for (long i = offset; i < offset + length; i++)
                d[i - offset] = data[i];

            return d;
        }
        else
            return null;
    }
    byte[] Append(byte[] data, byte[] arg)
    {
        byte[] a = new byte[data.Length + arg.Length];
        for (int i = 0; i < data.Length; i++)
            a[i] = data[i];

        for (int j = 0; j < arg.Length; j++)
            a[data.Length + j] = arg[j];

        return a;
    }
    uint ComputeCRC32(byte[] data)
    {
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
    bool Match(byte[] x, byte[] y)
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
    long FindFirst(byte[] data, byte[] pattern)
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
    /// Appends a structured bloc to another
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, usbloc b)
    {
        byte[] append = a.Append(a.data, b.data);
        return (usbloc)append;
        
    }
    /// <summary>
    /// Appends a byte array to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, byte[] b)
    {
        byte[] append = a.Append(a.data, b);
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a integer to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, int b)
    {
        byte[] append = a.Append(a.data, BitConverter.GetBytes(b));
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a long integer to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, long b)
    {
        byte[] append = a.Append(a.data, BitConverter.GetBytes(b));
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a byte to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, char b)
    {
        byte[] append = a.Append(a.data, BitConverter.GetBytes(b));
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a string to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, string b)
    {
        byte[] append = a.Append(a.data, Encoding.UTF8.GetBytes(b));
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a double to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, double b)
    {
        byte[] append = a.Append(a.data, BitConverter.GetBytes(b));
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a byte to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, byte b)
    {
        byte[] append = a.Append(a.data, new byte[1] {b});
        return (usbloc)append;

    }
    /// <summary>
    /// Appends a float to a structured bloc
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">B value</param>
    /// <returns>Structured bloc</returns>
    public static usbloc operator +(usbloc a, float b)
    {
        byte[] append = a.Append(a.data, BitConverter.GetBytes(b));
        return (usbloc)append;

    }

    /// <summary>
    /// Deletes a byte array from a position
    /// </summary>
    /// <param name="a">structured bloc</param>
    /// <param name="pos">value position</param>
    /// <returns></returns>
    public static usbloc operator -(usbloc a, long pos)
    {
        if (pos < a.LongLength)
        {
            byte[] x = a.Extract(a.data, 0,pos);
            byte[] y = a.Extract(a.data, pos+1, a.LongLength-pos-1);
            return (usbloc)(a.Append(x,y));
        }
        else return a;
    }
    /// <summary>
    /// Removes a structured bloc b from bloc a
    /// </summary>
    /// <param name="a">structured bloc</param>
    /// <param name="b">structured bloc</param>
    /// <returns></returns>
    public static usbloc operator -(usbloc a, usbloc b)
    {
        if (b.LongLength < a.LongLength)
        {
         
            long f = a.FindFirst(a.data, b.data);
            if (f == -1)
                return a;
            else
            {
                usbloc r = (usbloc)a;
                while (f != -1)
                {
                    byte[] x = r.Extract(a.data, 0, f);
                    if (f + b.LongLength >= r.LongLength)
                        r = (usbloc)x;
                    else
                    {
                        byte[] y = r.Extract(a.data, f + b.LongLength, a.LongLength - (f + b.LongLength));
                        r = (usbloc)(r.Append(x, y));
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
    public static usbloc operator ++(usbloc a)
    {
        Array.Sort(a.data);
        return (usbloc)a.data;
    }
    /// <summary>
    /// Descending sort
    /// </summary>
    /// <param name="a">Structured bloc</param>
    /// <returns>descending sorted bloc</returns>
    public static usbloc operator --(usbloc a)
    {
        Array.Reverse(a.data);
        return (usbloc)a.data;
    }


    /// <summary>
    /// Returns a value from the current position
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>structured bloc</returns>
    public static usbloc operator *(usbloc a, usbloc b)
    {
        if (b.StartIndex < a.LongLength)
        {
            byte[] res = a.Extract(a.data, b.StartIndex, b.LongLength);
            if (res == null)
                return b;
            else return (usbloc)res;
        }
        else return b;
    }

    /// <summary>
    /// Split the bloc from the current position
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>2 structured blocs</returns>
    public static usbloc[] operator /(usbloc a, long pos)
    {
        if (pos < a.LongLength)
        {
            byte[] x = a.Extract(a.data, 0, pos);
            byte[] y = a.Extract(a.data, pos, a.LongLength - pos);
            return new usbloc[2] {(usbloc)x, (usbloc)y };
        }
        else return  new usbloc[1] {a };
    }

    public override string ToString()
    {
        
        return Encoding.UTF8.GetString(data);
    }


    public static implicit operator usbloc(byte[] byteArray)
    {
        return new usbloc(byteArray);
    }
    public static implicit operator usbloc(long val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator usbloc(int val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator usbloc(short val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator usbloc(double val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator usbloc(float val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator usbloc(char val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }
    public static implicit operator usbloc(byte val)
    {
        return new usbloc(BitConverter.GetBytes(val));
    }


    public static implicit operator byte[](usbloc bloc)
    {
        return bloc.data;
    }
    public static implicit operator int(usbloc bloc)
    {
        return BitConverter.ToInt32(bloc.data,0);
    }
    public static implicit operator long(usbloc bloc)
    {
        return BitConverter.ToInt64(bloc.data, 0);
    }
    public static implicit operator short(usbloc bloc)
    {
        return BitConverter.ToInt16(bloc.data, 0);
    }
    public static implicit operator double(usbloc bloc)
    {
        return BitConverter.ToDouble(bloc.data, 0);
    }
    public static implicit operator char(usbloc bloc)
    {
        return BitConverter.ToChar(bloc.data, 0);
    }
    public static implicit operator byte(usbloc bloc)
    {
        return bloc [0];
    }
    public static implicit operator float(usbloc bloc)
    {
        return BitConverter.ToSingle(bloc.data, 0);
    }
    public static implicit operator string(usbloc bloc)
    {
        return Encoding.UTF8.GetString(bloc.data);
    }
    public static implicit operator int[](usbloc bloc)
    {
        List<int> arr = new List<int>();

      for(int i =0; i < bloc.LongLength; i+=4)
          if(i+4 <= bloc.LongLength)
             arr.Add(BitConverter.ToInt32(bloc.data, i));

      return arr.ToArray();
    }




    /// <summary>
    /// Compares a with b
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>structured bloc</returns>
    public static bool operator ==(usbloc a, usbloc b)
    {
        return a.Match(a.data, b.data);
    }
    /// <summary>
    /// Compares a with b
    /// </summary>
    /// <param name="a">A value</param>
    /// <param name="b">Position index</param>
    /// <returns>structured bloc</returns>
    public static bool operator !=(usbloc a, usbloc b)
    {
        return !a.Match(a.data, b.data);
    }
}