using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>
    /// Represents an integer list
    /// </summary>
    public struct LInteger
    {
        List<int> vals;
        public List<int> Value
        {
            get
            {
                return vals;
            }
        }
        public LInteger(int length = 0)
        {
            vals = new List<int>();
        }

        public static LInteger operator +(LInteger a, int b)
        {
            a.vals.Add(b);
            return a;
        }
        public static LInteger operator -(LInteger a, int b)
        {
            a.vals.Remove(b);
            return a;
        }
        public static LInteger operator ++(LInteger a)
        {
            a.vals.Sort();
            return a;
        }
        public static LInteger operator --(LInteger a)
        {
            a.vals.Reverse();
            return a;
        }
        public static LInteger operator !(LInteger a)
        {
            a.vals.Clear();
            return a;
        }
        public static bool operator *(LInteger a, int b)
        {
     
            return a.vals.Contains(b);
        }

        public int this[int index]
        {
            get { return this.vals[index]; }
            set { this.vals[index] = value; }
        }

        public static implicit operator LInteger(int a)
        {
            LInteger b = new LInteger(0);
            b = b + a;
            return b;
        }
        public static implicit operator LInteger(int [] a)
        {
            LInteger b = new LInteger(0);
            b.vals.AddRange(a);
            return b;
        }
        public static implicit operator int[](LInteger a)
        {
          
            return a.vals.ToArray();
        }

    }

