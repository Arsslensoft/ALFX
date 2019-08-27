using System;
    internal class ALCRT
    {
        public static void Write(bool value)
        // warning possible mismatch 
        {
            Console.Write(value);
        }
        public static void Write(char value)
        {
            Console.Write(value);
        }
        public static void Write(char[] buffer)
        {
            Console.Write(buffer);
        }
        public static void Write(decimal value)
        {
            Console.Write(value);
        }
        public static void Write(double value)
        // warning possible mismatch 
        {
            Console.Write(value);
        }
        public static void Write(int value)
        // warning possible mismatch 
        {
            Console.Write(value);
        }
        public static void Write(long value)
        // warning possible mismatch 
        {
            Console.Write(value);
        }
        public static void Write(object value)
        {
            Console.Write(value);
        }
        public static void Write(float value)
        {
            Console.Write(value);
        }
        public static void Write(string value)
        {
            Console.Write(value);
        }

        public static void Write(uint value)
        // warning possible mismatch 
        {
            Console.Write(value);
        }

        public static void Write(ulong value)
        // warning possible mismatch 
        {
            Console.Write(value);
        }

        public static void Write(string format, object arg0)
        {
            Console.Write(format, arg0);
        }

        public static void Write(string format, params object[] arg)
        {
            if (arg == null)
                Console.Write(format);
            else
                Console.Write(format, arg);
        }

        public static void Write(char[] buffer, int index, int count)
        // warning possible mismatch// warning possible mismatch 
        {
            Console.Write(buffer, index, count);
        }

        public static void Write(string format, object arg0, object arg1)
        {
            Console.Write(format, arg0, arg1);
        }

        public static void Write(string format, object arg0, object arg1, object arg2)
        {
            Console.Write(format, arg0, arg1, arg2);
        }


        public static void Writeln()
        {
            Console.WriteLine();
        }

        public static void Writeln(bool value)
        // warning possible mismatch 
        {
            Console.WriteLine(value);
        }

        public static void Writeln(char value)
        {
            Console.WriteLine(value);
        }

        public static void Writeln(char[] buffer)
        {
            Console.WriteLine(buffer);
        }

        public static void Writeln(decimal value)
        {
            Console.WriteLine(value);
        }

        public static void Writeln(double value)
        // warning possible mismatch 
        {
            Console.WriteLine(value);
        }

        public static void Writeln(int value)
        // warning possible mismatch 
        {
            Console.WriteLine(value);
        }

        public static void Writeln(long value)
        // warning possible mismatch 
        {
            Console.WriteLine(value);
        }

        public static void Writeln(object value)
        {
            Console.WriteLine(value);
        }

        public static void Writeln(float value)
        {
            Console.WriteLine(value);
        }

        public static void Writeln(string value)
        {
            Console.WriteLine(value);
        }


        public static void Writeln(uint value)
        // warning possible mismatch 
        {
            Console.WriteLine(value);
        }


        public static void Writeln(ulong value)
        // warning possible mismatch 
        {
            Console.WriteLine(value);
        }

        public static void Writeln(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }

        public static void Writeln(string format, params object[] arg)
        {
            if (arg == null)
                Console.WriteLine(format);
            else
                Console.WriteLine(format, arg);
        }

        public static void Writeln(char[] buffer, int index, int count)
        // warning possible mismatch// warning possible mismatch 
        {
            Console.WriteLine(buffer, index, count);
        }

        public static void Writeln(string format, object arg0, object arg1)
        {
            Console.WriteLine(format, arg0, arg1);
        }

        public static void Writeln(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(format, arg0, arg1, arg2);
        }
        public static int Read()
        {
            return Console.Read();
        }

        public static void Readln(out string val)
        {
            val = Console.ReadLine();
        }

        public static void Readln(out int val)
        {
            val = int.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out byte val)
        {
            val = byte.Parse(Console.ReadLine());
        }
        public static void Readln(out char val)
        {
            val = char.Parse(Console.ReadLine());
        }
        public static void Readln(out sbyte val)
        {
            val = sbyte.Parse(Console.ReadLine());
        }
        public static void Readln(out bool val)
        {
            val = bool.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out short val)
        {
            val = short.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out long val)
        {
            val = long.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out ulong val)
        {
            val = ulong.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out uint val)
        {
            val = uint.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out double val)
        {
            val = double.Parse(Console.ReadLine());
            // warning possible mismatch 
        }
        public static void Readln(out decimal val)
        {
            val = decimal.Parse(Console.ReadLine());
        }
        public static void Readln(out float val)
        {
            val = float.Parse(Console.ReadLine());
        }
        public static void Beep(int frequency, int duration_ms)
        // warning possible mismatch// warning possible mismatch 
        {
            Console.Beep(frequency, duration_ms);
        }
        public static string CRTWINDOWTEXT
        {
            get { return Console.Title; }
            // warning possible mismatch// warning possible mismatch 
            set { Console.Title = value; }
            // warning possible mismatch 

        }
        public static ConsoleColor CRTWINDOWCOLOR
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }
        public static void Clrscr()
        {
            Console.Clear();
        }
        public static void GotoXY(int x, int y)
        // warning possible mismatch// warning possible mismatch 
        {
            Console.SetCursorPosition(x, y);
        }
    }

