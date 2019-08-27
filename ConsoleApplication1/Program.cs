using Al.Security.Asn1;
using Al.Security.Asn1.Nist;
using Al.Security.Asn1.X509;
using Al.Security.Asn1.X509.Qualified;
using Al.Security.Asn1.X9;
using Al.Security.Crypto;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.Parameters;
using Al.Security.Crypto.Prng;
using Al.Security.Math;
using Al.Security.OpenSsl;
using Al.Security.Pkcs;
using Al.Security.Security;
using Al.Security.Utilities;
using Al.Security.X509;
using Al.Security.X509.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static DerSequence CreatePolicyInformationsSequence(string cps, string org, string oid, string desc)
        {

        
            Asn1EncodableVector av = new Asn1EncodableVector();
            av.Add(new DerInteger(1));
            DerSequence noticeNumbers = new DerSequence(av);

            Asn1EncodableVector qualifiers = new Asn1EncodableVector();

           if (cps != null)
            {
                PolicyQualifierInfo cpsnotice = new PolicyQualifierInfo(cps);
                qualifiers.Add(cpsnotice);
            }
            if(org != null && desc != null)
            {
            UserNotice un = new UserNotice(
                new NoticeReference(DisplayText.ContentTypeIA5String, org, noticeNumbers), // OPTIONAL Orgname
                        new DisplayText(DisplayText.ContentTypeVisibleString, desc
                        )
        );

            PolicyQualifierInfo pqiUNOTICE = new PolicyQualifierInfo(PolicyQualifierID.IdQtUnotice, un);
      
    
            qualifiers.Add(pqiUNOTICE);
            }

            DerObjectIdentifier policy = null;
            policy = new DerObjectIdentifier(oid);


            PolicyInformation policyInformation =
                new PolicyInformation(policy, new DerSequence(qualifiers));

            return new DerSequence(policyInformation);
            
        }
        static DerSequence CreateAuthorityAccessInformationSequence(string caissuer, string ocsp)
        {
            Asn1EncodableVector aia_ASN = new Asn1EncodableVector();
          // TODO
        //    AccessDescription ocsp = new AccessDescription(AccessDescription.id_ad_ocsp,
        //new GeneralName(GeneralName.uniformResourceIdentifier, new DERIA5String("http://ocsp.somewebsite.com")));
 
            AccessDescription acd = new AccessDescription(AccessDescription.IdADCAIssuers, new GeneralName(GeneralName.UniformResourceIdentifier, "http://www.arsslensoft.com"));
            aia_ASN.Add(acd);
            return new DerSequence(aia_ASN);
        }
        static void Main(string[] args)
        {
            PolicyInformation[] certPolicies = new PolicyInformation[2];
            certPolicies[0] = new PolicyInformation(new DerObjectIdentifier("2.16.840.1.101.2.1.11.5"));
            certPolicies[1] = new PolicyInformation(new DerObjectIdentifier("2.16.840.1.101.2.1.11.18"));

            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);
            var certificateGenerator = new X509V3CertificateGenerator();
         //serial
            var serialNumber =
    BigIntegers.CreateRandomInRange(
        BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            certificateGenerator.SetSerialNumber(serialNumber);
            // sig alg
          
            const string signatureAlgorithm = "SHA1WithRSA";
            certificateGenerator.SetSignatureAlgorithm(signatureAlgorithm);
           
           // Subjects
         //   Time x = new Time();
            var subjectDN = new X509Name("CN=localhost, O=Arsslensoft, C=TN,surname=Idadi,givenname=Arsslen, uniqueidentifier=15002060,businesscategory=Production,initials=Hello, gender=male, placeofbirth=El Manar, pseudonym=Arsslinko, postaladdress=2076, countryofcitizenship=TN, countryofresidence=TN,telephonenumber=53299093");
            var issuerDN = subjectDN;
            certificateGenerator.SetIssuerDN(issuerDN);
            certificateGenerator.SetSubjectDN(subjectDN);
           
            // Validity
            var notBefore = DateTime.UtcNow.Date.Subtract(new TimeSpan(5,0,0));
            var notAfter = notBefore.AddYears(2);

            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);
           
            // PKEY
            const int strength = 512;
           var keyGenerationParameters = new KeyGenerationParameters (random, strength);
         
      //  var x=    new Al.Security.Crypto.Generators.DsaKeyPairGenerator();
          // X9ECParameters ecP = NistNamedCurves.GetByName("B-571");
          // ECDomainParameters ecSpec = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());
          // ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator("ECDSA");
          // //ECPA par = new DsaParametersGenerator();
          // //par.Init(2048, 100, random);
          // //ECKeyGenerationParameters pa = new ECKeyGenerationParameters(random, par.GenerateParameters());
          ////  var keyPairGenerator = new DHKeyPairGenerator();
          //  //DsaParametersGenerator par = new DsaParametersGenerator();
          //  //par.Init(2048, 100, random);
          //  //DsaKeyGenerationParameters pa = new DsaKeyGenerationParameters(random, par.GenerateParameters());
          // // keyPairGenerator.Init(pa);
          // keyPairGenerator.Init(new ECKeyGenerationParameters(ecSpec, new SecureRandom()));
           //var keyPairGenerator = new DsaKeyPairGenerator();
           //DsaParametersGenerator par = new DsaParametersGenerator();
           //par.Init(1024, 100, random);
           //DsaKeyGenerationParameters pa = new DsaKeyGenerationParameters(random, par.GenerateParameters());
           //keyPairGenerator.Init(pa);
        //   KeyPair = keyPairGenerator.GenerateKeyPair();

           var keyPairGenerator = new RsaKeyPairGenerator();
           keyPairGenerator.Init(keyGenerationParameters);
           StreamReader str = new StreamReader("D:\\test.key");
           PemReader pem = new PemReader(str);
           AsymmetricCipherKeyPair keypair = (AsymmetricCipherKeyPair)pem.ReadObject();
           var subjectKeyPair = keypair;
           str.Close();
            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            // ext
        X509Extensions

                  certificateGenerator.AddExtension(X509Extensions.SubjectKeyIdentifier, false,
                    new SubjectKeyIdentifierStructure(subjectKeyPair.Public)); 
               certificateGenerator.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(subjectKeyPair.Public));
              certificateGenerator.AddExtension(X509Extensions.BasicConstraints, false, new BasicConstraints(false));
                // key usage
            certificateGenerator.AddExtension(
             X509Extensions.KeyUsage,
             true,
             new KeyUsage(KeyUsage.KeyAgreement | KeyUsage.DataEncipherment | KeyUsage.DigitalSignature));
            // extended key usage
            var usages = new[] { KeyPurposeID.IdKPServerAuth ,KeyPurposeID.IdKPClientAuth };
                ExtendedKeyUsage extendedKeyUsage = new ExtendedKeyUsage(usages);
                certificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage, false, extendedKeyUsage);
            // Test Policy

                DerSequence seq = CreatePolicyInformationsSequence("http://www.arsslensoft.com", "Arsslensoft", "1.3.6.1.4.1.23823.1.1.1", "Test Notice");

              //  certificateGenerator.AddExtension(X509Extensions.CertificatePolicies, false, new DerSequence(certPolicies));

            // Authority access
                List<GeneralSubtree> ees = new List<GeneralSubtree>();
                ees.Add(new GeneralSubtree(new GeneralName(GeneralName.UniformResourceIdentifier,"http://www.google.com")));
                certificateGenerator.AddExtension(X509Extensions.NameConstraints, true, new NameConstraints(null, ees));
             
                certificateGenerator.AddExtension(X509Extensions.NetscapeComment, true, new DerVisibleString("NS COMMENT"));
                certificateGenerator.AddExtension(X509Extensions.NetscapeBaseUrl, true, new DerIA5String("http://www.google.com"));
                certificateGenerator.AddExtension(X509Extensions.InhibitAnyPolicy, true, new DerInteger(12));
// Policy constraints
                byte inhibit = 12;
               byte explicitc = 12;
             //   certificateGenerator.AddExtension(X509Extensions.PolicyConstraints, false, new DerOctetSequence(new byte[] { 128, 1, explicitc, 129, 1, inhibit }));
                certificateGenerator.AddExtension(X509Extensions.NetscapeCertUsage, false, new KeyUsage(KeyUsage.KeyAgreement));

            certificateGenerator.AddExtension(X509Extensions.AuthorityInfoAccess, false, CreateAuthorityAccessInformationSequence("http://www.arsslensoft.com",null));
            // Subhect Issuer Alternative name
      GeneralName altName = new GeneralName(GeneralName.DnsName, "localhost");
GeneralNames subjectAltName = new GeneralNames(altName);
 
            certificateGenerator.AddExtension(X509Extensions.IssuerAlternativeName,false, subjectAltName);
            certificateGenerator.AddExtension(X509Extensions.SubjectAlternativeName, false, subjectAltName);
         //   certificateGenerator.AddExtension(new DerObjectIdentifier("2.16.840.1.11730.29.53"), false, subjectAltName);
            //
      
            GeneralNames s;
         
            //CRL Distribution Points
            DistributionPointName distPointOne = new DistributionPointName(new GeneralNames(
                    new GeneralName(GeneralName.UniformResourceIdentifier, "http://crl.somewebsite.com/master.crl")));
            GeneralNames gns = new GeneralNames(new GeneralName[] {
                    new GeneralName(GeneralName.UniformResourceIdentifier, "ldap://crl.somewebsite.com/cn%3dSecureCA%2cou%3dPKI%2co%3dCyberdyne%2cc%3dUS?certificaterevocationlist;binary"), new GeneralName(GeneralName.Rfc822Name,"Arslen")});
            DistributionPointName distPointTwo = new DistributionPointName(gns);

            DistributionPoint[] distPoints = new DistributionPoint[2];
            distPoints[0] = new DistributionPoint(distPointOne, null, null);
            distPoints[1] = new DistributionPoint(distPointTwo, null, gns);
         
            IssuingDistributionPoint iss = new IssuingDistributionPoint(distPointOne, false, true,null, false,false);
            certificateGenerator.AddExtension(X509Extensions.IssuingDistributionPoint, false ,iss);
        
            certificateGenerator.AddExtension(X509Extensions.CrlDistributionPoints, false, new CrlDistPoint(distPoints));
 
            // Biometric
            Asn1EncodableVector v = new Asn1EncodableVector();
         
            BiometricData bdat = new BiometricData(new TypeOfBiometricData(TypeOfBiometricData.HandwrittenSignature), new AlgorithmIdentifier(new DerObjectIdentifier("1.3.14.3.2.26")), new DerOctetString(new byte[] { 169, 74, 143, 229, 204, 177, 155, 166, 28, 76, 8, 115, 211, 145, 233, 135, 152, 47, 187, 211 }), new DerIA5String("http://www.google.com"));
            v.Add(bdat);
            v.Add(new BiometricData(new TypeOfBiometricData(TypeOfBiometricData.HandwrittenSignature), new AlgorithmIdentifier(new DerObjectIdentifier("1.3.14.3.2.26")), new DerOctetString(new byte[] { 169, 74, 143, 229, 204, 177, 155, 166, 28, 76, 8, 115, 211, 145, 233, 135, 152, 47, 187, 211 }), new DerIA5String("http://www.google.co")));
            certificateGenerator.AddExtension(X509Extensions.BiometricInfo, false, new DerSequenceOf(v));
            
            QCStatement st = new QCStatement(Rfc3739QCObjectIdentifiers.IdQcs);
            certificateGenerator.AddExtension(X509Extensions.QCStatements, false,st);
             //Al.Security.Pkcs.Pkcs10CertificationRequest c = new Al.Security.Pkcs.Pkcs10CertificationRequest(     
        //certificateGenerator.AddExtension(X509Extensions.ReasonCode, false, ce);
            // test done
            certificateGenerator.AddExtension(X509Extensions.SubjectInfoAccess, false, CreateAuthorityAccessInformationSequence("http://www.arsslensoft.com", null));
            //// 2
            //TargetInformation ti = new Al.Security.Asn1.X509.TargetInformation(new Target[] { new Target(Target.Choice.Name, new GeneralName(GeneralName.UniformResourceIdentifier, "http://www.go.com")) });
            //certificateGenerator.AddExtension(X509Extensions.TargetInformation, false, new DerSequence(ti));
 // 3
            PrivateKeyUsagePeriod kup = new PrivateKeyUsagePeriod(DateTime.Now, DateTime.Now.AddYears(2));
            certificateGenerator.AddExtension(X509Extensions.PrivateKeyUsagePeriod, false,  new DerSequence(kup));
         
    
            //generate
            var issuerKeyPair = subjectKeyPair;
            var certificate = certificateGenerator.Generate(issuerKeyPair.Private, random);
   
            
            StreamWriter wstr = new StreamWriter(Path.ChangeExtension("D:\\test.crt", ".pem"), false);
            PemWriter pemWriter = new PemWriter(wstr);
            pemWriter.WriteObject(certificate);
            pemWriter.WriteObject(issuerKeyPair.Private);

            wstr.Flush();
            wstr.Close();
                  
         //   System.Security.Cryptography.X509Certificates.X509Certificate x509_ = DotNetUtilities.ToX509Certificate(certificate.CertificateStructure);

         //File.WriteAllBytes(@"D:\\test.crt",   x509_.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12));

  
        }
    }
}
/*
TODO : 
 

*/