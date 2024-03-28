using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.GkhGji.Regions.Voronezh.SMEV2.Proxy.ProcessTask;
using Bars.GkhGji.Regions.Voronezh.TypeProperty;
using Castle.Windsor;
//using CryptoPro.Sharpei.Xml;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class SMEVValidPassportService : ISMEVValidPassportService
    {
        #region Constants

        IWindsorContainer _container;

        #endregion

        #region Properties

        public IDomainService<SMEVValidPassport> SMEVValidPassportDomain { get; set; }

        public IDomainService<SMEVValidPassportFile> SMEVValidPassportFileDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }


        #endregion

        #region Fields

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVValidPassportService(IFileManager fileManager, IWindsorContainer container)
        {
            _fileManager = fileManager;
            _container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса выписки ЕГРИП
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVValidPassport requestData, IProgressIndicator indicator = null)
        {
            try
            {
                ExcerptService excerptService = new ExcerptService();
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVValidPassportFileDomain.GetAll().Where(x => x.SMEVValidPassport == requestData).ToList().ForEach(x => SMEVValidPassportFileDomain.Delete(x.Id));

                //формируем отправляемую xml      
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Properties.Resources.PropTypeXml);
                var processTask = GetProcessTask(requestData);
                XElement processTaskElem = ToXElement(processTask);
                doc.GetElementsByTagName("soapenv:Body")[0].InnerXml = processTaskElem.ToString();
                SaveRequestFile(doc, "SendRequestRequestUnsigned.dat", requestData);
                //Подписываем
                doc = SignWSSecurity(doc);
                try
                {
                    var responseMessage = SendSoapData(doc, false).GetAwaiter().GetResult();
                    var result = responseMessage.SoapXML.Descendants()
                                    .Where(x => x.Name.LocalName == "parameters")
                                        .Select(x => new
                                        {
                                            Name = ((XElement)x.FirstNode).Value,
                                            Value = ((XElement)x.FirstNode.NextNode)?.Value
                                        })
                                    .GroupBy(x => x.Name)
                                    .ToDictionary(x => x.Key, y => y.First().Value);

                    if (result != null)
                    {
                        requestData.DocStatus = getDocStatus(result["DOC_STATUS"]);
                        requestData.InvalidityReason = result.ContainsKey("INVALIDITY_REASON") ? getInvalidReason(result["INVALIDITY_REASON"]) : string.Empty;
                        requestData.InvaliditySince = result.ContainsKey("INVALIDITY_SINCE") ? DateTime.Parse(result["INVALIDITY_SINCE"]) : default;
                        requestData.RequestState = RequestState.ResponseReceived;
                        SMEVValidPassportDomain.Update(requestData);
                    }
                    else
                    {
                        var errorDesc = responseMessage.SoapXML.Descendants().Where(x => x.Name.LocalName == "errorDescription").Select(x => x.Value).FirstOrDefault();
                        requestData.Answer = errorDesc;
                        requestData.RequestState = RequestState.ResponseReceived;
                        SMEVValidPassportDomain.Update(requestData);
                    }
                    SaveFile(requestData, responseMessage.sendedData, "SendResultRequestAsync.dat");
                    SaveFile(requestData, responseMessage.receivedData, "SendResultResponse.dat");
                }
                catch (Exception e)
                {
                    requestData.RequestState = RequestState.Error;
                    SMEVValidPassportDomain.Update(requestData);
                }                  
            }
            catch (HttpRequestException e)
            {
                //ошибки связи прокидываем в контроллер
                requestData.RequestState = RequestState.Error;
                SMEVValidPassportDomain.Update(requestData);
                throw;
            }
            catch (Exception e)
            {
                requestData.RequestState = RequestState.Error;
                SMEVValidPassportDomain.Update(requestData);
                throw;
                //  SaveException(requestData, e);
                // SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        private async Task<HTTPResponse> SendSoapData(XmlDocument doc, bool saveLog = false)
        {
            using (var soapClient = new SMEVPropertyTypeSoapClient())
            {
                return
                await soapClient.PostAsync(
                        endpoint: new Uri("http://pk1nlbsmev.k1.egov.local:80/ws/r36/SID0000113"),
                        soapVersion: SoapVersion.Soap11, //это прикол смэва, не ошибка
                        doc,
                        storeLog: saveLog, null
                        ).ConfigureAwait(false);
            }
        }

        private XmlDocument SignWSSecurity(XmlDocument doc)
        {
            SmevSignedXml signedXml = new SmevSignedXml(doc);

            var configProvider = _container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("SMEV3Library");
            string url = "http://192.168.111.2/wsgwsoaphttp2/soaphttpengine/SEIProxy/ExcerptFF594Service/BasicHttpBinding_IExcerptService";
            string url2 = config.GetAs("Endpoint", @"http://smev3-n0.test.gosuslugi.ru:7500/smev/v1.2/ws", true).Replace("\u200B", "");
            //   string thumbprint = config.GetAs("Thumbprint", "‎be6bfa913149e26cc4ce3e1531f9fcc547302f2e", true).Replace("\u200B", "");
            //  string thumbprint = config.GetAs("Thumbprint", "‎1dcd8000ce3ac6d37ae3a2ce298db472262638d8", true).Replace("\u200B", "");
            string thumbprint = "c7a8151337b106a2f2eabbdd992bcc86944e2b2c";
            string normalizedThumbprint = Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();
            StoreLocation storeLocation = StoreLocation.LocalMachine;
            string Storage = "MY";
            X509Store store = new X509Store(Storage, storeLocation);
            try
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка открытия хранилища: " + e.Message);
            }
            X509Certificate2Collection sertificates = store.Certificates.Find(X509FindType.FindByThumbprint, normalizedThumbprint, false);
            // Проверяем, что нашли ровно один сертификат.
            if (sertificates.Count == 0)
                throw new Exception("Ошибка открытия сертификата: cертификат не найден.");
            if (sertificates.Count > 1)
                throw new Exception("Ошибка открытия сертификата: найдено больше одного сертификата.");
            X509Certificate2 Certificate = sertificates[0];
            signedXml.SigningKey = Certificate.PrivateKey;
            Reference reference = new Reference();
            reference.Uri = "#SIGNED_BY_CONSUMER";

      //      reference.DigestMethod = CPSignedXml.XmlDsigGost3411UrlObsolete;
            XmlDsigExcC14NTransform c14 = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14);
            //    signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410UrlObsolete;
            //   signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410UrlObsolete;
       //     signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410_2012_256Url;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.AddReference(reference);
            AsymmetricAlgorithm key = Certificate.PrivateKey;
            try
            {
                signedXml.ComputeSignature();
            }
            catch (Exception e)
            {

            }

            XmlElement xmlDigitalSignature = signedXml.GetXml();
            //  XmlElement xmlDigitalSignature = SignXmlNode(doc, "#SIGNED_BY_CONSUMER", key, Certificate);
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignatureValue")[0], true));
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignedInfo")[0], true));
            doc.GetElementsByTagName("wsse:BinarySecurityToken")[0].InnerText = Convert.ToBase64String(Certificate.RawData);
            return doc;
        }
        static XmlElement SignXmlNode(XmlDocument Document, string SignedElement, AsymmetricAlgorithm Key, X509Certificate Certificate)
        {

            // Создаем объект SignedXml по XML документу.
            SmevSignedXml signedXml = new SmevSignedXml(Document);

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
            XmlDsigExcC14NTransform env = new XmlDsigExcC14NTransform();
            reference.AddTransform(env);

            // Добавляем СМЭВ трансформ.
            // начиная с .NET 4.5.1 для проверки подписи, необходимо добавить этот трансформ в довернные:
            // signedXml.SafeCanonicalizationMethods.Add("urn://smev-gov-ru/xmldsig/transform");
         //   XmlDsigSmevTransform smev =
       //         new XmlDsigSmevTransform();
       //     reference.AddTransform(smev);

            // Добавляем transform для канонизации.
            XmlDsigEnvelopedSignatureTransform c14 = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(c14);

            // Добавляем ссылку на подписываемые данные
            signedXml.AddReference(reference);

            // Создаем объект KeyInfo.
            KeyInfo keyInfo = new KeyInfo();

            // Добавляем сертификат в KeyInfo
            keyInfo.AddClause(new KeyInfoX509Data(Certificate));

            // Добавляем KeyInfo в SignedXml.
            signedXml.KeyInfo = keyInfo;

            // Можно явно проставить алгоритм подписи: ГОСТ Р 34.10.
            // Если сертификат ключа подписи ГОСТ Р 34.10
            // и алгоритм ключа подписи не задан, то будет использован
            // XmlDsigGost3410Url
            // для 12 госта
            //signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410UrlObsolete;
        //    signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3411_2012_256Url;


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
     //           signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410_2012_256Url;
                signedXml.ComputeSignature();
            }

            // Получаем XML представление подписи и сохраняем его 
            // в отдельном node.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            return xmlDigitalSignature;

        }

        private string getDocStatus(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case "300": return "Действителен";
                    case "301": return "Недействителен";
                    case "302": return "Сведениями по заданным реквизитам не располагаем";
                }
            }
            return string.Empty;
        }

        private string getInvalidReason(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case "601": return "Истек срок действия";
                    case "602": return "Заменен на новый";
                    case "603": return "Выдан с нарушением";
                    case "604": return "Числится в розыске";
                    case "605": return "Изъят, уничтожен";
                    case "606": return "В связи со смертью владельца";
                    case "607": return "Технический брак";
                    case "609": return "Утрачен";
                }
            }
            return string.Empty;
        }

        private processTask GetProcessTask(SMEVValidPassport requestData)
        {
            processTask message = new processTask
            {
                Message = new SMEV2.Proxy.ProcessTask.Message
                {
                    Sender = new MessageSender
                    {
                        Code = "360105",
                        Name = "Государственная жилищная инспекция Воронежской области"
                    },
                    Recipient = new MessageRecipient
                    {
                        Code = "FMS001001",
                        Name = "ФМС России"
                    },
                    Originator = new MessageOriginator
                    {
                        Code = "FMS001001",
                        Name = "ФМС России"
                    },
                    TypeCode = TypeCodeType.GSRV.ToString(),
                    Status = "REQUEST",
                    Date = DateTime.Now,
                    ExchangeType = "3",
                    ServiceCode = "10000001111",
                    CaseNumber = $"P001_{requestData.Id * 15}",
                },
                MessageData = new MessageData
                {
                    AppData = new MessageDataAppData
                    {
                        user = new user
                        {
                            organization = "R2000000000",
                            person = new userPerson
                            {
                                id = requestData.Inspector.Id.ToString(),
                                firstName = requestData.Inspector.Fio.Split(' ')[0],
                                secondName = requestData.Inspector.Fio.Split(' ')[1],
                                lastName = requestData.Inspector.Fio.Split(' ')[2]
                            }
                        },
                        serviceCode = "P001",
                        versionCode = "001",
                        parameters = new parameters[]
                        {
                            new parameters { name = "DOC_SERIE", value = requestData.DocSerie },
                            new parameters { name = "DOC_NUMBER", value = requestData.DocNumber },
                            new parameters { name = "DOC_ISSUEDATE", value = requestData.DocIssueDate.ToString() }
                        }

                    }
                }
            };
            return message;
        }

        private XElement ToXElement(processTask message)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(processTask));
                    xmlSerializer.Serialize(streamWriter, message);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        #endregion

        #region Private methods


        private void SaveFile(SMEVValidPassport request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVValidPassportFileDomain.Save(new SMEVValidPassportFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVValidPassport = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveRequestFile(XmlDocument doc, string fileName, SMEVValidPassport rt)
        {
            //сохраняем ошибку
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Indent = true;

                using (XmlWriter xw = XmlWriter.Create(ms, xws))
                {         
                    doc.WriteTo(xw);                  
                }
                SMEVValidPassportFileDomain.Save(new SMEVValidPassportFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVValidPassport = rt,
                    SMEVFileType = SMEVFileType.Request,
                    FileInfo = _fileManager.SaveFile(ms, fileName)
                });
            }
          
        }
        #endregion
    }
}
