using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Al.Security.CA
{

      public enum ElementType
  {
      BOOLEAN,
      NULL,
      INTEGER, 
      ENUM,
      OID, 
      UTCTIME,
      GENERALIZEDTIME,
      OCTETSTRING,
      BITSTRING,
      UNIVERSALSTRING,
      IA5STRING, 
      UTF8String, 
      BMPSTRING, 
      VISIBLESTRING, 
      PRINTABLESTRING,
      T61STRING, 
      TELETEXSTRING,
      GeneralString,
      NUMERICSTRING,
      NUMERIC,
      SEQUENCE

  }
    public class ExtensionCoder
  {
      public static void DefineSection(string name)
      {
          SectionManager.DefineSection(name, "["+name+"]");
      }
      public static void AddFieldInSection(string sectionname, string field)
      {
          if (SectionManager.SectionValues.ContainsKey(sectionname))
              SectionManager.SectionValues[sectionname] += Environment.NewLine + field;

          
      }
       
  }
    public class ASNElement
    {
        public ElementType EType { get; set; }
        public List<ASNElement> Childs { get; set; }
        public ASNElement ParentSection { get; set; }
        public bool HasChildrens
        {
            get
            {
                return (Childs.Count > 0);
            }

        }
        public string Value = "";
        public string Name = "";

        public readonly bool CanAdd;
        public bool IsValid()
        {

            if (EType == ElementType.BOOLEAN)
                return (Value.ToUpper() == "TRUE" || Value.ToUpper() == "FALSE");
            else if (EType != ElementType.SEQUENCE && HasChildrens)
                return false;
            else return true;
        }
        // Expected to sequence
        public ASNElement(ElementType type, string name)
        {
            EType = type;
            Childs = new List<ASNElement>();
            if (type == ElementType.SEQUENCE)
                CanAdd = true;
            else
                CanAdd = false;
            Name = name;
            SectionName = "";
        }
        public ASNElement(ElementType type, string name, string value)
        {
            SectionName = "";

            Childs = new List<ASNElement>();
            if (type == ElementType.SEQUENCE)
                CanAdd = true;
            else
                CanAdd = false;
            EType = type;
            Value = value;
            Name = name;
        }
        public bool HasElement(ASNElement el)
        {
            foreach (ASNElement e in Childs)
                if (e.Name == el.Name)
                    return true;

            return false;

        }
        public bool AddElement(ASNElement el)
        {
            if (!HasElement(el) && CanAdd)
            {
                Childs.Add(el);
                el.ParentSection = this;
                return true;
            }
            else return false;
        }
        public string SectionName { get; set; }

        public string ToOpenSSLConfig()
        {
            if (EType == ElementType.SEQUENCE)
            {
                string sectionname = SectionManager.CreateSectionSequence();
                SectionName = sectionname;
                ExtensionCoder.DefineSection(sectionname);
                if (ParentSection != null)
                    ExtensionCoder.AddFieldInSection(ParentSection.SectionName, Name + "=SEQUENCE:" + sectionname);

                // Childrens
                foreach (ASNElement el in Childs)
                    el.ToOpenSSLConfig();

                if (ParentSection == null)
                    return Name + "=" + EType.ToString() + ":" + sectionname;
            }
            else if (ParentSection != null)
                ExtensionCoder.AddFieldInSection(ParentSection.SectionName, Name + "=" + EType.ToString() + ":" + Value);
            else
                return Name + "=" + EType.ToString() + ":" + Value;

            return "";

        }
    }
    public class CustomSubjectName
    {
        public string NativeName;
        public string Value;

     public virtual string ToOpenSSLEntry()
        { 
     
            if(Value.Length > 0)
                       return NativeName + "="+Value;
            return "";


        }     
    }
    public class OidDeclaration
    {
        public string Name;
        public string OidNumber;

        public virtual string ToOpenSSLEntry()
        {


            if (OidNumber.Length > 0)
                return Name + "=" + OidNumber;
            return "";


        }
    }
    public class CustomExtension : X509Ext
    {
        void WriteXmlNode(StreamWriter str, ASNElement el)
        {

            if (el.HasChildrens)
            {
                str.WriteLine("<asn name=\"{0}\" value=\"{1}\" section=\"{2}\" type=\"{3}\">", el.Name.Replace("\"", "{$quote$}"), el.Value.Replace("\"", "{$quote$}"), el.SectionName.Replace("\"", "{$quote$}"), el.EType.ToString());

                foreach (ASNElement e in el.Childs)
                    WriteXmlNode(str, e);


                str.WriteLine("</asn>");
            }
            else
                str.WriteLine("<asn name=\"{0}\" value=\"{1}\" section=\"{2}\" type=\"{3}\" />", el.Name.Replace("\"", "{$quote$}"), el.Value.Replace("\"", "{$quote$}"), el.SectionName.Replace("\"", "{$quote$}"), el.EType.ToString());
        }
        public void SaveXml(string file)
        {
            using (StreamWriter str = new StreamWriter(file, false))
            {
                str.WriteLine("<ext oid=\"{0}\">", NativeName);
                foreach (ASNElement el in Elements)
                    WriteXmlNode(str, el);
                str.WriteLine("</ext>");
            }
        }
        ASNElement LoadASN(XmlElement el, ASNElement parent)
        {
            ElementType ety = (ElementType)Enum.Parse(typeof(ElementType), el.GetAttribute("type"));
            string name = el.GetAttribute("name").Replace("{$quote$}", "\"");
            string value = el.GetAttribute("value").Replace("{$quote$}", "\"");
            string section = el.GetAttribute("section").Replace("{$quote$}", "\"");
            ASNElement asn = null;
            if (ety == ElementType.SEQUENCE)
            {
                asn = new ASNElement(ety, name);
                asn.SectionName = section;
                if (parent != null)
                    asn.ParentSection = parent;

                foreach (XmlElement e in el.ChildNodes)
                    asn.AddElement(LoadASN(e, asn));
            }
            else
            {
                asn = new ASNElement(ety, name, value);
                asn.SectionName = section;
                if (parent != null)
                    asn.ParentSection = parent;



            }
            return asn;

        }
        public void LoadFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            NativeName = doc.DocumentElement.GetAttribute("oid");
            Elements = new List<ASNElement>();
            foreach (XmlElement e in doc.DocumentElement.ChildNodes)
                Elements.Add(LoadASN(e, null));

        }
        public List<ASNElement> Elements { get; set; }

        public CustomExtension(string oid)
        {
            Elements = new List<ASNElement>();
            NativeName = oid;
        }
        public override string ToOpenSSLEntry()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ASNElement el in Elements)
            {
                string code = el.ToOpenSSLConfig();
                if (code.Length > 0)
                    sb.AppendLine(code);
            }
            string crit = "";
            if (Critical)
                crit = "critical, ";
            if (sb.Length > 0)
            {
                string sec = SectionManager.CreateSection("EXT");
                SectionManager.DefineSection(sec, Environment.NewLine + "[" + sec + "]" + Environment.NewLine + sb.ToString());
                return NativeName + "=" + crit + "ASN1:SEQUENCE:" + sec;
            }
            return "";
        }
   
    }


    public class SectionManager
    {
        public static void RemoveFixedSection(string name)
        {
            if (sections.Contains(name))
                sections.Remove(name);
            if (FixedSectionValues.ContainsKey(name))
                FixedSectionValues.Remove(name);
        }
        internal static Dictionary<string, string> SectionValues = new Dictionary<string, string>();
        public static Dictionary<string, string> FixedSectionValues = new Dictionary<string, string>();
        static List<string> sections = new List<string>();
        static Random rd = new Random();
        public static string CreateSection(string prefix)
        {
            string sec = prefix + rd.Next().ToString();
            if (sections.Contains(sec))
                return CreateSection(prefix);
            else
            {
                sections.Add(sec);
                return sec;

            }
        }
        public static string CreateSectionCrl()
        {
            string sec = "crldp" + rd.Next().ToString() + "_section";
            if (sections.Contains(sec))
                return CreateSectionCrl();
            else
            {
                sections.Add(sec);
                return sec;

            }
        }
        public static void DefineSection(string section, string code)
        {
            if (!SectionValues.ContainsKey(section))
              SectionValues.Add(section, code);
            
        }
        public static void DefineSectionFixed(string section, string code)
        {
            if (!FixedSectionValues.ContainsKey(section))
                FixedSectionValues.Add(section, code);
            

        }
        public static string CreateSectionSequence()
        {
            string sec = "seq" + rd.Next().ToString() + "_sect";
            if (sections.Contains(sec))
                return CreateSectionSequence();
            else
            {
                sections.Add(sec);
                return sec;

            }
        }
        public static string SectionsToCode()
        {
            string code = "";
            foreach (KeyValuePair<string, string> K in SectionValues)
                         code = code.Insert(0,K.Value + Environment.NewLine);
            foreach (KeyValuePair<string, string> K in FixedSectionValues)
                code = code.Insert(0, K.Value + Environment.NewLine);
            return code;
        }
        public static void Clean()
        {
            SectionValues.Clear();
            sections.Clear();
        }
    }

    
    public abstract class X509Ext
    {
   public bool Critical = false;
      public string NativeName = "extendedKeyUsage";
      public string Value;
        public virtual string ToOpenSSLEntry()
        {    string crit = "";
          if (Critical)
              crit = "critical,";
            if(Value.Length > 0)
                       return NativeName + "="+ crit+Value;
            return "";


        }
    }
    public enum X509KeyUsages
    {
        digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment, keyAgreement, keyCertSign, cRLSign, encipherOnly ,decipherOnly
    }
  public  class X509KeyUsageExt : X509Ext
    {
 

      public X509KeyUsageExt(List<X509KeyUsages> ku)
      {
          NativeName = "keyUsage";
          StringBuilder sb = new StringBuilder();
          foreach (X509KeyUsages keu in ku)
              sb.Append(keu.ToString() + ", ");
          string keys = sb.ToString();
          if(sb.Length >0)
              keys = keys.Remove(sb.Length - 2, 2);
          Value = keys;
      }
  
    }


  public enum X509ExtendedKeyUsages
  {
      serverAuth, //     SSL/TLS Web Server Authentication.
      clientAuth, //     SSL/TLS Web Client Authentication.
      codeSigning, //    Code signing.
      emailProtection, //E-mail Protection (S/MIME).
      timeStamping, //   Trusted Timestamping
      msCodeInd, //      Microsoft Individual Code Signing (authenticode)
      msCodeCom, //      Microsoft Commercial Code Signing (authenticode)
      msCTLSign, //      Microsoft Trust List Signing
      msSGC, //  Microsoft Server Gated Crypto
      msEFS,  //  Microsoft Encrypted File System
      nsSGC //  Netscape Server Gated Crypto
  }
  public class X509ExtendedKeyUsageExt : X509Ext
  {


      public X509ExtendedKeyUsageExt(List<X509ExtendedKeyUsages> ku)
      {
          NativeName = "extendedKeyUsage";
          StringBuilder sb = new StringBuilder();
          foreach (X509ExtendedKeyUsages keu in ku)
              sb.Append(keu.ToString() + ", ");
          string keys = sb.ToString();
          if (sb.Length > 0)
              keys = keys.Remove(sb.Length - 2, 2);
          Value = keys;
      }
      public X509ExtendedKeyUsageExt(List<X509ExtendedKeyUsages> ku, List<string> customoid)
      {
          NativeName = "extendedKeyUsage";
          StringBuilder sb = new StringBuilder();
          foreach (X509ExtendedKeyUsages keu in ku)
              sb.Append(keu.ToString() + ", ");

          foreach (string ckeu in customoid)
              sb.Append(ckeu + ", ");
         // sb.Append("1.3.6.1.4.1.44215.1.1, ");
          string keys = sb.ToString();
          if (sb.Length > 0)
              keys = keys.Remove(sb.Length - 2, 2);
          Value = keys;
      }
     
  }

  public class BasicConstraintsExt : X509Ext
  {
      public BasicConstraintsExt(bool CA, int pathlen)
      {
          NativeName = "basicConstraints";
          if (CA)
          {
              Value = "CA:TRUE";
              if (pathlen > -1)
                  Value += ", pathlen:"+pathlen.ToString();
          }
          else
              Value = "CA:FALSE";
      }

 
  }
 
    
 public class SubjectKeyIdentifierExt : X509Ext
  {

     public SubjectKeyIdentifierExt()
      {
          NativeName = "subjectKeyIdentifier";
          Value = "hash";

      }
  }
  public class AuthorityKeyIdentifierExt : X509Ext
  {

      public AuthorityKeyIdentifierExt()
      {
          NativeName = "authorityKeyIdentifier";
          Value = "keyid,issuer";
      }
  }



  public enum GeneralNameHook : int
  {
      email = 1,
      URI = 6,
      DNS = 2,
      RID = 8,
      IP = 7,
      dirName=4,
      otherName = 0

  }
  public class GlobalName
  {
      public GeneralNameHook NameType;
      public string Value="";
      string SectionValue="";
      public string SectionName = "";
      public GlobalName(GeneralNameHook hk, string value)
      
      {
          NameType = hk;
          if (hk == GeneralNameHook.dirName)
          {
              
          string sec = SectionManager.CreateSection("S");
          SectionName = sec;
              Value = "dirName:"+sec;
              StringBuilder sb = new StringBuilder();
              
             sb.AppendLine("[" + sec + "]");
             foreach (string val in value.Split(','))
                 sb.AppendLine(val);

             SectionValue = sb.ToString();
             SectionManager.DefineSectionFixed(sec, SectionValue);
          }
          else
              Value = hk.ToString() + ":" + value;

        
      }

  }

  public class SubjectAltNameExt : X509Ext
  {
      string sections="";
      public SubjectAltNameExt(List<GlobalName> Gn)
      {
          NativeName = "subjectAltName";
          Value = "";
        
          foreach (GlobalName g in Gn)
          {
              Value += g.Value+", ";

              //if (g.SectionValue != "")
              //    sections += g.SectionValue + Environment.NewLine;
          }
          if (Value.Length > 0)
              Value = Value.Remove(Value.Length - 2, 2);
      }
      public override string ToOpenSSLEntry()
      {
          string basic = base.ToOpenSSLEntry();
          basic += Environment.NewLine + sections;
          return basic;
      }
  }

  public class IssuerAltNameExt : X509Ext
  {
      string sections = "";
      public IssuerAltNameExt(List<GlobalName> Gn)
      {
          NativeName = "issuerAltName";
          Value = "";

          foreach (GlobalName g in Gn)
          {
              Value += g.Value + ", ";

              //if (g.SectionValue != "")
              //    sections += g.SectionValue + Environment.NewLine;
          }
          if (Value.Length > 0)
              Value = Value.Remove(Value.Length - 2, 2);
      }
      public IssuerAltNameExt(bool copy)
      {
               NativeName = "issuserAltName";
               Value = "issuer:copy";

      }
      public override string ToOpenSSLEntry()
      {
          string basic = base.ToOpenSSLEntry();
          basic += Environment.NewLine + sections;
          return basic;
      }
  }

  public class AccessInformationEntry
  {
     public string EntryCode = "";
     public AccessInformationEntry(bool ocsp, List<GlobalName> gn)
      {
          //string sections = "";
          string value = "caIssuers;";
       if(ocsp)
         value = "OCSP;";

          foreach (GlobalName g in gn)
          {
              if (g.NameType != GeneralNameHook.dirName)
                  value += g.Value + ", ";
         
              //if (g.SectionValue != "")
              //    sections += g.SectionValue + Environment.NewLine;
          }
          if (value.Length > 0)
              value = value.Remove(value.Length - 2, 2);
          else if (value == "caIssuer" || value == "OCS")
              value = "";
          EntryCode = value;

      }
  }
  public class AuthorityInfoAccessExt : X509Ext
  {

      public AuthorityInfoAccessExt(AccessInformationEntry acs)
      {
          NativeName = "authorityInfoAccess";
          Value = acs.EntryCode;

      }
      public AuthorityInfoAccessExt(List<AuthorityInfoAccessExt> acs)
      {
          NativeName = "authorityInfoAccess";
                  Value = "";
                  foreach (AuthorityInfoAccessExt ext in acs)
                      Value += ext.Value + ", ";

                  if (Value.Length > 0)
                      Value = Value.Remove(Value.Length - 2, 2);

      }
  }

  public enum ReasonsHook : int
  {
      keyCompromise =  (1 << 6),
 CACompromise =  (1 << 5), 
affiliationChanged = (1 << 4), 
superseded =(1 << 3) , 
cessationOfOperation = (1 << 2), 
certificateHold= (1 << 1), 
privilegeWithdrawn=(1 << 0),
AACompromise=(1 << 15)
  }

  public class DistributionPointEntry
  {
      public string Name="";
      public string SectionCode="";
      public bool Issuer = false;
      public DistributionPointEntry(List<ReasonsHook> reasons, List<GlobalName> access, List<GlobalName> crlissuer)
      {

          string sec = SectionManager.CreateSectionCrl();
          Name = sec;
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("["+sec+"]");
              string fullname = "";
              if (access.Count > 0)
              {
                  fullname = "fullname=";
                  string fs = "";
                  foreach (GlobalName g in access)
                  {
                      fullname += g.Value + ", ";

                      //if (g.SectionValue != "")
                      //    fs += g.SectionValue + Environment.NewLine;
                  }
                  if (fullname.Length > 0)
                      fullname = fullname.Remove(fullname.Length - 2, 2);
              }
              string crliss = "";

          crliss = "CRLissuer=";
          string cs = "";
          foreach (GlobalName g in crlissuer)
          {
              crliss += g.Value + ", ";

              //if (g.SectionValue != "")
              //    cs += g.SectionValue + Environment.NewLine;
          }
          if (crliss.Length > 0)
              crliss = crliss.Remove(crliss.Length - 2, 2);
          string reason = "";
          if (reasons.Count > 0)
          {
               reason = "reasons=";
              foreach (ReasonsHook rh in reasons)
                  reason += rh.ToString() + ", ";

              if (reason.Length > 0)
                  reason = reason.Remove(reason.Length - 2, 2);
          }
          sb.AppendLine(fullname);
          sb.AppendLine(crliss);
          sb.AppendLine(reason);
          //if (fs == cs)
          //    sb.AppendLine(fs);
          //else
          //{

          //    sb.AppendLine(fs);
          //    sb.AppendLine(cs);
          //}
          //sb.AppendLine("[issuer_sect]");
          //sb.AppendLine(" C=TN");
          SectionCode = sb.ToString();
          SectionName = sec;
          SectionManager.DefineSectionFixed(sec, SectionCode);
      }
      public string SectionName = "";
      public DistributionPointEntry(List<ReasonsHook> reasons, List<GlobalName> access, bool indirecturl,bool onlyca, bool onlyaa, bool onlyuser)
      {
          Issuer = true;
          string sec = SectionManager.CreateSection("IDP");
          Name = sec;
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("[" + sec + "]");
          string fullname = "";
          if (access.Count > 0)
          {
              fullname = "fullname=";
              string fs = "";
              foreach (GlobalName g in access)
              {
                  if (g.NameType != GeneralNameHook.dirName)
                      fullname += g.Value + ", ";

                  //if (g.SectionValue != "")
                  //    fs += g.SectionValue + Environment.NewLine;
              }
              if (fullname.Length > 0)
                  fullname = fullname.Remove(fullname.Length - 2, 2);
          }
          string reason = "";
          if (reasons.Count > 0)
          {
              reason = "onlysomereasons=";
              foreach (ReasonsHook rh in reasons)
                  reason += rh.ToString() + ", ";

              if (reason.Length > 0)
                  reason = reason.Remove(reason.Length - 2, 2);
          }
          sb.AppendLine(fullname);
         sb.AppendLine(reason);
         sb.AppendLine("indirectCRL=" + indirecturl.ToString().ToUpper());
         sb.AppendLine("onlyuser=" +  onlyuser.ToString().ToUpper());
         sb.AppendLine("onlyCA=" + onlyca.ToString().ToUpper());
         sb.AppendLine("onlyAA=" + onlyaa.ToString().ToUpper());


          //if (fs == cs)
          //    sb.AppendLine(fs);
          //else
          //{

          //    sb.AppendLine(fs);
          //    sb.AppendLine(cs);
          //}

         sb.AppendLine("[issuer_sect]");
         sb.AppendLine(" C=TN");
         SectionCode = sb.ToString();
         SectionName = sec;
          SectionManager.DefineSectionFixed(sec, SectionCode);
      }

     
  }
  public class CrlDistributionPointsExt : X509Ext
  {
      string sections = "";
      public CrlDistributionPointsExt(DistributionPointEntry ent)
      {
          if (ent.Issuer)
              throw new ArgumentException("This is not a valid crl distribution point");
          NativeName = "crlDistributionPoints";
          Value = ent.Name;
          sections = Environment.NewLine + ent.SectionCode + Environment.NewLine;
      }
      public CrlDistributionPointsExt(List<DistributionPointEntry> ents)
      {
          NativeName = "crlDistributionPoints";
          Value = "";
          foreach (DistributionPointEntry ent in ents)
          {
              if (ent.Issuer)
                  throw new ArgumentException("This is not a valid crl distribution point");
              Value +=ent.Name+", ";
              if(!sections.Contains(ent.SectionCode))
              sections += Environment.NewLine + ent.SectionCode + Environment.NewLine;
          }
          if(Value.Length > 0)
              Value = Value.Remove(Value.Length - 2, 2);
       
 
      }
      
      public override string ToOpenSSLEntry()
      {
          return base.ToOpenSSLEntry();
      }

  }

  public class IssuingDistributionPointsExt : X509Ext
  {
      //TODO multiple ISSUING DISTRIBUTION POINTS
      string sections = "";
      public IssuingDistributionPointsExt(DistributionPointEntry ent)
      {
          if (!ent.Issuer)
              throw new ArgumentException("This is not a valid issuer distribution point");
          NativeName = "issuingDistributionPoint";
          Value = "@"+ent.Name;
          sections = Environment.NewLine + ent.SectionCode + Environment.NewLine;
      }
    
      public override string ToOpenSSLEntry()
      {
          return base.ToOpenSSLEntry();
      }

  }

  public class PolicyInformationEntry
  {
      public string Name;
      string SectionValue="";
      public string SectionName = "";
      public PolicyInformationEntry(string oid,string cps, string org, string usernotice)
      {
          Name = SectionManager.CreateSection("POL");
      
                SectionValue="["+Name+"]"+Environment.NewLine;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("policyIdentifier="+oid);
          if(!string.IsNullOrEmpty(cps))
              sb.AppendLine("CPS.1="+cps);

          if (!string.IsNullOrEmpty(org) || !string.IsNullOrEmpty(usernotice))
          {
              string noticesection = SectionManager.CreateSection("NOTICEPOL");
              sb.AppendLine("userNotice.1=@" + noticesection);
              SectionValue += sb.ToString();

              sb.Length = 0;


              string noticecode = "[" + noticesection + "]" + Environment.NewLine;
              if (!string.IsNullOrEmpty(usernotice))
              {
                  sb.AppendLine("explicitText=" + usernotice);
                  sb.AppendLine("noticeNumbers=1");
              }
              if (!string.IsNullOrEmpty(org))
                  sb.AppendLine("organization=" + org);


         

              noticecode += sb.ToString();
              SectionManager.DefineSectionFixed(noticesection, noticecode);
              SectionName = noticesection;
          }
          else
              SectionValue += sb.ToString();
          SectionManager.DefineSectionFixed(Name, SectionValue);
      }
  }
  public class CertificatePoliciesExt : X509Ext
  {
      public CertificatePoliciesExt(PolicyInformationEntry ent)
      {
          NativeName = "certificatePolicies";
          Value = "ia5org,@" + ent.Name;

      }
      public CertificatePoliciesExt(List<PolicyInformationEntry> ents)
      {
          NativeName = "certificatePolicies";
          Value = "";
          foreach (PolicyInformationEntry ent in ents)
              Value += "@"+ent.Name+", ";

          if (Value.Length > 0)
              Value = Value.Remove(Value.Length - 2, 2).Insert(0, "ia5org,");

      }
  }


  public class PolicyConstraintsExt : X509Ext
  {
      public PolicyConstraintsExt(int exp, int inh)
      {
          NativeName = "policyConstraints";
          Value = "";
          if (exp > -1)
              Value += "requireExplicitPolicy:"+exp.ToString();
          if (inh > -1)
          {
              if (Value.Length > 0)
                  Value += ", inhibitPolicyMapping:" + inh.ToString();
              else
                  Value = "inhibitPolicyMapping:" + inh.ToString();
          }         
      }
  }
  public class InhibAnyPolicyConstraintsExt : X509Ext
  {
      public InhibAnyPolicyConstraintsExt(int h)
      {
          NativeName = "inhibitAnyPolicy";
        
          if (h > -1)
              Value = h.ToString();
     
      }
  }
  public class NsCommentExt : X509Ext
  {
      public NsCommentExt(string h)
      {
          NativeName = "nsComment";
             Value =h;

      }
  }
  public class NsUrlExt : X509Ext
  {
      public NsUrlExt(string h)
      {
          NativeName = "nsBaseUrl";
          Value = h;

      }
  }
  public class nsRevocationUrlExt : X509Ext
  {
      public nsRevocationUrlExt(string h)
      {
          NativeName = "nsRevocationUrl";
          Value = h;

      }
  }
  public class nsCaRevocationUrlExt : X509Ext
  {
      public nsCaRevocationUrlExt(string h)
      {
          NativeName = "nsCaRevocationUrl";
          Value = h;

      }
  }

  public class nsRenewalUrlExt : X509Ext
  {
      public nsRenewalUrlExt(string h)
      {
          NativeName = "nsRenewalUrl";
          Value = h;

      }
  }
  public class nsCaPolicyUrlExt  : X509Ext
  {
      public nsCaPolicyUrlExt(string h)
      {
          NativeName = "nsCaPolicyUrl";
          Value = h;

      }
  }
  public class  nsSslServerNameExt : X509Ext
  {
      public nsSslServerNameExt(string h)
      {
          NativeName = "nsSslServerName";
          Value = h;

      }
  }


    public enum NetscapeKeyUsage
    {
        client,
        server,
        email,
        objsign, 
        reserved, 
        sslCA, 
        emailCA, 
        objCA

    }
  public class NsKeyUsageExt : X509Ext
  {
      public NsKeyUsageExt(List<NetscapeKeyUsage> h)
      {
          NativeName = "nsCertType";

          Value = "";
          foreach (NetscapeKeyUsage k in h)
              Value += k.ToString()+", ";

          if (Value.Length > 0)
              Value = Value.Remove(Value.Length - 2, 2);


      }
  }

  public class NameConstraintsExt : X509Ext
  {
      public NameConstraintsExt(GlobalName g, bool permitted)
      {
          NativeName = "nameConstraints";
          Value = "";
          if (permitted)
          {
              if (g.NameType != GeneralNameHook.IP)
                  Value = "permitted;" + g.Value;
          }
          else
          {
              if (g.NameType != GeneralNameHook.IP)
                  Value = "excluded;" + g.Value;
          }
          
      }
      public NameConstraintsExt(List<NameConstraintsExt> acs)
      {
          NativeName = "nameConstraints";
                  Value = "";
                  foreach (NameConstraintsExt ext in acs)
                      Value += ext.Value + ", ";

                  if (Value.Length > 0)
                      Value = Value.Remove(Value.Length - 2, 2);

      }
  }


  public class X509ExtensionManager
  {
      public static void Export(string config, List<X509Ext> ext)
      {
          using (StreamWriter str = new StreamWriter(config, false))
          {
              str.WriteLine("[ cert ]");
              str.WriteLine("default_ca	= CA_default");
              str.WriteLine("[ CA_default ]");
              str.WriteLine("x509_extensions	= v3_ca");
              str.WriteLine("[ v3_ca ]");
              foreach (X509Ext ex in ext)
              {
                  str.WriteLine(ex.ToOpenSSLEntry());
              }
              str.WriteLine(SectionManager.SectionsToCode());
            //  str.WriteLine(Al.Security.CA.Properties.Resources.newoids);
          }
      }
  }

}
