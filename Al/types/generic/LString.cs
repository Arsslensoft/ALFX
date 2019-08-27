using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Represents an string list
/// </summary>
public struct LString
{
    List<string> vals;
    public List<string> Value
    {
        get
        {
            return vals;
        }
    }
    public LString(int length=0)
    {
        vals = new List<string>();
    }

    public static LString operator +(LString a, string b)
    {
        a.vals.Add(b);
        return a;
    }
    public static LString operator -(LString a, string b)
    {
        a.vals.Remove(b);
        return a;
    }
    public static LString operator ++(LString a)
    {
        a.vals.Sort();
        return a;
    }
    public static LString operator --(LString a)
    {
        a.vals.Reverse();
        return a;
    }
    public static LString operator !(LString a)
    {
        a.vals.Clear();
        return a;
    }
    public static bool operator *(LString a, string b)
    {

        return a.vals.Contains(b);
    }

    public string this[int index]
    {
        get { return this.vals[index]; }
        set { this.vals[index] = value; }
    }

    public static implicit operator LString(string a)
    {
        LString b = new LString(0);
        b = b + a;
        return b;
    }
    public static implicit operator LString(string [] a)
    {
        LString b = new LString(0);
        b.vals.AddRange(a);
        return b;
    }
    public static implicit operator string[](LString a)
    {

        return a.vals.ToArray();
    }
}

