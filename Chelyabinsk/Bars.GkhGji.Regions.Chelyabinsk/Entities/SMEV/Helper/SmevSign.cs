using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CryptoPro.Sharpei.Xml;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities.SMEV
{
    public static class SmevSign
    {
       // private const String certName = "Главное управление \"\"ГЖИ Челябинской области\"\"";
        private const String certName = "Государственная жилищная инспекция Самарской области";
        public static X509Certificate2 GetCertificateFromStore()
        {
            // Get the certificate store for the current user.
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                // Place all certificates in an X509Certificate2Collection object.
                X509Certificate2Collection certCollection = store.Certificates;
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                //X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySerialNumber, certName, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            finally
            {
                store.Close();
            }

        }

        public static XmlElement Sign(XmlDocument xmlDocument, X509Certificate2 certificate, string[] references)
        {
            xmlDocument.PreserveWhitespace = true;

            var signedXml = new SignedXml(xmlDocument)
            {
                SigningKey = certificate.PrivateKey
            };
            signedXml.SafeCanonicalizationMethods.Add("urn://smev-gov-ru/xmldsig/transform");

            foreach (var referenceUri in references)
            {
                var reference = new Reference();
                reference.Uri = "#" + referenceUri;
                reference.DigestMethod = CPSignedXml.XmlDsigGost3411UrlObsolete;

                var c14 = new XmlDsigExcC14NTransform();
                reference.AddTransform(c14);

                var smev = new XmlDsigSmevTransform();
                reference.AddTransform(smev);

                signedXml.AddReference(reference);
            }
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.KeyInfo = keyInfo;

            ////"http://www.w3.org/2001/10/xml-exc-c14n#"
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410UrlObsolete;

            //Вычисляем сигнатуру
            signedXml.ComputeSignature();
            if (!signedXml.CheckSignature())
                throw new ApplicationException("Неверная подпись!!!");
            else
            {
                string str = "";
            }

            //Формируем Xml сигнатуры
            var xmlDigitalSignature = signedXml.GetXml();

            return xmlDigitalSignature;
        }

        public static XmlDocument SignXmlFile(XmlDocument doc, X509Certificate2 Certificate)
        {
            // Создаём объект SmevSignedXml - наследник класса SignedXml с перегруженным GetIdElement
            // для корректной обработки атрибута wsu:Id. 
            SmevSignedXml signedXml = new SmevSignedXml(doc);

            // Задаём ключ подписи для документа SmevSignedXml.
            signedXml.SigningKey = Certificate.PrivateKey;

            // Создаем ссылку на подписываемый узел XML. В данном примере и в методических
            // рекомендациях СМЭВ подписываемый узел soapenv:Body помечен идентификатором "body".
            Reference reference = new Reference();
            reference.Uri = "#body";

            // Задаём алгоритм хэширования подписываемого узла - ГОСТ Р 34.11-94. Необходимо
            // использовать устаревший идентификатор данного алгоритма, т.к. именно такой
            // идентификатор используется в СМЭВ.
#pragma warning disable 612
            //warning CS0612: 'CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411UrlObsolete' is obsolete
            reference.DigestMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411UrlObsolete;
#pragma warning restore 612

            // Добавляем преобразование для приведения подписываемого узла к каноническому виду
            // по алгоритму http://www.w3.org/2001/10/xml-exc-c14n# в соответствии с методическими
            // рекомендациями СМЭВ.
            XmlDsigExcC14NTransform c14 = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14);

            // Добавляем ссылку на подписываемый узел.
            signedXml.AddReference(reference);

            // Задаём преобразование для приведения узла ds:SignedInfo к каноническому виду
            // по алгоритму http://www.w3.org/2001/10/xml-exc-c14n# в соответствии с методическими
            // рекомендациями СМЭВ.
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            // Задаём алгоритм подписи - ГОСТ Р 34.10-2001. Необходимо использовать устаревший
            // идентификатор данного алгоритма, т.к. именно такой идентификатор используется в
            // СМЭВ.
#pragma warning disable 612
            //warning CS0612: 'CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411UrlObsolete' is obsolete
            signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410UrlObsolete;
#pragma warning restore 612

            // Вычисляем подпись.
            signedXml.ComputeSignature();

            // Получаем представление подписи в виде XML.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Добавляем необходимые узлы подписи в исходный документ в заготовленное место.
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(
                doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignatureValue")[0], true));
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(
                doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignedInfo")[0], true));

            // Добавляем сертификат в исходный документ в заготовленный узел
            // wsse:BinarySecurityToken.
            doc.GetElementsByTagName("wsse:BinarySecurityToken")[0].InnerText =
                Convert.ToBase64String(Certificate.RawData);

            return doc;
        }
    }

    class SmevSignedXml : SignedXml
    {
        public const string WSSecurityWSSENamespaceUrl = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        public const string WSSecurityWSUNamespaceUrl = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

        public SmevSignedXml(XmlDocument document)
            : base(document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            XmlNameTable myXmlNameTable = new NameTable();
            XmlNamespaceManager myNamespacemanager = new XmlNamespaceManager(myXmlNameTable);
            myNamespacemanager.AddNamespace("wsu",
                    "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            XmlNodeList lst = document.SelectNodes("//*[@wsu:Id='" + idValue + "' or @wsu:ID='" + idValue +
                    "' or @wsu:ID='" + idValue + "']", myNamespacemanager);
            if (lst.Count != 1)
                return null;
            return (XmlElement)lst.Item(0);
        }
    }
}
