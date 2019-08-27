using System;
using System.Collections.Generic;
using System.Text;


    /// <summary>
    /// Represents an unsigned encryption bloc
    /// </summary>
    public struct uebloc
    {
       private byte[] data;

        /// <summary>
        /// Creates a new encryption bloc
        /// </summary>
        /// <param name="byteArray">the base byte array</param>
        public uebloc(byte[] byteArray)
        {
            data = byteArray;
        }

        /// <summary>
        /// Returns the long length of the encryption bloc
        /// </summary>
        public long LongLength
        {
            get
            {
                return data.LongLength;
            }
        }
        /// <summary>
        /// Returns the length of the encryption bloc
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

        /// <summary>
        /// Performs an add encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static uebloc operator +(uebloc a, uebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            byte[] result = new byte[max];
              if (bisbig)
                {
                    for (long i = 0; i < max; i++)
                    {
                        if (i < a.data.LongLength)
                            result[i] = (byte)(a.data[i] + b.data[i]);
                        else
                            result[i] = b.data[i];
                    }
                }
                else
              {
                  for (long i = 0; i < max; i++)
                  {
                      if (i < b.data.LongLength)
                          result[i] = (byte)(a.data[i] + b.data[i]);
                      else
                          result[i] = a.data[i];
                  }

                }
              return (uebloc)result;
        }
        /// <summary>
        /// Performs an xor encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static uebloc operator ^(uebloc a, uebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            byte[] result = new byte[max];
            if (bisbig)
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < a.data.LongLength)
                        result[i] = (byte)(a.data[i] ^ b.data[i]);
                    else
                        result[i] = b.data[i];
                }
            }
            else
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < b.data.LongLength)
                        result[i] = (byte)(a.data[i] ^ b.data[i]);
                    else
                        result[i] = a.data[i];
                }

            }
            return (uebloc)result;
        }
        /// <summary>
        /// Performs an sub encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static uebloc operator -(uebloc a, uebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            byte[] result = new byte[max];
            if (bisbig)
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < a.data.LongLength)
                        result[i] = (byte)(a.data[i] - b.data[i]);
                    else
                        result[i] = b.data[i];
                }
            }
            else
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < b.data.LongLength)
                        result[i] = (byte)(a.data[i] - b.data[i]);
                    else
                        result[i] = a.data[i];
                }

            }
            return (uebloc)result;
        }
        /// <summary>
        /// Performs an XNOR encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static uebloc operator *(uebloc a, uebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            byte[] result = new byte[max];
            if (bisbig)
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < a.data.LongLength)
                        result[i] = (byte)(~(a.data[i] ^ b.data[i]));
                    else
                        result[i] = b.data[i];
                }
            }
            else
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < b.data.LongLength)
                        result[i] = (byte)(~(a.data[i] ^ b.data[i]));
                    else
                        result[i] = a.data[i];
                }

            }
            return (uebloc)result;
        }

        /// <summary>
        /// Casts a byte array to uebloc
        /// </summary>
        /// <param name="byteArray">Data array</param>
        /// <returns>Encryption bloc</returns>
        public static implicit operator uebloc(byte[] byteArray)
        {
            return new uebloc(byteArray);
        }

        /// <summary>
        /// Casts an encryption bloc to a byte array
        /// </summary>
        /// <param name="bloc">Encryption bloc</param>
        /// <returns>Byte array</returns>
        public static implicit operator byte[](uebloc bloc)
        {
            return bloc.data;
        }

        public static implicit operator uebloc(usbloc sbyteArray)
        {
            return new uebloc(sbyteArray.Value);
        }
        public static implicit operator usbloc(uebloc bloc)
        {
            return bloc.data;
        }
    }

