namespace Bars.GkhGji.Regions.Chelyabinsk.Services.Impl
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPredLicIP;
    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPredLicUL;
    using System.Xml;
    using Bars.GkhGji.Regions.Chelyabinsk.smevHistoryServiceV2;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    // TODO: wcf
   // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
   // [ServiceBehavior(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiPredLicIP")]
    public class pmvGZHIgzhiPredLicIP : IPredLicIP
    {        
        [XmlSerializerFormat]
        public createSmvGzhiPredLicIPResponse1 createSmvGzhiPredLicIP(createSmvGzhiPredLicIPRequest1 request)
        {
            var Container = ApplicationContext.Current.Container;
            var fileMan = Container.Resolve<IFileManager>();
            string header = GetHeader("UidHeader", "http://sys.smev.ru/xsd/uidh");

            // smevHistory
            var service = new smevHistoryServiceV2();
            string smevHistoryInnerId = Guid.NewGuid().ToString();
            string smevHistoryActionCode = "gzhiPredLicIP";
            SmevHistory smevHistory = new SmevHistory
            {
                ActionCode = smevHistoryActionCode,
                LicenseRequestType = LicenseRequestType.GrantingLicense,
                InnerId = smevHistoryInnerId
            };

            var requestData = request.createSmvGzhiPredLicIPRequest.smvGzhiPredLicIP;

            //Prepare response
            var retVal = new createSmvGzhiPredLicIPResponse1();
            var resp = new createSmvGzhiPredLicIPResponse();
            resp.smvGzhiPredLicIP = requestData;
            retVal.createSmvGzhiPredLicIPResponse = resp;

            ManOrgLicenseRequest molRequest = new ManOrgLicenseRequest();
            var molRequestDomain = Container.ResolveDomain<ManOrgLicenseRequest>();
            var ContragentDomain = Container.ResolveDomain<Contragent>();
            var provDocDomain = Container.ResolveDomain<LicenseProvidedDoc>();
            var orgFormDomain = Container.ResolveDomain<OrganizationForm>();
            var manOrgRequestProvDocDomain = Container.ResolveDomain<ManOrgRequestProvDoc>();
            var smevHistoryDomain = Container.ResolveDomain<SmevHistory>();
            var StateDomain = Container.ResolveDomain<State>();

            var orgFrom = orgFormDomain.GetAll().FirstOrDefault(x => x.Code == "91"); //ИП
            molRequest.Contragent = ContragentDomain.GetAll().FirstOrDefault(x => x.Ogrn == requestData.G2Ogrn);
            if (molRequest.Contragent == null)
            {
                var newContragent = new Contragent
                {
                    //ExportId = null,
                    //Parent = null,
                    //Municipality = null,
                    //MoSettlement = null,
                    Name = string.IsNullOrEmpty(requestData.G1LastName)
                        ? requestData.G1Family + " " + requestData.G1Name
                        : requestData.G1Family + " " + requestData.G1Name + " " + requestData.G1LastName,

                    //FiasFactAddress = null,
                    //FiasJuridicalAddress = null,
                    //FiasMailingAddress = null,
                    //FiasOutsideSubjectAddress = null,

                    //AddressOutsideSubject = null,
                    FactAddress = null,
                    JuridicalAddress = null,
                    //MailingAddress = null,
                    //DateTermination = null,
                    //Description = null,
                    Email = requestData.G1Email,
                    Inn = requestData.G3Inn,
                    //Kpp = null,
                    //IsSite = null,
                    //OfficialWebsite = null,
                    Ogrn = requestData.G2Ogrn,
                    OgrnRegistration = requestData.G2Org,
                    EgrulExcNumber = requestData.G2Number,
                    EgrulExcDate = requestData.G2Date,
                    OrganizationForm = orgFrom,
                    Phone = requestData.G1Phone,
                    //Fax = null,
                    //PhoneDispatchService = null,
                    ContragentState = ContragentState.Active,
                    //SubscriberBox = null,
                    //TweeterAccount = null,
                    //FrguRegNumber = null,
                    //FrguOrgNumber = null,
                    //FrguServiceNumber = null,
                    //YearRegistration = null,
                    //DateRegistration = null,

                    //ActivityDateStart = null,
                    //ActivityDateEnd = null,
                    //ActivityDescription = null,
                    //ActivityGroundsTermination = null,
                    //Okpo = null,
                    //Okved = null,
                    TypeEntrepreneurship = TypeEntrepreneurship.NotSet,

                    //NameGenitive = null,
                    //NameDative = null,
                    //NameAccusative = null,
                    //NameAblative = null,
                    //NamePrepositional = null,

                    //Okato = null,
                    //Oktmo = null,
                    //TaxRegistrationSeries = null,
                    //TaxRegistrationNumber = null,
                    //TaxRegistrationIssuedBy = null,
                    //TaxRegistrationDate = null,
                    //RegDateInSocialUse = null,
                    //LicenseDateReceipt = null,
                    //ProviderCode = null,
                    //TimeZoneType = null,
                    //Okogu = null,
                    //Okfs = null
                };
                ContragentDomain.Save(newContragent);
                molRequest.Contragent = newContragent;
            }

            molRequest.Contragent.TaxRegistrationNumber = requestData.G3Number;
            molRequest.Contragent.TaxRegistrationDate = requestData.G3Date2;
            molRequest.Contragent.TaxRegistrationIssuedBy = requestData.G3Org;
            molRequest.Contragent.OgrnRegistration = requestData.G2Org;

            molRequest.DateRequest = DateTime.Now;
            molRequest.RegisterNum = GetNewNumber();
            molRequest.RegisterNumber = molRequest.RegisterNum + "рпгу";
            if (!string.IsNullOrEmpty(requestData.G5FileName) && requestData.G5File != null)
            {
                if (!string.IsNullOrEmpty(requestData.G5Doc) || !string.IsNullOrEmpty(requestData.G5DocNumber))
                {
                    molRequest.ConfirmationOfDuty = requestData.G5Doc + " № " + requestData.G5DocNumber;
                }
                else
                {
                    molRequest.ConfirmationOfDuty = "Вложение со сканом квитанции " + requestData.G5FileName;
                }
            }
            else
            {
                molRequest.ConfirmationOfDuty = "Оплата пошлины не подтверждена";
            }
           
            molRequest.ReasonOffers = null;
            molRequest.SendMethod = Gkh.Enums.Licensing.SendMethod.Portal;
            molRequest.ReasonRefusal = null;
            var smevState = StateDomain.GetAll()
                    .Where(x => x.TypeId == "gkh_manorg_license_request" && x.Code == "Получено с портала госуслуг").FirstOrDefault();
            if (smevState != null)
            {
                molRequest.State = smevState;
            }
            else
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                try
                {
                    stateProvider.SetDefaultState(molRequest);
                }
                finally
                {
                    Container.Release(stateProvider);
                }
            }
           // molRequest.State = GetBeginState();
            molRequest.Type = LicenseRequestType.GrantingLicense;
            molRequest.Note = header;
            molRequest.ManOrgLicense = null;
            molRequest.Applicant = null;
            molRequest.TaxSum = 0;
            molRequest.LicenseRegistrationReason = null;
            molRequest.LicenseRejectReason = null;
            molRequest.NoticeAcceptanceDate = null;
            molRequest.NoticeViolationDate = null;
            molRequest.ReviewDate = null;
            molRequest.NoticeReturnDate = null;
            molRequest.ReviewDateLk = null;
            molRequest.PreparationOfferDate = null;
            molRequest.SendResultDate = null;
            molRequest.SendMethod = null;
            molRequest.OrderDate = null;
            molRequest.OrderNumber = null;
            molRequest.OrderFile = null;
            molRequest.TypeIdentityDocument = null;

            molRequest.IdSerial = null;
            molRequest.IdNumber = null;
            molRequest.IdIssuedBy = null;
            molRequest.IdIssuedDate = null;

            molRequestDomain.Save(molRequest);

            /*Сохранение файлов*/

            //Скан образ документа подтверждающего уплату государственной пошлины
            if (requestData.G5FileName != null && requestData.G5File != null)
            {
                var g5File = fileMan.SaveFile(requestData.G5FileName, requestData.G5File);
                ManOrgRequestProvDoc molDocG5 = new ManOrgRequestProvDoc();
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                    .Where(x => x.Code == "10").FirstOrDefault();
                molDocG5.LicProvidedDoc = lcp;
                molDocG5.File = g5File;
                molDocG5.Date = DateTime.Now;
                molDocG5.ObjectCreateDate = DateTime.Now;
                molDocG5.ObjectEditDate = DateTime.Now;
                molDocG5.ObjectVersion = 333;
                molDocG5.LicRequest = molRequest;
                manOrgRequestProvDocDomain.Save(molDocG5);

            }

            //Копии учредительных документов юридического лица(Устав и  изменения, внесенные в Устав), засвидетельствованные в нотариальном порядке
            if (requestData.gzhiDoc1Name != null && requestData.gzhiDoc1 != null)
            {
                var doc1 = fileMan.SaveFile(requestData.gzhiDoc1Name, requestData.gzhiDoc1);
                ManOrgRequestProvDoc molDoc1 = new ManOrgRequestProvDoc();
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                 .Where(x => x.Code == "1").FirstOrDefault();
                molDoc1.LicProvidedDoc = lcp;
                molDoc1.File = doc1;
                molDoc1.Date = DateTime.Now;
                molDoc1.ObjectCreateDate = DateTime.Now;
                molDoc1.ObjectEditDate = DateTime.Now;
                molDoc1.ObjectVersion = 333;
                molDoc1.LicRequest = molRequest;
                manOrgRequestProvDocDomain.Save(molDoc1);
            }

            //Копия квалификационного аттестата должностного лица соискателя лицензии

            if (requestData.gzhiDoc2Name != null && requestData.gzhiDoc2 != null)
            {
                var doc2 = fileMan.SaveFile(requestData.gzhiDoc2Name, requestData.gzhiDoc2);
                ManOrgRequestProvDoc molDoc2 = new ManOrgRequestProvDoc();
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
               .Where(x => x.Code == "3").FirstOrDefault();
                molDoc2.LicProvidedDoc = lcp;
                molDoc2.File = doc2;
                molDoc2.Date = DateTime.Now;
                molDoc2.ObjectCreateDate = DateTime.Now;
                molDoc2.ObjectEditDate = DateTime.Now;
                molDoc2.ObjectVersion = 333;
                molDoc2.LicRequest = molRequest;
                manOrgRequestProvDocDomain.Save(molDoc2);
            }

            //Копия приказа о назначении или избрании на должность должностного лица соискателя лицензии
            if (requestData.gzhiDoc3Name != null && requestData.gzhiDoc3 != null)
            {
                var doc3 = fileMan.SaveFile(requestData.gzhiDoc3Name, requestData.gzhiDoc3);
                ManOrgRequestProvDoc molDoc3 = new ManOrgRequestProvDoc();
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                .Where(x => x.Code == "14").FirstOrDefault();
                molDoc3.LicProvidedDoc = lcp;
                molDoc3.Date = DateTime.Now;
                molDoc3.ObjectCreateDate = DateTime.Now;
                molDoc3.ObjectEditDate = DateTime.Now;
                molDoc3.ObjectVersion = 333;
                molDoc3.LicRequest = molRequest;              
                molDoc3.File = doc3;
                molDoc3.LicRequest = molRequest;
                manOrgRequestProvDocDomain.Save(molDoc3);
            }

            //Опись прилагаемых документов
            if (requestData.gzhiDoc5Name != null && requestData.gzhiDoc5 != null)
            {
                var doc5 = fileMan.SaveFile(requestData.gzhiDoc5Name, requestData.gzhiDoc5);
                ManOrgRequestProvDoc molDoc5 = new ManOrgRequestProvDoc();
                molDoc5.File = doc5;
                molDoc5.LicRequest = molRequest;
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
               .Where(x => x.Code == "4").FirstOrDefault();
                molDoc5.LicProvidedDoc = lcp;
                molDoc5.Date = DateTime.Now;
                molDoc5.ObjectCreateDate = DateTime.Now;
                molDoc5.ObjectEditDate = DateTime.Now;
                molDoc5.ObjectVersion = 333;
                molDoc5.LicRequest = molRequest;
                molDoc5.LicRequest = molRequest;
                manOrgRequestProvDocDomain.Save(molDoc5);
            }

            //Копия паспорта должностного лица организации (директора)
            if (requestData.gzhiDoc25Name != null && requestData.gzhiDoc25 != null)
            {
                var doc25 = fileMan.SaveFile(requestData.gzhiDoc25Name, requestData.gzhiDoc25);
                ManOrgRequestProvDoc molDoc25 = new ManOrgRequestProvDoc();
                molDoc25.File = doc25;
                molDoc25.LicRequest = molRequest;
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                .Where(x => x.Code == "13").FirstOrDefault();
                molDoc25.LicProvidedDoc = lcp;
                molDoc25.Date = DateTime.Now;
                molDoc25.ObjectCreateDate = DateTime.Now;
                molDoc25.ObjectEditDate = DateTime.Now;
                molDoc25.ObjectVersion = 333;
                molDoc25.LicRequest = molRequest;
                molDoc25.LicRequest = molRequest;
                manOrgRequestProvDocDomain.Save(molDoc25);
            }
            
            //    molRequestDomain.Save(molRequest);
            
            newActionRequest historyReq = new newActionRequest
            {
                actionCode = smevHistoryActionCode,
                applicantId = requestData.applicant,
                fromIS = "lk",
                rguId = "7400000000163262794",
                rguTargetId = "7400000000163264107",
                name = "Получение лицензии на осуществление предпринимательской деятельности по управлению многоквартирными домами в Челябинской области",
                socId = requestData.login,
                //socId = "926705810004",
                status = "4",
                toIS = "ppu",
                recOrgCode = "GZHI",
                innerId = smevHistoryInnerId
            };
            try
            {
                newActionResponse historyResp = service.newAction(historyReq);

                smevHistory.Guid = historyResp.guid;
                smevHistory.UniqId = historyResp.uniqId;
                smevHistory.ExtActionId = historyResp.extActionId;
                smevHistory.Status = historyResp.statusId;
                smevHistory.SocId = historyResp.socId;
            }
            catch (Exception e)
            {
                string status = $"Ошибка: {e.Message}";
                smevHistory.SocId = requestData.login;
                smevHistory.UniqId = "-";
                smevHistory.Status = status.Length > 1000 ? status.Substring(0, 1000) : status;
            }
            smevHistoryDomain.Save(smevHistory);
            return retVal;
        }

        private int GetNewNumber()
        {
            var Container = ApplicationContext.Current.Container;
           var molRequestDomain = Container.ResolveDomain<ManOrgLicenseRequest>();
            var maxRequest = molRequestDomain.GetAll()
                  .OrderByDescending(x => x.RegisterNum).FirstOrDefault();
            int newRegisterNum = 0;
            if (maxRequest.RegisterNum.HasValue)
            {
                newRegisterNum = maxRequest.RegisterNum.Value + 1;
                return newRegisterNum;
            }
            else
            {
                var maxRequestById = molRequestDomain.GetAll()
                 .OrderByDescending(x => x.Id).FirstOrDefault();

                string s3 = maxRequestById.RegisterNumber;
                string result = "";
                char[] charsS3 = s3.ToCharArray();
                for (int i = 0; i < s3.Length; i++)
                {
                    if (char.IsDigit(charsS3[i]))
                    {
                        result = result + charsS3[i];

                    }
                }
                if (!string.IsNullOrEmpty(result))
                {
                    return Convert.ToInt32(result);
                }
                else
                    return 1;
                    
            }
            
        }

        private static string GetHeader(string name, string ns)
        {
            try
            {
                var v3 = OperationContext.Current.IncomingMessageHeaders.ToList();
                var v4 = OperationContext.Current.RequestContext.RequestMessage.ToString();
                var xDoc = new XmlDocument();
                xDoc.LoadXml(v4);
                var xRoot = xDoc.DocumentElement;
                var nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("uidh", "http://sys.smev.ru/xsd/uidh");
                var guid = xDoc.SelectSingleNode("//*/uidh:guid",nsm)?.InnerText;
                return guid;
                //return OperationContext.Current.IncomingMessageHeaders.FindHeader(name, ns) > -1
                //    ? OperationContext.Current.IncomingMessageHeaders.GetHeader<T>(name, ns)
                //    : default(T);
            }
            catch (Exception e)
            {             
                return "";
            }
        }
    }
}