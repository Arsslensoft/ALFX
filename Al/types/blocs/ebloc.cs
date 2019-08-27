using System;
using System.Collections.Generic;
using System.Text;


    /// <summary>
    /// Represents an signed encryption bloc
    /// </summary>
    public struct ebloc
    {
       private sbyte[] data;

        /// <summary>
        /// Creates a new encryption bloc
        /// </summary>
        /// <param name="sbyteArray">the base sbyte array</param>
        public ebloc(sbyte[] sbyteArray)
        {
            data = sbyteArray;
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

        sbyte[] Sym(sbyte[] dat)
        {
            sbyte[] res = new sbyte[dat.LongLength];
            for (long i = 0; i < dat.LongLength; i++)
                res[i] = (sbyte)(-dat[i]);
            return res;
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

        /// <summary>
        /// Performs an add encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static ebloc operator +(ebloc a, ebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            sbyte[] result = new sbyte[max];
              if (bisbig)
                {
                    for (long i = 0; i < max; i++)
                    {
                        if (i < a.data.LongLength)
                            result[i] = (sbyte)(a.data[i] + b.data[i]);
                        else
                            result[i] = b.data[i];
                    }
                }
                else
              {
                  for (long i = 0; i < max; i++)
                  {
                      if (i < b.data.LongLength)
                          result[i] = (sbyte)(a.data[i] + b.data[i]);
                      else
                          result[i] = a.data[i];
                  }

                }
              return (ebloc)result;
        }
        /// <summary>
        /// Performs an xor encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static ebloc operator ^(ebloc a, ebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            sbyte[] result = new sbyte[max];
            if (bisbig)
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < a.data.LongLength)
                        result[i] = (sbyte)(a.data[i] ^ b.data[i]);
                    else
                        result[i] = b.data[i];
                }
            }
            else
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < b.data.LongLength)
                        result[i] = (sbyte)(a.data[i] ^ b.data[i]);
                    else
                        result[i] = a.data[i];
                }

            }
            return (ebloc)result;
        }
        /// <summary>
        /// Performs an sub encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static ebloc operator -(ebloc a, ebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            sbyte[] result = new sbyte[max];
            if (bisbig)
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < a.data.LongLength)
                        result[i] = (sbyte)(a.data[i] - b.data[i]);
                    else
                        result[i] = b.data[i];
                }
            }
            else
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < b.data.LongLength)
                        result[i] = (sbyte)(a.data[i] - b.data[i]);
                    else
                        result[i] = a.data[i];
                }

            }
            return (ebloc)result;
        }
        /// <summary>
        /// Performs an XNOR encryption operation
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">B value</param>
        /// <returns>Encryption bloc </returns>
        public static ebloc operator *(ebloc a, ebloc b)
        {
            long max = b.data.LongLength;
            bool bisbig = true;
            if (a.data.LongLength > b.data.LongLength)
            {
                max = a.data.LongLength;
                bisbig = false;
            }
            sbyte[] result = new sbyte[max];
            if (bisbig)
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < a.data.LongLength)
                        result[i] = (sbyte)(~(a.data[i] ^ b.data[i]));
                    else
                        result[i] = b.data[i];
                }
            }
            else
            {
                for (long i = 0; i < max; i++)
                {
                    if (i < b.data.LongLength)
                        result[i] = (sbyte)(~(a.data[i] ^ b.data[i]));
                    else
                        result[i] = a.data[i];
                }

            }
            return (ebloc)result;
        }

        /// <summary>
        /// Inverse value
        /// </summary>
        /// <param name="a">Structured bloc</param>
        /// <returns>descending sorted bloc</returns>
        public static ebloc operator !(ebloc a)
        {

            return (ebloc)a.Sym(a.data);
        }

        /// <summary>
        /// Casts a sbyte array to ebloc
        /// </summary>
        /// <param name="sbyteArray">Data array</param>
        /// <returns>Encryption bloc</returns>
        public static implicit operator ebloc(sbyte[] sbyteArray)
        {
            return new ebloc(sbyteArray);
        }
        /// <summary>
        /// Casts an encryption bloc to a sbyte array
        /// </summary>
        /// <param name="bloc">Encryption bloc</param>
        /// <returns>sbyte array</returns>
        public static implicit operator sbyte[](ebloc bloc)
        {
            return bloc.data;
        }



        public static implicit operator ebloc(sbloc sbyteArray)
        {
            return new ebloc(sbyteArray.Value);
        }
        public static implicit operator sbloc(ebloc bloc)
        {
            return bloc.data;
        }
    }

