using Al.Security.Asn1;
using Al.Security.Asn1.X509;
using System;
using System.Collections.Generic;

using System.Text;

namespace Al.Security.CA
{
  public  class x509NameBind
    {
      public static readonly Dictionary<string, string> Oids;
      public static readonly Dictionary<string, string> Reverse;
      public static readonly Dictionary<string, string> OpenSSLOids;
      public static readonly Dictionary<string, string> ReverseOids;
      static x509NameBind()
      {
          if (Oids == null)
          {
              OpenSSLOids = new Dictionary<string, string>();
              Reverse = new Dictionary<string, string>();
              ReverseOids = new Dictionary<string, string>();
              Oids = new Dictionary<string, string>();
              Oids.Add("ou", "2.5.4.11");
              Oids.Add("gender", "1.3.6.1.5.5.7.9.3");
              Oids.Add("surname", "2.5.4.4");
              Oids.Add("givenname", "2.5.4.42");
              Oids.Add("t", "2.5.4.12");
              Oids.Add("pseudonym", "2.5.4.65");
              Oids.Add("dateofbirth", "1.3.6.1.5.5.7.9.1");
              //Oids.Add("dn", "2.5.4.46");
              Oids.Add("st", "2.5.4.8");
              Oids.Add("l", "2.5.4.7");
              Oids.Add("dc", "0.9.2342.19200300.100.1.25");
              Oids.Add("initials", "2.5.4.43");
              Oids.Add("placeofbirth", "1.3.6.1.5.5.7.9.2");
              Oids.Add("cn", "2.5.4.3");
              Oids.Add("c", "2.5.4.6");
              Oids.Add("o", "2.5.4.10");
           //   Oids.Add("e", "1.2.840.113549.1.9.1");
              Oids.Add("telephonenumber", "2.5.4.20");
              Oids.Add("unstructuredname", "1.2.840.113549.1.9.2");
              Oids.Add("countryofresidence", "1.3.6.1.5.5.7.9.5");
              Oids.Add("nameofbirth", "1.3.36.8.3.14");
              Oids.Add("serialnumber", "2.5.4.5");
              Oids.Add("emailaddress", "1.2.840.113549.1.9.1");
              Oids.Add("postalcode", "2.5.4.17");
              Oids.Add("generation", "2.5.4.44");
              Oids.Add("countryofcitizenship", "1.3.6.1.5.5.7.9.4");
              Oids.Add("street", "2.5.4.9");
              Oids.Add("businesscategory", "2.5.4.15");
              Oids.Add("uniqueidentifier", "2.5.4.45");
              Oids.Add("unstructuredaddress", "1.2.840.113549.1.9.8");
              Oids.Add("uid", "0.9.2342.19200300.100.1.1");
              Oids.Add("postaladdress", "2.5.4.16");

         // EV
              Oids.Add("businessCategory", "2.5.4.15"); //TW
              Oids.Add("jurisdictionOfIncorporationStateOrProvinceName", "1.3.6.1.4.1.311.60.2.1.2"); //TN
              Oids.Add("jurisdictionOfIncorporationLocalityName", "1.3.6.1.4.1.311.60.2.1.1"); //TW
              Oids.Add("jurisdictionOfIncorporationCountryName", "1.3.6.1.4.1.311.60.2.1.3"); //TN


              OpenSSLOids.Add("CN", "cn"); //A
              OpenSSLOids.Add("O", "o"); //B
              OpenSSLOids.Add("OU", "ou"); //C
              OpenSSLOids.Add("emailAddress", "emailaddress"); //D
              OpenSSLOids.Add("ST", "st"); //E
              OpenSSLOids.Add("L", "l"); //F
              OpenSSLOids.Add("street", "street"); //G
              OpenSSLOids.Add("DC", "dc"); //H
              OpenSSLOids.Add("UID", "uid"); //I
              OpenSSLOids.Add("C", "c"); //JP
              OpenSSLOids.Add("SN", "surname"); //L
              OpenSSLOids.Add("GN", "givenname"); //M
              OpenSSLOids.Add("initials", "initials"); //N
              OpenSSLOids.Add("id-pda-gender", "gender"); //O
              OpenSSLOids.Add("x500UniqueIdentifier", "uniqueidentifier"); //P
              OpenSSLOids.Add("telephoneNumber", "telephonenumber"); //Q
              OpenSSLOids.Add("pseudonym", "pseudonym"); //R
              OpenSSLOids.Add("postalAddress", "postaladdress"); //S
              OpenSSLOids.Add("id-pda-countryOfCitizenship", "countryofcitizenship"); //TW
              OpenSSLOids.Add("id-pda-countryOfResidence", "countryofresidence"); //TN
              // EV SSL SUBJECT
              OpenSSLOids.Add("businessCategory", "2.5.4.15"); //TW
              OpenSSLOids.Add("jurisdictionOfIncorporationStateOrProvinceName", "1.3.6.1.4.1.311.60.2.1.2"); //TN
              OpenSSLOids.Add("jurisdictionOfIncorporationLocalityName", "1.3.6.1.4.1.311.60.2.1.1"); //TW
              OpenSSLOids.Add("jurisdictionOfIncorporationCountryName", "1.3.6.1.4.1.311.60.2.1.3"); //TN
              foreach (KeyValuePair<string, string> p in OpenSSLOids)
                  ReverseOids.Add(p.Value, p.Key);

              foreach (KeyValuePair<string, string> p in Oids)
              if(!Reverse.ContainsKey(p.Value))
                  Reverse.Add(p.Value, p.Key);
          }
      }
      public x509NameBind()
      {

      }
      List<string> Keys = new List<string>();
      public bool ReverseOrder()
      {
          Dictionary<string, string> nv = new Dictionary<string, string>();
          for (int i = Keys.Count - 1; i >= 0; i--)
              nv.Add(Keys[i], Vals[Keys[i]]);

          Vals = nv;
          return true;
      }
      public Dictionary<string, string> Vals = new Dictionary<string, string>();
   
      public bool Add(string name, string value)
      {
          string oid = GetOID(name);
        if(!Vals.ContainsKey(oid))
        {
              Vals.Add(oid, value);
            Keys.Add(oid);
        }
          else
              return false;

          return true;
      }
      public bool Add(string name, int value)
      {
          return Add(name, value.ToString());
      }
      public bool Add(string name, DateTime value)
      {
                    return Add(name, value.ToString());
      }
      public string TryGetOID(string name)
      {
          if (ReverseOids.ContainsKey(name))
              return ReverseOids[name];
          else if (OpenSSLOids.ContainsKey(name))
              return name;
          else
              return null;
      }
      public string GetOID(string name)
      {
          if (Oids.ContainsKey(name.ToLower()))
              return Oids[name.ToLower()];
          else if(OpenSSLOids.ContainsKey(name))
              return OpenSSLOids[name];
          else if (Reverse.ContainsKey(name))
          {
              string rev = Reverse[name];

              if (ReverseOids.ContainsKey(rev))
                  rev = ReverseOids[rev];
              
            
              return rev;

          }
          else return name;
      }
    }
}