namespace Bars.Gkh1468.Domain.PassportImport.Impl.DataProvider
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.Pkcs;
    using System.Xml;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Authentification;

    using Gkh.Domain.ArchieveReader;
    using Gkh.Import;
    using Interfaces;

    public class SignedArchiveDataProvider : IDynamicDataProvider
    {
        private BaseParams _baseParams;
        private ILogImport _logger;
        private IGkhUserManager _userManager;
        private bool _allowImportWoDigitSign;

        public SignedArchiveDataProvider(BaseParams baseParams, ILogImport logger, IGkhUserManager userManager, bool allowImportWoDigitSign)
        {
            _baseParams = baseParams;
            _logger = logger;
            _userManager = userManager;
            _allowImportWoDigitSign = allowImportWoDigitSign;
        }

        public object GetData()
        {
            return new XmlDocumentEnumerable(_baseParams, _logger, _userManager, _allowImportWoDigitSign);
        }
    }

    internal sealed class XmlDocumentEnumerable : IEnumerable<XmlDocument>
    {
        private BaseParams _baseParams;
        private ILogImport _logger;
        private IGkhUserManager _userManager;
        private bool _allowImportWoDigitSign;

        public XmlDocumentEnumerable(BaseParams baseParams, ILogImport logger, IGkhUserManager userManager, bool allowImportWoDigitSign)
        {
            _baseParams = baseParams;
            _logger = logger;
            _userManager = userManager;
            _allowImportWoDigitSign = allowImportWoDigitSign;
        }

        public IEnumerator<XmlDocument> GetEnumerator()
        {
            var files = _baseParams.Files.Where(x => x.Value != null).Select(x => x.Value);

            foreach (var file in files)
            {
                using (var str = new MemoryStream(file.Data))
                {
                    var archiveReader = ArchiveReaderFactory.Create(file.Extention);

                    var parts =
                        archiveReader.GetArchiveParts(str, file.FileName)
                                     .GroupBy(
                                         x =>
                                         Path.GetFileNameWithoutExtension(
                                             x.FileName.ToUpperInvariant().Replace(".XML.SIG", ".SIG")))
                                     .ToDictionary(x => x.Key, y => y);

                    foreach (var part in parts)
                    {
                        string content = GetDecryptedData(part.Value, file.FileName);

                        if (content.IsEmpty())
                        {
                            yield return null;
                        }
                        else
                        {
                            yield return WrapWithXmlDocument(content);
                        }
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private XmlDocument WrapWithXmlDocument(string content)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            return xmlDoc;
        }

        private string GetDecryptedData(IEnumerable<ArchivePart> archiveParts, string fileName)
        {
            var certificate = archiveParts.FirstOrDefault(x => x.FileName.ToUpperInvariant().EndsWith(".SIG"));

            if (!_allowImportWoDigitSign && certificate == null)
            {
                _logger.Error("", "Не удалось найти файл подписи в архиве: {0}".FormatUsing(fileName));
                return null;
            }

            var encryptedData = archiveParts.FirstOrDefault(x => !x.FileName.ToUpperInvariant().EndsWith(".SIG"));

            if (!_allowImportWoDigitSign)
            {
                using (var cert = certificate.StreamProvider.OpenStream())
                {
                    using (var file = encryptedData.StreamProvider.OpenStream())
                    {
                        if (!SignatureIsValid(cert, file))
                        {
                            _logger.Error(
                                "",
                                string.Format(
                                    "Подпись в архиве \"{0}\" для файла \"{1}\" некорректна",
                                    fileName,
                                    certificate.FileName));
                            return null;
                        }
                    }
                }
            }

            using (var stream = (MemoryStream)encryptedData.StreamProvider.OpenStream())
            {
                using (var ms = new MemoryStream(stream.ToArray()))
                {
                    using (var sr = new StreamReader(ms))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        private bool SignatureIsValid(Stream cert, Stream file)
        {
            cert.Seek(0, SeekOrigin.Begin);
            file.Seek(0, SeekOrigin.Begin);

            MemoryStream certStr = null;
            MemoryStream fileStr = null;

            var str = cert as MemoryStream;
            if (str != null)
            {
                certStr = str;
            }
            else
            {
                certStr = new MemoryStream();
                cert.CopyTo(certStr);
                certStr.Seek(0, SeekOrigin.Begin);
            }

            var stream = file as MemoryStream;
            if (stream != null)
            {
                fileStr = stream;
            }
            else
            {
                fileStr = new MemoryStream();
                file.CopyTo(fileStr);
                fileStr.Seek(0, SeekOrigin.Begin);
            }

            var contentInfo = new ContentInfo(fileStr.ToArray());
            var signedCms = new SignedCms(contentInfo, true);

            try
            {
                signedCms.Decode(certStr.ToArray());

                signedCms.CheckSignature(false);
                return true;
            }
            catch(Exception e)
            {
                _logger.Error("Ошибка проверки подписи", e.Message);
                return false;
            }
        }
    }
}