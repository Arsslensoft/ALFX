using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


    public struct strfile
    {
        string filename;
        public strfile(string file)
        {
            filename = file;
        }

        public static implicit operator strfile(string fi)
        {
            return new strfile(fi);
        }
        public static implicit operator string(strfile fi)
        {
            return fi.filename;
        }
        public static implicit operator long(strfile fi)
        {
            return new FileInfo(fi.filename).Length;
        }
        public static implicit operator DateTime(strfile fi)
        {
            return new FileInfo(fi.filename).CreationTime;
        }

        public static bool operator !(strfile a)
        {
            return File.Exists(a.filename);
        }
        public static bool operator ~(strfile a)
        {
            if (!a)
            {

                File.Delete(a.filename);
                return true;
            }
            else
                return false;
        }


        public static bool operator +(strfile a,string line)
        {
            File.AppendAllText(a.filename, line);
            return true;
        }

        public string GetAllText()
        {
            return File.ReadAllText(filename);
        }
        public string[] GetAllLines()
        {
            return File.ReadAllLines(filename);
        }
        public void SetAllText(string txt)
        {
            File.WriteAllText(filename, txt);
        }
        public void SetAllLines(string[] txt)
        {
            File.WriteAllLines(filename, txt);
        }


    }

