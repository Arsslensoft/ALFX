using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Represents an double list
/// </summary>
public struct Ldouble
{
    List<double> vals;
    public List<double> Value
    {
        get
        {
            return vals;
        }
    }
    public Ldouble(int length = 0)
    {
        vals = new List<double>();
    }

    public static Ldouble operator +(Ldouble a, double b)
    {
        a.vals.Add(b);
        return a;
    }
    public static Ldouble operator -(Ldouble a, double b)
    {
        a.vals.Remove(b);
        return a;
    }
    public static Ldouble operator ++(Ldouble a)
    {
        a.vals.Sort();
        return a;
    }
    public static Ldouble operator --(Ldouble a)
    {
        a.vals.Reverse();
        return a;
    }
    public static Ldouble operator !(Ldouble a)
    {
        a.vals.Clear();
        return a;
    }
    public static bool operator *(Ldouble a, double b)
    {

        return a.vals.Contains(b);
    }

    public double this[int index]
    {
        get { return this.vals[index]; }
        set { this.vals[index] = value; }
    }

    public static implicit operator Ldouble(double a)
    {
        Ldouble b = new Ldouble(0);
        b = b + a;
        return b;
    }
    public static implicit operator Ldouble(double[] a)
    {
        Ldouble b = new Ldouble(0);
        b.vals.AddRange(a);
        return b;

    }
    public static implicit operator double[](Ldouble a)
    {

        return a.vals.ToArray();
    }
}

