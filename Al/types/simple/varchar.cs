using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Represents a varchar type
/// </summary>
public struct varchar
{
    char[] strval;
    public varchar(int val)
    {
        strval = new char[val];
    }
    public varchar(string value)
    {
        strval = new char[value.Length];
        for (int i = 0; i < value.Length; i++)
            strval[i] = value[i];
    }

    public override string ToString()
    {
        return this;
    }

    public static implicit operator string(varchar val)
    {
        string x = "";
        foreach (char c in val.strval)
            x += c;
        return x;
    }
    public static implicit operator varchar(string val)
    {
        return new varchar(val);
    }

    public static bool operator ==(varchar a, varchar b)
    {
        return ((string)a == (string)b);
    }
    public static bool operator !=(varchar a, varchar b)
    {
          return ((string)a != (string)b);
    }
}