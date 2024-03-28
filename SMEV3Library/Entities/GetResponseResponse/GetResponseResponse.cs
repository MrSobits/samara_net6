using Microsoft.Win32;
using SMEV3Library.Namespaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;

namespace SMEV3Library.Entities.GetResponseResponse
{
    /// <summary>
    /// Ответ на GetResponseRequest
    /// </summary>
    public abstract class GetResponseResponse : SMEVResponse
    {
        public GetResponseResponse(HTTPResponse response)
        {
            Error = response.Error;
            SendedData = response.sendedData;
            ReceivedData = response.receivedData;
            Attachments = response.Attachments;
            CreatedTime = DateTime.Now;

            var fault = response.SoapXML?.Element(SoapNamespaces.SoapNamespace + "Fault");
            if (fault != null)
            {
                FaultXML = fault;
                isAnswerPresent = true;
            }
        }

        #region Fields

        public FsAttachmentProxy[] FSAttachmentsList;

        /// <summary>
        /// Содержит ли сообщение ответ?
        /// </summary>
        public bool isAnswerPresent = false;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string OriginalMessageId = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string ReferenceMessageID = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string To = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string ReplyTo = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement MessagePrimaryContent = null;
        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement FullMessageElement = null;


        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement RequestRejected = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement AsyncProcessingStatus = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement RequestStatus = null;

        /// <summary>
        /// Приаттаченные файлы
        /// </summary>
        public List<FileAttachment> Attachments = null;

        /// <summary>
        /// Время получения
        /// </summary>
        public DateTime CreatedTime;

        public void SetSignatureFileName(XElement xElement)
        {
            if (xElement == null)
                return;

            foreach (var header in xElement.Elements(SMEVNamespaces12.BasicNamespace + "AttachmentHeader"))
            {
                var fileName = header.Element(SMEVNamespaces12.BasicNamespace + "contentId")?.Value + ".sig";
                var href = header.Element(SMEVNamespaces12.BasicNamespace + "SignaturePKCS7")?.Element(SoapNamespaces.XopNamespace + "Include")?.Attribute("href")?.Value;
                var fileguid = href.Replace("cid:", "").Split('@')[0].ToLower();

                foreach (var file in Attachments.Where(x => x.FileGuid == fileguid))
                    file.FileName = fileName;
                
            }
        }

        public void SetSignatureFileNameSGIO(XElement xElement)
        {
            if (xElement == null)
                return;

            foreach (var header in xElement.Elements(SMEVNamespaces12.BasicNamespace + "AttachmentHeader"))
            {
                string subject = string.Empty;
                var memtype = header.Element(SMEVNamespaces12.BasicNamespace + "MimeType").Value;
                string ext = GetExtension(memtype);

                var fileGiuid = header.Element(SMEVNamespaces12.BasicNamespace + "contentId")?.Value;
                var fileName = header.Element(SMEVNamespaces12.BasicNamespace + "contentId")?.Value + "."+ ext;
                var signatureEl = header.Element(SMEVNamespaces12.BasicNamespace + "SignaturePKCS7");
                if (signatureEl != null && !string.IsNullOrEmpty(signatureEl.Value))
                {
                    try
                    {
                        var signature = signatureEl.Value;
                        var bytes = Convert.FromBase64String(signature);
                        SignedCms signedCms = new SignedCms();
                        signedCms.Decode(bytes);
                        if (signedCms.Certificates != null && signedCms.Certificates.Count > 0)
                        {
                            var cert = signedCms.Certificates[signedCms.Certificates.Count-1];
                            if (cert != null)
                            {
                                subject = $"Серийный номер: {cert.SerialNumber}, Владелец сертификата: {cert.Subject}";
                            }
                        }
                      
                    }
                    catch
                    { }
                }
       
                try
                {
                    if (Attachments == null)
                    {
                        Attachments = new List<FileAttachment>();
                    }
                    Attachments.Add(new FileAttachment { FileName = fileName, FileGuid = fileGiuid, SignerInfo = subject });
                }
                catch (Exception e)
                {
                    string str = e.Message;
                }
            }
        }

        public void SetFileName(XElement xElement)
        {
            if (xElement == null)
                return;

            foreach (var attachmentContent in xElement.Elements(SMEVNamespaces12.BasicNamespace + "AttachmentContent"))
            {
                var fileGuid = attachmentContent.Element(SMEVNamespaces12.BasicNamespace + "Id")?.Value;
                var href = attachmentContent.Element(SMEVNamespaces12.BasicNamespace + "Content").Value;
                byte[] newBytes = Convert.FromBase64String(href);


                foreach (var file in Attachments.Where(x => x.FileGuid == fileGuid))
                    file.FileData = newBytes;
            }
        }
        public void SetFileNameSGIO(XElement xElement)
        {
            if (xElement == null)
                return;

            foreach (var attachmentContent in xElement.Elements(SMEVNamespaces12.BasicNamespace + "AttachmentContent"))
            {
                var fileGuid = attachmentContent.Element(SMEVNamespaces12.BasicNamespace + "Id")?.Value;
                var href = attachmentContent.Element(SMEVNamespaces12.BasicNamespace + "Content").Value;
                byte[] newBytes = Convert.FromBase64String(href);


                foreach (var file in Attachments.Where(x => x.FileGuid == fileGuid))
                    file.FileData = newBytes;
            }
        }

        private string GetDefaultExtension(string mimeType)
        {
            string result;
            RegistryKey key;
            object value;

            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }
        private string GetExtension(string ext)
        {
            string registryExt = GetDefaultExtension(ext);
            if (!string.IsNullOrEmpty(registryExt))
            {
                return registryExt;
            }
          
            switch (ext)
            {
                case "application/octet-stream":
                        return "dat";

                case "application/pdf":
                    return "pdf";

                case "application/zip":
                    return "zip";

                case "application/msword":
                    return "docx";

                case "text/xml":
                    return "xml";

                case "image/bmp":
                    return "bmp";

                case "image/gif":
                    return "gif";

                case "image/jpeg":
                    return "jpeg";

                case "image/png":
                    return "png";

                case "image/tiff":
                    return "tiff";
            }    
            return "dat";
        }
        #endregion
    }
}
