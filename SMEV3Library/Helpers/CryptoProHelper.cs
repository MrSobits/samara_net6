using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

using CryptoPro.Sharpei.Xml;
using System;
using System.Text.RegularExpressions;
using System.IO;
using CryptoPro.Sharpei;
using Bars.B4.Utils;
using System.Security.Cryptography.Pkcs;
using SMEV3Library.Entities;

namespace SMEV3Library.Helpers
{
    internal static class CryptoProHelper
    {
      //  internal static string Thumbprint = "f87605dc726b515a1e120bec36b9736212a5e1bb";
        internal static string Thumbprint = "0ffa5e66dd1518ddf858fe7edc70275bcc565200";
        internal static string ThumbprintPers = "e76296331971f66a745b5d79c04ee359e985984b";
        internal static string HeaderThumbprint = "0076b6cd388a6c5332e0588663c3ba76cd842daf";
        //
        internal static string Storage = "MY";
        internal static StoreLocation StoreLocation = StoreLocation.CurrentUser;

        internal static XmlElement SignPersonalSig(XmlDocument Document, string SignedElement)
        {
            //Формируем отпечаток в машинном формате
            string normalizedThumbprint = Regex.Replace(ThumbprintPers, @"[^\da-zA-z]", string.Empty).ToUpper();

            //Открытие хранилища
            X509Store store = new X509Store(Storage, StoreLocation);
            try
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка открытия хранилища: " + e.Message);
            }

            // Ищем сертификат для подписи.
            X509Certificate2Collection sertificates = store.Certificates.Find(X509FindType.FindByThumbprint, normalizedThumbprint, false);
            // Проверяем, что нашли ровно один сертификат.
            if (sertificates.Count == 0)
                throw new Exception("Ошибка открытия сертификата: cертификат не найден.");
            if (sertificates.Count > 1)
                throw new Exception("Ошибка открытия сертификата: найдено больше одного сертификата.");
            X509Certificate2 Certificate = sertificates[0];

            // Открываем ключ подписи.
            AsymmetricAlgorithm Key = Certificate.PrivateKey;

            // Подписываем созданный XML файл и сохраняем.
            return SignXmlNode(Document, SignedElement, Key, Certificate);
        }

        internal static XmlElement Sign(XmlDocument Document, string SignedElement)
        {
            //Формируем отпечаток в машинном формате
            string normalizedThumbprint = Regex.Replace(Thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();

            //Открытие хранилища
            X509Store store = new X509Store(Storage, StoreLocation);
            try
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка открытия хранилища: "+e.Message); 
            }

            // Ищем сертификат для подписи.
            X509Certificate2Collection sertificates = store.Certificates.Find(X509FindType.FindByThumbprint, normalizedThumbprint, false);
            // Проверяем, что нашли ровно один сертификат.
            if (sertificates.Count == 0)
                throw new Exception("Ошибка открытия сертификата: cертификат не найден.");
            if (sertificates.Count > 1)
                throw new Exception("Ошибка открытия сертификата: найдено больше одного сертификата.");
            X509Certificate2 Certificate = sertificates[0];

            // Открываем ключ подписи.
            AsymmetricAlgorithm Key = Certificate.PrivateKey;

            // Подписываем созданный XML файл и сохраняем.
            return SignXmlNode(Document, SignedElement, Key, Certificate);
        }

        internal static byte[] SignFileDetached2012256 (string filePath)
        {
            //Формируем отпечаток в машинном формате
            string normalizedThumbprint = Regex.Replace(HeaderThumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();

            //Открытие хранилища
            X509Store store = new X509Store(Storage, StoreLocation);
            try
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка открытия хранилища: " + e.Message);
            }

            // Ищем сертификат для подписи.
            X509Certificate2Collection sertificates = store.Certificates.Find(X509FindType.FindByThumbprint, normalizedThumbprint, false);
            // Проверяем, что нашли ровно один сертификат.
            if (sertificates.Count == 0)
                throw new Exception("Ошибка открытия сертификата: cертификат не найден.");
            if (sertificates.Count > 1)
                throw new Exception("Ошибка открытия сертификата: найдено больше одного сертификата.");
            X509Certificate2 Certificate = sertificates[0];

            //// Открываем ключ подписи.
            //AsymmetricAlgorithm Key = Certificate.PrivateKey;

            // Открываем для чтения:
            FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // create ContentInfo
            ContentInfo content = new ContentInfo(fs.ReadAllBytes());
            fs.Close();

            // SignedCms represents signed data
            SignedCms signedMessage = new SignedCms(content, true);

            // create a signer
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, Certificate);

            // sign the data
            signedMessage.ComputeSignature(signer, false);

            // create PKCS #7 byte array
            byte[] signedBytes = signedMessage.Encode();

            // return signed data
            return Convert.ToBase64String(signedBytes).ToUtf8Bytes();

            //// Объект, реализующий алгоритм вычисления подписи.
            //Gost3410_2012_256CryptoServiceProvider prov = (Gost3410_2012_256CryptoServiceProvider)Certificate.PrivateKey;

            //// Объект, реализующий алгритм хэширования.
            //Gost3411_2012_256CryptoServiceProvider GostHash = new Gost3411_2012_256CryptoServiceProvider();

            //// Вычисляем подпись для потока данных
            //var result = prov.SignData(fs, GostHash);
            //fs.Close();
            //var dsfgfd = Convert.ToBase64String(result);
            //return Convert.ToBase64String(result).ToUtf8Bytes();
        }

        internal static byte[] SignAttachment(FileAttachment file)
        {
          
            //Формируем отпечаток в машинном формате
            string normalizedThumbprint = Regex.Replace(HeaderThumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();

            //Открытие хранилища
            X509Store store = new X509Store(Storage, StoreLocation);
            try
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка открытия хранилища: " + e.Message);
            }

            // Ищем сертификат для подписи.
            X509Certificate2Collection sertificates = store.Certificates.Find(X509FindType.FindByThumbprint, normalizedThumbprint, false);
            // Проверяем, что нашли ровно один сертификат.
            if (sertificates.Count == 0)
                throw new Exception("Ошибка открытия сертификата: cертификат не найден.");
            if (sertificates.Count > 1)
                throw new Exception("Ошибка открытия сертификата: найдено больше одного сертификата.");
            X509Certificate2 Certificate = sertificates[0];

            byte[] fileContent = file.FileData;
            byte[] hash = Gost3411_2012_256.Create().ComputeHash(fileContent);
            ContentInfo cInfo = new ContentInfo(fileContent);
            SignedCms signedMessage = new SignedCms(cInfo, true);

            // create a signer
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, Certificate);

            // sign the data
            signedMessage.ComputeSignature(signer, false);

            // create PKCS #7 byte array
            byte[] signedBytes = signedMessage.Encode();

            // return signed data
            return signedBytes;

        }

        static XmlElement SignXmlNode(XmlDocument Document, string SignedElement, AsymmetricAlgorithm Key, X509Certificate Certificate)
        {

            // Создаем объект SignedXml по XML документу.
            SignedXml signedXml = new SignedXml(Document);

            // Добавляем ключ в SignedXml документ. 
            signedXml.SigningKey = Key;

            // Создаем ссылку на node для подписи.
            Reference reference = new Reference(SignedElement);

            // Явно проставляем алгоритм хеширования,
            // по умолчанию SHA1.
            reference.DigestMethod = "http://www.w3.org/2001/04/xmldsig-more#gostr3411";
            //2012 ГОСТ
           // reference.DigestMethod = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256";

            // Добавляем transform на подписываемые данные
            // для удаления вложенной подписи.
            XmlDsigExcC14NTransform env =  new XmlDsigExcC14NTransform();
            reference.AddTransform(env);

            // Добавляем СМЭВ трансформ.
            // начиная с .NET 4.5.1 для проверки подписи, необходимо добавить этот трансформ в довернные:
            // signedXml.SafeCanonicalizationMethods.Add("urn://smev-gov-ru/xmldsig/transform");
            XmlDsigSmevTransform smev =
                new XmlDsigSmevTransform();
            reference.AddTransform(smev);

            // Добавляем transform для канонизации.
            XmlDsigEnvelopedSignatureTransform  c14 = new XmlDsigEnvelopedSignatureTransform ();
            reference.AddTransform(c14);

            // Добавляем ссылку на подписываемые данные
            signedXml.AddReference(reference);

            // Создаем объект KeyInfo.
            KeyInfo keyInfo = new KeyInfo();

            // Добавляем сертификат в KeyInfo
            keyInfo.AddClause(new KeyInfoX509Data(Certificate));

            // Добавляем KeyInfo в SignedXml.
            signedXml.KeyInfo = keyInfo;

            //var transform = new XmlDsigExcC14NTransform();
            //transform.LoadInput(Document);
            //var output = (XmlDocument)transform.GetOutput(typeof(XmlDocument));
            // Можно явно проставить алгоритм подписи: ГОСТ Р 34.10.
            // Если сертификат ключа подписи ГОСТ Р 34.10
            // и алгоритм ключа подписи не задан, то будет использован
            // XmlDsigGost3410Url
            // для 12 госта
            //signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410UrlObsolete;
            signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3411_2012_256Url;


            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            signedXml.SafeCanonicalizationMethods.Add("urn://smev-gov-ru/xmldsig/transform");

            // Вычисляем подпись.
            try
            {
                signedXml.ComputeSignature();
            }
            catch (Exception e)
            {
                //urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256
                signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410_2012_256Url;
                signedXml.ComputeSignature();
            }

            // Получаем XML представление подписи и сохраняем его 
            // в отдельном node.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            return xmlDigitalSignature;

        }
    }
}
