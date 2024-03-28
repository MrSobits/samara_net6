using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using Bars.GkhGji.Regions.Habarovsk.TypeProperty;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVPropertyTypeService : ISMEVPropertyTypeService
    {
        #region Constants

        IWindsorContainer _container;

        #endregion

        #region Properties

        public IDomainService<SMEVPropertyType> SMEVPropertyTypeDomain { get; set; }

        public IDomainService<SMEVPropertyTypeFile> SMEVPropertyTypeFileDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }


        #endregion

        #region Fields

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVPropertyTypeService(IFileManager fileManager, IWindsorContainer container)
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
        public bool SendInformationRequest(SMEVPropertyType requestData, IProgressIndicator indicator = null)
        {
            try
            {
                ExcerptService excerptService = new ExcerptService();
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVPropertyTypeFileDomain.GetAll().Where(x => x.SMEVPropertyType == requestData).ToList().ForEach(x => SMEVPropertyTypeFileDomain.Delete(x.Id));

                //формируем отправляемую xml      
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Properties.Resources.PropTypeXml);
                var message = GetMessage(requestData);
                var messageata = GetMessageData(requestData);
                XElement messageElem = ToXElement(message);
                XElement messageataElem = ToXElement(messageata);
                doc.GetElementsByTagName("MessageEnvelopeOf_RegisterRequestAppData")[0].InnerXml = messageElem.ToString() + messageataElem.ToString();
                SaveRequestFile(doc, "SendRequestRequestUnsigned.dat", requestData);
                //Подписываем
                doc = SignWSSecurity(doc);

               
                //
               // MessageDataUsingRegisterRequestResponseAppData responce = null;
            
                try
                {
                    var responseMessage = SendSoapData(doc, false).GetAwaiter().GetResult();
                    SaveFile(requestData, responseMessage.sendedData, "SendRequestRequestAsync.dat");
                    SaveFile(requestData, responseMessage.receivedData, "SendRequestResponse.dat");
                }
                catch (Exception e)
                {
                    requestData.RequestState = RequestState.Error;
                    requestData.Attr3 = e.Message;
                    SMEVPropertyTypeDomain.Update(requestData);
                }                  
            }
            catch (HttpRequestException e)
            {
                //ошибки связи прокидываем в контроллер
                requestData.RequestState = RequestState.Error;
                requestData.Attr1 = e.Message;
                SMEVPropertyTypeDomain.Update(requestData);
                throw;
            }
            catch (Exception e)
            {
                requestData.RequestState = RequestState.Error;
                requestData.Attr2 = e.Message;
                SMEVPropertyTypeDomain.Update(requestData);
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
                          endpoint: new Uri("http://192.168.111.2/wsgwsoaphttp2/soaphttpengine/SEIProxy/ExcerptFF594Service/BasicHttpBinding_IExcerptService"),
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
            string thumbprint = config.GetAs("Thumbprint", "‎be6bfa913149e26cc4ce3e1531f9fcc547302f2e", true).Replace("\u200B", "");
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
            Reference reference = new Reference();
            reference.Uri = "#SIGNED_BY_CONSUMER";
           // reference.DigestMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411UrlObsolete;
            XmlDsigExcC14NTransform c14 = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14);
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
           // signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410UrlObsolete;
            AsymmetricAlgorithm key = Certificate.PrivateKey;
            XmlElement xmlDigitalSignature = SignXmlNode(doc, "#SIGNED_BY_CONSUMER", key, Certificate);
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignatureValue")[0], true));
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignedInfo")[0], true));
            doc.GetElementsByTagName("wsse:BinarySecurityToken")[0].InnerText =  Convert.ToBase64String(Certificate.RawData);

            signedXml.SigningKey = Certificate.PrivateKey;
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
          //  XmlDsigSmevTransform smev =
         //       new XmlDsigSmevTransform();
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
          //  signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3411_2012_256Url;


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
           //     signedXml.SignedInfo.SignatureMethod = CPSignedXml.XmlDsigGost3410_2012_256Url;
                signedXml.ComputeSignature();
            }

            // Получаем XML представление подписи и сохраняем его 
            // в отдельном node.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            return xmlDigitalSignature;

        }



        private Message GetMessage(SMEVPropertyType requestData)
        {
            Message message = new Message
            {
               Sender = new orgExternal
               {
                   Code = "TEST01001",
                   Name = "Государственная жилищная инспекция Воронежской области"
               },
               Recipient = new orgExternal
               {
                   Code = "104901361",
                   Name = "Департамент имущественных и земельных отношений Воронежской области"
               },
               Date = DateTime.Now,
               ExchangeType = 2,
               TypeCode = TypeCodeType.GSRV
            };
            return message;
        }

        private XElement ToXElement(Message message)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Message));
                    xmlSerializer.Serialize(streamWriter, message);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private XElement ToXElement(MessageDataUsingRegisterRequestAppData message)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(MessageDataUsingRegisterRequestAppData));
                    xmlSerializer.Serialize(streamWriter, message);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private MessageDataUsingRegisterRequestAppData GetMessageData(SMEVPropertyType requestData)
        {
            if (string.IsNullOrEmpty(requestData.CadastralNumber))
            {
                MessageDataUsingRegisterRequestAppData data = new MessageDataUsingRegisterRequestAppData
                {
                    AppData = new RegisterRequestAppData
                    {
                        DeclarantINN = "3664032439",
                        DeclarantName = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ",
                        DeclarantOGRN = "1033600084968",
                        IncomingDate = DateTime.Now,
                        IncomingNumber = requestData.Id.ToString(),
                        IsFullRequest = true,
                        PropertyType = requestData.Room != null ? PropertyType.Premise : PropertyType.Building,
                        PublicPropertyLevel = GetPropertyLevel(requestData.PublicPropertyLevel),
                        Query = GetAddressQuery(requestData.RealityObject, requestData.Room)
                    }
                };
                return data;
            }
            else
            {
                MessageDataUsingRegisterRequestAppData data = new MessageDataUsingRegisterRequestAppData
                {
                    AppData = new RegisterRequestAppData
                    {
                        DeclarantINN = "3664032439",
                        DeclarantName = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ",
                        DeclarantOGRN = "1033600084968",
                        IncomingDate = DateTime.Now,
                        IncomingNumber = requestData.Id.ToString(),
                        IsFullRequest = true,
                        PropertyType = requestData.Room != null ? PropertyType.Premise : PropertyType.Building,
                        PublicPropertyLevel = GetPropertyLevel(requestData.PublicPropertyLevel),
                        Query = GetCadastralQuery(requestData.CadastralNumber)
                    }
                };
                return data;
            }
        }

        private AddressQuery GetAddressQuery(RealityObject ro, Room rm)
        {
            AddressQuery aq = new AddressQuery
            {
                RequestCopyingNote = $"Просим предоставить сведения о собственности в отношении объекта по адресу объекта",
                Address = new Address
                {
                    ZipCode = ro.FiasAddress.PostCode,
                    Country = "Российская Федерация",
                    Region = "Воронежская область",
                    District = ro.Municipality.Name,
                    City = ro.MoSettlement == null? ro.Address.Split(',')[0].Trim():"",
                    Locality = ro.MoSettlement != null ? ro.MoSettlement.Name:"",
                    Street = ro.FiasAddress.StreetName,
                    House = ro.FiasAddress.House,
                    Build = ro.FiasAddress.Building,
                    Construct = ro.FiasAddress.Housing,
                    Flat = rm == null? "":rm.RoomNum
                }
            };
            return aq;
        }

        private CadasterNumberQuery GetCadastralQuery(string cadastralNumber)
        {
            CadasterNumberQuery aq = new CadasterNumberQuery
            {
                CadasterNumber = cadastralNumber,
                RequestCopyingNote = $"Просим предоставить сведения о собственности в отношении объекта по кадастровому номеру {cadastralNumber}"
            };
            return aq;
        }

        private TypeProperty.PublicPropertyLevel GetPropertyLevel(Enums.PublicPropertyLevel lvl)
        {
            switch (lvl)
            {

                case Enums.PublicPropertyLevel.Mun:
                    return TypeProperty.PublicPropertyLevel.M;

                case Enums.PublicPropertyLevel.Subj:
                    return TypeProperty.PublicPropertyLevel.S;

                default: return TypeProperty.PublicPropertyLevel.M;
            }
                 
        }

      

        private decimal NullableDecimalParse(string value)
        {
            if (value == null)
                return 0;

            decimal result;

            return (decimal.TryParse(value, out result) ? result : 0);
        }      

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        #endregion

        #region Private methods


        private void SaveFile(SMEVPropertyType request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVPropertyTypeFileDomain.Save(new SMEVPropertyTypeFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVPropertyType = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void ChangeState(SMEVEGRIP requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVPropertyTypeDomain.Update(requestData);
        }

        private void SetErrorState(SMEVEGRIP requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVPropertyTypeDomain.Update(requestData);
        }

        private void SaveRequestFile(XmlDocument doc, string fileName, SMEVPropertyType rt)
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
                SMEVPropertyTypeFileDomain.Save(new SMEVPropertyTypeFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVPropertyType = rt,
                    SMEVFileType = SMEVFileType.Request,
                    FileInfo = _fileManager.SaveFile(ms, fileName)
                });
            }
          
        }

       


        #endregion
    }
}
