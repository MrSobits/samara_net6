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
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Regions.Chelyabinsk.smevHistoryServiceV2;
    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPereofUL;

    
    // TODO: wcf
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    //[ServiceBehavior(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiPereofUL")]
    public class pmvGZHIgzhiPereofUL : IPereofUL
    {
        [XmlSerializerFormat]
        public createSmvGzhiPereofULResponse1 createSmvGzhiPereofUL(createSmvGzhiPereofULRequest1 request)
        {
            // smevHistory
            var service = new smevHistoryServiceV2();
            string smevHistoryInnerId = Guid.NewGuid().ToString();
            string smevHistoryActionCode = "gzhiPereofUL";

            SmevHistory smevHistory = new SmevHistory
            {
                ActionCode = smevHistoryActionCode,
                LicenseRequestType = LicenseRequestType.RenewalLicense,
                InnerId = smevHistoryInnerId
            };

            var requestData = request.createSmvGzhiPereofULRequest.smvGzhiPereofUL;

            //Prepare response
            var retVal = new createSmvGzhiPereofULResponse1();
            var resp = new createSmvGzhiPereofULResponse();
            resp.smvGzhiPereofUL = requestData;
            retVal.createSmvGzhiPereofULResponse = resp;

            var Container = ApplicationContext.Current.Container;
            var fileMan = Container.Resolve<IFileManager>();

            LicenseReissuance licReis = new LicenseReissuance();
            var licReisDomain = Container.ResolveDomain<LicenseReissuance>();
            var ContragentDomain = Container.ResolveDomain<Contragent>();
            var orgFormDomain = Container.ResolveDomain<OrganizationForm>();
            var manOrgRequestProvDocDomain = Container.ResolveDomain<ManOrgRequestProvDoc>();
            var provDocDomain = Container.ResolveDomain<LicenseProvidedDoc>();
            var reissProvDocDomain = Container.ResolveDomain<LicenseReissuanceProvDoc>();
            var smevHistoryDomain = Container.ResolveDomain<SmevHistory>();
            var ManOrgLicenseDomain = Container.ResolveDomain<ManOrgLicense>();
            var StateDomain = Container.ResolveDomain<State>();
            var orgFromDefault = orgFormDomain.GetAll().FirstOrDefault(x => x.Code == "65"); //ООО
            var orgFormRequest = orgFormDomain.GetAll().FirstOrDefault(x => x.Name.ToLower() == requestData.G1Opf.ToLower()); //ООО

            licReis.ManOrgLicense = ManOrgLicenseDomain.GetAll()
                .Where(x => x.LicNum.ToString() == requestData.G7Number && x.DateIssued == requestData.G7Date).FirstOrDefault(); // Ищшем лицензию по номеру и дате выдачи
            licReis.Contragent = ContragentDomain.GetAll().FirstOrDefault(x => x.Ogrn == requestData.G2Ogrn);
            if (licReis.Contragent == null)
            {
                var newContragent = new Contragent
                {

                    Name = string.IsNullOrEmpty(requestData.G1FullName)
                        ? requestData.G1FullName : requestData.G1ShortName,

                    ShortName = requestData.G1ShortName,
                    FactAddress = null,
                    JuridicalAddress = null,
                    Email = requestData.G1Email,
                    Inn = requestData.G3Inn,
                    Ogrn = requestData.G2Ogrn,
                    OgrnRegistration = requestData.G2Org,
                    EgrulExcNumber = requestData.G2Number,
                    EgrulExcDate = requestData.G2Date,
                    OrganizationForm = orgFormRequest != null ? orgFormRequest : orgFromDefault,
                    Phone = requestData.G1Phone,
                    ContragentState = ContragentState.Active,
                    TypeEntrepreneurship = TypeEntrepreneurship.NotSet,
                };
                ContragentDomain.Save(newContragent);
                licReis.Contragent = newContragent;
            }
            licReis.Contragent.TaxRegistrationNumber = requestData.G3Number;
            licReis.Contragent.TaxRegistrationDate = requestData.G3Date2;
            licReis.Contragent.TaxRegistrationIssuedBy = requestData.G3Org;
            licReis.Contragent.OgrnRegistration = requestData.G2Org;

            licReis.ReissuanceDate = DateTime.Now;
            licReis.RegisterNum = GetNewNumber();
            licReis.RegisterNumber = licReis.RegisterNum + "рпгу";
            if (!string.IsNullOrEmpty(requestData.G5FileName) && requestData.G5File != null)
            {
                if (!string.IsNullOrEmpty(requestData.G5Doc) || !string.IsNullOrEmpty(requestData.G5DocNumber))
                {
                    licReis.ConfirmationOfDuty = requestData.G5Doc + " № " + requestData.G5DocNumber;
                }
                else
                {
                    licReis.ConfirmationOfDuty = "Вложение со сканом квитанции " + requestData.G5FileName;
                }
            }
            else
            {
                licReis.ConfirmationOfDuty = "Оплата пошлины не подтверждена";
            }
            var smevState = StateDomain.GetAll()
                    .Where(x => x.TypeId == "gkh_manorg_license_reissuance" && x.Code == "Получено с портала госуслуг").FirstOrDefault();
            if (smevState != null)
            {
                licReis.State = smevState;
            }
            else
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                try
                {
                    stateProvider.SetDefaultState(licReis);
                }
                finally
                {
                    Container.Release(stateProvider);
                }
            }
            try
            {
                licReisDomain.Save(licReis);
                smevHistory.RequestId = licReis.Id;
            }
            catch (Exception e)
            {
                // smevHistory
                newActionRequest historyReqErr = new newActionRequest
                {
                    actionCode = smevHistoryActionCode,
                    applicantId = requestData.applicant,
                    fromIS = "lk",
                    rguId = "7400000000163262794",
                    rguTargetId = "7400000000163264107",
                    name = "Получение лицензии на осуществление предпринимательской деятельности по управлению многоквартирными домами в Челябинской области",
                    socId = requestData.login,
                    //socId = "926705810004",
                    status = "9",
                    toIS = "ppu",
                    recOrgCode = "GZHI",
                    innerId = smevHistoryInnerId
                };
                try
                {
                    newActionResponse historyResp = service.newAction(historyReqErr);

                    string status = $"{historyResp.statusId}; ошибка: {e.Message}";
                    smevHistory.Guid = historyResp.guid;
                    smevHistory.UniqId = historyResp.uniqId;
                    smevHistory.ExtActionId = historyResp.extActionId;
                    smevHistory.Status = status.Length > 1000 ? status.Substring(0, 1000) : status;
                    smevHistory.SocId = historyResp.socId;
                }
                catch (Exception ee)
                {
                    string status = $"Ошибка: {e.Message}; {ee.Message}";
                    smevHistory.SocId = requestData.login;
                    smevHistory.UniqId = "-";
                    smevHistory.Status = status.Length > 1000 ? status.Substring(0, 1000) : status ;
                }
                smevHistoryDomain.Save(smevHistory);
                return retVal;
            }

            /*Сохранение файлов*/

            //Скан образ документа подтверждающего уплату государственной пошлины
            if (requestData.G5FileName != null && requestData.G5File != null)
            {
                var g5File = fileMan.SaveFile(requestData.G5FileName, requestData.G5File);
                LicenseReissuanceProvDoc licReisDocG5 = new LicenseReissuanceProvDoc();
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                    .Where(x => x.Code == "10").FirstOrDefault();
                licReisDocG5.LicProvidedDoc = lcp;
                licReisDocG5.File = g5File;
                licReisDocG5.Date = DateTime.Now;
                licReisDocG5.ObjectCreateDate = DateTime.Now;
                licReisDocG5.ObjectEditDate = DateTime.Now;
                licReisDocG5.ObjectVersion = 333;
                licReisDocG5.LicenseReissuance = licReis;
                reissProvDocDomain.Save(licReisDocG5);

            }

            //Копия приказа о назначении или избрании на должность должностного лица соискателя лицензии
            if (requestData.gzhiDoc3Name != null && requestData.gzhiDoc3 != null)
            {
                var doc3 = fileMan.SaveFile(requestData.gzhiDoc3Name, requestData.gzhiDoc3);
                LicenseReissuanceProvDoc licReisDoc3 = new LicenseReissuanceProvDoc();
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                .Where(x => x.Code == "14").FirstOrDefault();
                licReisDoc3.LicProvidedDoc = lcp;
                licReisDoc3.Date = DateTime.Now;
                licReisDoc3.ObjectCreateDate = DateTime.Now;
                licReisDoc3.ObjectEditDate = DateTime.Now;
                licReisDoc3.ObjectVersion = 333;
                licReisDoc3.LicenseReissuance = licReis;
                licReisDoc3.File = doc3;
                reissProvDocDomain.Save(licReisDoc3);
            }

            //Опись прилагаемых документов
            if (requestData.gzhiDoc5Name != null && requestData.gzhiDoc5 != null)
            {
                var doc5 = fileMan.SaveFile(requestData.gzhiDoc5Name, requestData.gzhiDoc5);
                LicenseReissuanceProvDoc licReisDoc5 = new LicenseReissuanceProvDoc();
                licReisDoc5.File = doc5;
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                    .Where(x => x.Code == "4").FirstOrDefault();
                licReisDoc5.LicProvidedDoc = lcp;
                licReisDoc5.Date = DateTime.Now;
                licReisDoc5.ObjectCreateDate = DateTime.Now;
                licReisDoc5.ObjectEditDate = DateTime.Now;
                licReisDoc5.ObjectVersion = 333;
                licReisDoc5.LicenseReissuance = licReis;
                reissProvDocDomain.Save(licReisDoc5);
            }

            //Копия паспорта должностного лица организации (директора)
            if (requestData.gzhiDoc25Name != null && requestData.gzhiDoc25 != null)
            {
                var doc25 = fileMan.SaveFile(requestData.gzhiDoc25Name, requestData.gzhiDoc25);
                LicenseReissuanceProvDoc licReisDoc25 = new LicenseReissuanceProvDoc();
                licReisDoc25.File = doc25;
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                    .Where(x => x.Code == "13").FirstOrDefault();
                licReisDoc25.LicProvidedDoc = lcp;
                licReisDoc25.Date = DateTime.Now;
                licReisDoc25.ObjectCreateDate = DateTime.Now;
                licReisDoc25.ObjectEditDate = DateTime.Now;
                licReisDoc25.ObjectVersion = 333;
                licReisDoc25.LicenseReissuance = licReis;
                reissProvDocDomain.Save(licReisDoc25);
            }

            //Копия лицензии на осуществление предпринимательской деятельности по управлению многоквартирными домами
            if (requestData.gzhiDoc6Name != null && requestData.gzhiDoc6 != null)
            {
                var doc6 = fileMan.SaveFile(requestData.gzhiDoc6Name, requestData.gzhiDoc6);
                LicenseReissuanceProvDoc licReisDoc6 = new LicenseReissuanceProvDoc();
                licReisDoc6.File = doc6;
                LicenseProvidedDoc lcp = provDocDomain.GetAll()
                    .Where(x => x.Code == "15").FirstOrDefault();
                licReisDoc6.LicProvidedDoc = lcp;
                licReisDoc6.Date = DateTime.Now;
                licReisDoc6.ObjectCreateDate = DateTime.Now;
                licReisDoc6.ObjectEditDate = DateTime.Now;
                licReisDoc6.ObjectVersion = 333;
                licReisDoc6.LicenseReissuance = licReis;
                reissProvDocDomain.Save(licReisDoc6);
            }

            //  licReisDomain.Save(licReis);

            // smevHistory
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
            var licReisDomain = Container.ResolveDomain<LicenseReissuance>();
            var maxRequest = licReisDomain.GetAll()
                  .OrderByDescending(x => x.RegisterNum).FirstOrDefault();
            int newRegisterNum = 0;
            if (maxRequest.RegisterNum.HasValue)
            {
                newRegisterNum = maxRequest.RegisterNum.Value + 1;
                return newRegisterNum;
            }
            else
            {
                var maxRequestById = licReisDomain.GetAll()
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
    }
}