using Al.Security.Asn1.Nist;
using Al.Security.Asn1.X9;
using Al.Security.Crypto;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.Parameters;
using Al.Security.Crypto.Prng;
using Al.Security.Security;
using OpenSSL.Core;
using OpenSSL.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Al.Security.CA
{
  public  class KeyGenerationBind
    {
      public string KeyProvider {get;set;}
      public int KeySize { get; set; }

      
      public CryptoKey KeyPair { get; set; }
      public MessageDigest SignatureAlgorithm { get; set; }
      public bool ExportPrivate { get; set; }
      public bool ExportPublic { get; set; }
    public  MessageDigest GetMessageDigest(string algo)
      {
          switch (algo.ToUpper())
          {
              case "SHA1":
                  return MessageDigest.SHA1;
              case "SHA224":
                  return MessageDigest.SHA224;
              case "SHA256":
                  return MessageDigest.SHA256;
              case "SHA384":
                  return MessageDigest.SHA384;
              case "SHA512":
                  return MessageDigest.SHA512;
              case "RIPEMD160":
                  return MessageDigest.RipeMD160;
              case "MD4":
                  return MessageDigest.MD4;
              case "MD5":
                  return MessageDigest.MD5;
              case "ECDSA":
                  return MessageDigest.ECDSA;

              default:
                  return MessageDigest.SHA;

          }
      }
      /// <summary>
      /// Generate a key pair
      /// </summary>
      /// <param name="keyprovider">RSA,DSA,ECDSA_P***, ECDSA_B***</param>
      /// <param name="strength">size of the key</param>
      /// <param name="algo">Signature algorithm</param>
      public void Generate(string keyprovider, int strength, string algo)
      {
          SignatureAlgorithm = GetMessageDigest(algo);
          KeySize = strength;
          KeyProvider = keyprovider;
          if (keyprovider == "RSA")
          {

              using (var rsa = new RSA())
              {
                  BigNumber exponent = 0x10001; // this needs to be a prime number
                  rsa.GenerateKeys(strength, exponent, null, null);

                  KeyPair =  new CryptoKey(rsa);
              }
           
         
          }
          //else if (keyprovider.StartsWith("ECDSA"))
          //{
          //    //  var x=    new Al.Security.Crypto.Generators.DsaKeyPairGenerator();
          //    X9ECParameters ecP = NistNamedCurves.GetByName(keyprovider.Replace("ECDSA_","").Insert(1,"-"));
          //    ECDomainParameters ecSpec = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());
          //    ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator("ECDSA");

          //    keyPairGenerator.Init(new ECKeyGenerationParameters(ecSpec, new SecureRandom()));
          //    KeyPair = keyPairGenerator.GenerateKeyPair();
          //}
          else
          {

              using (var dsa = new DSA(strength,null,null))
              {
               //   BigNumber exponent = 0x10001; // this needs to be a prime number
                  dsa.GenerateKeys();

                  KeyPair = new CryptoKey(dsa);
              }
          }
      }

      /// <summary>
      /// Generate a key pair
      /// </summary>
      public void Generate()
      {

          if (KeyProvider == "RSA")
          {

              using (var rsa = new RSA())
              {
                  BigNumber exponent = 0x10001; // this needs to be a prime number
                  rsa.GenerateKeys(KeySize, exponent, null, null);

                  KeyPair = new CryptoKey(rsa);
              }


          }
          //else if (keyprovider.StartsWith("ECDSA"))
          //{
          //    //  var x=    new Al.Security.Crypto.Generators.DsaKeyPairGenerator();
          //    X9ECParameters ecP = NistNamedCurves.GetByName(keyprovider.Replace("ECDSA_","").Insert(1,"-"));
          //    ECDomainParameters ecSpec = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());
          //    ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator("ECDSA");

          //    keyPairGenerator.Init(new ECKeyGenerationParameters(ecSpec, new SecureRandom()));
          //    KeyPair = keyPairGenerator.GenerateKeyPair();
          //}
          else
          {

              using (var dsa = new DSA(KeySize, null, null))
              {
                  //   BigNumber exponent = 0x10001; // this needs to be a prime number
                  dsa.GenerateKeys();

                  KeyPair = new CryptoKey(dsa);
              }
          }
      }
    }
}
