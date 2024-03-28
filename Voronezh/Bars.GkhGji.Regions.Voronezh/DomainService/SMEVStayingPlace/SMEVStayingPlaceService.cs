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
using Bars.GkhGji.Regions.Voronezh.SMEV2.Proxy.BookRequest;
using Bars.GkhGji.Regions.Voronezh.SMEV2.Proxy.GetResult;
using Bars.GkhGji.Regions.Voronezh.TypeProperty;
using Castle.Windsor;
// TODO: Заменить
//using CryptoPro.Sharpei.Xml;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Message = Bars.GkhGji.Regions.Voronezh.SMEV2.Proxy.BookRequest.Message;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class SMEVStayingPlaceService : ISMEVStayingPlaceService
    {
        #region Constants

        IWindsorContainer _container;

        #endregion

        #region Properties

        public IDomainService<SMEVStayingPlace> SMEVStayingPlaceDomain { get; set; }

        public IDomainService<SMEVStayingPlaceFile> SMEVStayingPlaceFileDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }


        #endregion

        #region Fields

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVStayingPlaceService(IFileManager fileManager, IWindsorContainer container)
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
        public bool SendInformationRequest(SMEVStayingPlace requestData, IProgressIndicator indicator = null)
        {
            try
            {
                ExcerptService excerptService = new ExcerptService();
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVStayingPlaceFileDomain.GetAll().Where(x => x.SMEVStayingPlace == requestData).ToList().ForEach(x => SMEVStayingPlaceFileDomain.Delete(x.Id));

                //формируем отправляемую xml      
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Properties.Resources.PropTypeXml);
                var bookRequest = GetBookRequest(requestData);
                XElement bookRequestElem = ToXElement(bookRequest);
                doc.GetElementsByTagName("soapenv:Body")[0].InnerXml = bookRequestElem.ToString();
                SaveRequestFile(doc, "SendRequestRequestUnsigned.dat", requestData);
                //Подписываем
                doc = SignWSSecurity(doc);

                try
                {
                    var responseMessage = SendSoapData(doc, false).GetAwaiter().GetResult();
                    var taskId = responseMessage.SoapXML.Descendants().Where(x => x.Name.LocalName == "taskId").Select(x => x.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(taskId))
                    {
                        requestData.TaskId = taskId;
                        requestData.RequestState = RequestState.Queued;
                        SMEVStayingPlaceDomain.Update(requestData);
                    }
                    else
                    {
                        var errorDesc = responseMessage.SoapXML.Descendants().Where(x => x.Name.LocalName == "errorDescription").Select(x => x.Value).FirstOrDefault();
                        requestData.Answer = errorDesc;
                        requestData.RequestState = RequestState.Error;
                        SMEVStayingPlaceDomain.Update(requestData);
                    }
                    SaveFile(requestData, responseMessage.sendedData, "SendRequestRequestAsync.dat");
                    SaveFile(requestData, responseMessage.receivedData, "SendRequestResponse.dat");
                }
                catch (Exception e)
                {
                    requestData.Answer = e.Message;
                    requestData.RequestState = RequestState.Error;
                    SMEVStayingPlaceDomain.Update(requestData);
                }
            }
            catch (HttpRequestException e)
            {
                //ошибки связи прокидываем в контроллер
                requestData.RequestState = RequestState.Error;
                SMEVStayingPlaceDomain.Update(requestData);
                throw;
            }
            catch (Exception e)
            {
                requestData.RequestState = RequestState.Error;
                SMEVStayingPlaceDomain.Update(requestData);
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

       //     reference.DigestMethod = CPSignedXml.XmlDsigGost3411UrlObsolete;
            XmlDsigExcC14NTransform c14 = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14);
            //    signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410UrlObsolete;
            //   signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410UrlObsolete;
            
            
         //   signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410_2012_256Url;
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
            //reference.DigestMethod = "http://www.w3.org/2001/04/xmldsig-more#gostr3411";
            //2012 ГОСТ
             reference.DigestMethod = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256";

            // Добавляем transform на подписываемые данные
            // для удаления вложенной подписи.
            XmlDsigExcC14NTransform env = new XmlDsigExcC14NTransform();
            reference.AddTransform(env);

            // Добавляем СМЭВ трансформ.
            // начиная с .NET 4.5.1 для проверки подписи, необходимо добавить этот трансформ в довернные:
            // signedXml.SafeCanonicalizationMethods.Add("urn://smev-gov-ru/xmldsig/transform");
        //    XmlDsigSmevTransform smev =
        //        new XmlDsigSmevTransform();
          //  reference.AddTransform(smev);

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
           
            //signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3411_2012_256Url;


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
                
               // signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3411_2012_256Url;
                signedXml.ComputeSignature();
            }

            // Получаем XML представление подписи и сохраняем его 
            // в отдельном node.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            return xmlDigitalSignature;

        }


        public bool GetResult(SMEVStayingPlace requestData, IProgressIndicator indicator = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Properties.Resources.PropTypeXml);
            var getResult = GetGetResult(requestData);
            XElement getResultElem = ToXElement(getResult);
            doc.GetElementsByTagName("soapenv:Body")[0].InnerXml = getResultElem.ToString();
            SaveRequestFile(doc, "SendResultRequestUnsigned.dat", requestData);
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
                SaveFile(requestData, responseMessage.sendedData, "SendResultRequestAsync.dat");
                SaveFile(requestData, responseMessage.receivedData, "SendResultResponse.dat");

                if (result.Count > 0)
                {
                  //  requestData.Answer = getAnswer(result["REG_TYPE"]);
                    requestData.Answer = result["COMMENT"];
                    requestData.LPlaceRegion = result["LPLACE_REGION"];
                    requestData.LPlaceCity = result["LPLACE_CITY"];
                    requestData.LPlaceStreet = result["LPLACE_STREET"];
                    requestData.LPlaceHouse = result["LPLACE_HOUSE"];
                    requestData.LPlaceFlat = result["LPLACE_FLAT"];
                    requestData.RequestState = RequestState.ResponseReceived;
                    SMEVStayingPlaceDomain.Update(requestData);
                }
                else
                {
                    var errorDesc = responseMessage.SoapXML.Descendants().Where(x => x.Name.LocalName == "errorDescription").Select(x => x.Value).FirstOrDefault();
                    requestData.Answer = errorDesc;
                    requestData.RequestState = RequestState.Error;
                    SMEVStayingPlaceDomain.Update(requestData);
                }
               
            }
            catch (Exception e)
            {
                requestData.RequestState = RequestState.Error;
                requestData.Answer = e.Message + " " + e.StackTrace;
                SMEVStayingPlaceDomain.Update(requestData);
                return false;
            }

            return true;
        }

        private string getAnswer(string code)
        {
            return !string.IsNullOrEmpty(code) ?
                B4.Utils.EnumExtensions.GetDisplayName(((SMEVLivingPlaceHasReg)int.Parse(code))) :
                string.Empty;
        }

        private getResult GetGetResult(SMEVStayingPlace requestData)
        {
            getResult getResult = new getResult
            {
                Message = new SMEV2.Proxy.GetResult.Message
                {
                    Sender = new SMEV2.Proxy.GetResult.MessageSender
                    {
                        Code = "360105",
                        Name = "Государственная жилищная инспекция Воронежской области"
                    },
                    Recipient = new SMEV2.Proxy.GetResult.MessageRecipient
                    {
                        Code = "FMS001001",
                        Name = "ФМС России"
                    },
                    Originator = new SMEV2.Proxy.GetResult.MessageOriginator
                    {
                        Code = "FMS001001",
                        Name = "ФМС России"
                    },
                    TypeCode = TypeCodeType.GSRV.ToString(),
                    Status = "REQUEST",
                    Date = DateTime.Now,
                    ExchangeType = "3",
                    ServiceCode = "10000001111",
                    CaseNumber = $"R002_{requestData.Id * 15}",
                },
                MessageData = new SMEV2.Proxy.GetResult.MessageData
                {
                    AppData = new SMEV2.Proxy.GetResult.MessageDataAppData
                    {
                        user = new SMEV2.Proxy.GetResult.user
                        {
                            organization = "R2000000000",
                            person = new SMEV2.Proxy.GetResult.userPerson
                            {
                                id = requestData.Inspector.Id.ToString(),
                                firstName = requestData.Inspector.Fio.Split(' ')[0],
                                secondName = requestData.Inspector.Fio.Split(' ')[1],
                                lastName = requestData.Inspector.Fio.Split(' ')[2]
                            }
                        },
                        taskId = requestData.TaskId
                    }
                }
            };

            return getResult;
        }

        private bookRequest GetBookRequest(SMEVStayingPlace requestData)
        {
            bookRequest bookRequest = new bookRequest
            {
                Message = new SMEV2.Proxy.BookRequest.Message
                {
                    Sender = new SMEV2.Proxy.BookRequest.MessageSender
                    {
                        Code = "360105",
                        Name = "Государственная жилищная инспекция Воронежской области"
                    },
                    Recipient = new SMEV2.Proxy.BookRequest.MessageRecipient
                    {
                        Code = "FMS001001",
                        Name = "ФМС России"
                    },
                    Originator = new SMEV2.Proxy.BookRequest.MessageOriginator
                    {
                        Code = "FMS001001",
                        Name = "ФМС России"
                    },
                    TypeCode = TypeCodeType.GSRV.ToString(),
                    Status = "REQUEST",
                    Date = DateTime.Now,
                    ExchangeType = "3",
                    ServiceCode = "10000001111",
                    CaseNumber = $"R002_{requestData.Id * 15}",

                },
                MessageData = new SMEV2.Proxy.BookRequest.MessageData
                {
                    AppData = new SMEV2.Proxy.BookRequest.MessageDataAppData
                    {
                        user = new SMEV2.Proxy.BookRequest.user
                        {
                            organization = "R2000000000",
                            person = new SMEV2.Proxy.BookRequest.userPerson
                            {
                                id = requestData.Inspector.Id.ToString(),
                                firstName = requestData.Inspector.Fio.Split(' ')[0],
                                secondName = requestData.Inspector.Fio.Split(' ')[1],
                                lastName = requestData.Inspector.Fio.Split(' ')[2]
                            }
                        },
                        serviceCode = "R002",
                        versionCode = "003",
                        parameters = new parameters[]
                        {
                            new parameters { name = "CITIZEN_FIRSTNAME", value = requestData.CitizenFirstname },
                            new parameters { name = "CITIZEN_GIVENNAME", value = requestData.CitizenGivenname },
                            new parameters { name = "CITIZEN_LASTNAME", value = requestData.CitizenLastname },
                            new parameters { name = "CITIZEN_BIRTHDAY", value = requestData.CitizenBirthday.ToShortDateString() },
                            new parameters { name = "CITIZEN_SNILS", value = requestData.CitizenSnils },
                            new parameters { name = "DOC_TYPE", value = getDocType(requestData.DocType) },
                            new parameters { name = "DOC_SERIE", value = requestData.DocSerie},
                            new parameters { name = "DOC_NUMBER", value =  requestData.DocNumber },
                            new parameters { name = "DOC_ISSUEDATE", value = requestData.DocIssueDate.ToShortDateString() },
                            new parameters { name = "REGION_CODE", value = requestData.RegionCode }
                        }
                    }
                }
            };
            return bookRequest;
        }


        private string getDocType(SMEVStayingPlaceDocType docType)
        {
            switch (docType)
            {
                case SMEVStayingPlaceDocType.RFPassp:
                    return "01";
                case SMEVStayingPlaceDocType.Zagran:
                    return "02";
                case SMEVStayingPlaceDocType.Birth:
                    return "03";
            }
            return string.Empty;
        }

        private XElement ToXElement<T>(T request)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, request);
                    XElement el = XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                    return el;
                }
            }
        }

        private XElement GetMessageData(SMEVStayingPlace requestData)
        {
            var data = new XElement("MessageData",
                            new XElement("AppData",
                                new XElement("user",
                                    new XElement("organization", "R2000000000"),
                                    new XElement("person",
                                        new XElement("id", requestData.Inspector.Id),
                                        new XElement("firstName", requestData.Inspector.Fio.Split(' ')[0]),
                                        new XElement("secondName", requestData.Inspector.Fio.Split(' ')[1]),
                                        new XElement("lastName", requestData.Inspector.Fio.Split(' ')[2])
                                        )
                                    ),
                                new XElement("serviceCode", "R004"),
                                new XElement("versionCode", "001"),
                                new XElement("parameters",
                                    new XElement("name", "FOR_FIRSTNAME"),
                                    new XElement("value", requestData.CitizenFirstname)
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "FOR_GIVENNAME"),
                                    new XElement("value", requestData.CitizenGivenname)
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "FOR_LASTNAME"),
                                    new XElement("value", requestData.CitizenLastname)
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "DOC_TYPE"),
                                    new XElement("value", "01")
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "DOC_ID"),
                                    new XElement("value", requestData.DocSerie + requestData.DocNumber)
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "DOC_ISSUEDATE"),
                                    new XElement("value", requestData.DocIssueDate.ToShortDateString())
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "DOC_COUNTRY"),
                                    new XElement("value", requestData.DocCountry)
                                    ),
                                new XElement("parameters",
                                    new XElement("name", "REGION_CODE"),
                                    new XElement("value", requestData.RegionCode)
                                    )
                                )
                            );
            return data;
        }      

        #endregion

        #region Private methods


        private void SaveFile(SMEVStayingPlace request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVStayingPlaceFileDomain.Save(new SMEVStayingPlaceFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVStayingPlace = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveRequestFile(XmlDocument doc, string fileName, SMEVStayingPlace rt)
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
                SMEVStayingPlaceFileDomain.Save(new SMEVStayingPlaceFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVStayingPlace = rt,
                    SMEVFileType = SMEVFileType.Request,
                    FileInfo = _fileManager.SaveFile(ms, fileName)
                });
            }
          
        }
        #endregion
    }
}
