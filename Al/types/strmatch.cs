using System;
using System.Collections.Generic;
using System.Text;


    public struct strmatch
    {
        string text;
        string match;
        public strmatch(string txt)
        {
            text = txt;
            match ="";
        }

        public static implicit operator strmatch(string text)
        {
            return new strmatch(text);
        }
        public static implicit operator string(strmatch a)
        {
            return a.text;
        }
        public static implicit operator int(strmatch a)
        {
            return a.text.IndexOf(a.match);
        }

        public static strmatch operator +(strmatch a, string match)
        {
            a.match = match;
            return a;
        }
        public static strmatch operator -(strmatch a, string match)
        {
            a.match = "";
            return a;
        }

        public static bool operator !(strmatch a)
        {
            return a.text.Contains(a.match);
          
        }

    

    }

