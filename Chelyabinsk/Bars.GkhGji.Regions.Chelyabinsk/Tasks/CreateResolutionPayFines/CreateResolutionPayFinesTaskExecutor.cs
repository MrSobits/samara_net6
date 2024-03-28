using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bars.GkhGji.Enums;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.CreateResolutionPayFines
{
    public class CreateResolutionPayFinesTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private string operationlog { get; set; }

        public IDomainService<PayReg> PayRegDomain { get; set; }

        public IDomainService<GisGmp> GisGmpDomain { get; set; }

        public IDomainService<ResolutionPayFine> ResolutionPayFineDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<Protocol197> Protocol197Domain { get; set; }
        public IDomainService<ProtocolRSO> ProtocolRSODomain { get; set; }
        public IDomainService<ProtocolRSOArticleLaw> ProtocolRSOArticleLawDomain { get; set; }
        public IDomainService<ProtocolMhc> ProtocolMhcDomain { get; set; }
        public IDomainService<ResolPros> ResolProsDomain { get; set; }
        public IDomainService<ResolProsArticleLaw> ResolProsArticleLawDomain { get; set; }
        public IDomainService<ProtocolMhcArticleLaw> ProtocolMhcArticleLawDomain { get; set; }
        public IDomainService<Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
        public IDomainService<GISGMPPayerStatus> GISGMPPayerStatusDomain { get; set; }

        public IDomainService<ManOrgLicenseRequest> ManOrgLicenseRequestDomain { get; set; }

        public IDomainService<LicenseReissuance> LicenseReissuanceDomain { get; set; }

        public IDomainService<ManOrgRequestProvDoc> ManOrgRequestProvDocDomain { get; set; }

        public IDomainService<LicenseReissuanceProvDoc> LicenseReissuanceProvDocDomain { get; set; }

        public IDomainService<LicenseProvidedDoc> LicenseProvidedDocDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                CreateCalcRequests();
            }
            catch(Exception e)
            {
                var s = e.Message;
            }
            try
            {
                int num = 0;
                var gmpids = PayRegDomain.GetAll()
                    .Where(x=> x.GisGmp != null)
                    .Where(x => !x.IsPayFineAdded)
                    .Select(x => x.GisGmp.Id).Distinct().ToList();
                var gisGmps = GisGmpDomain.GetAll()
                    .Where(x=> gmpids.Contains(x.Id))
                    .Select(x=> new
                    {
                        x.Id,
                        x.TypeLicenseRequest,
                        Protocol = x.Protocol != null? new {x.Protocol.Id}: null,
                        ManOrgLicenseRequest = x.ManOrgLicenseRequest != null? new {x.ManOrgLicenseRequest.Id }:null,
                        LicenseReissuance = x.LicenseReissuance != null ? new { x.LicenseReissuance.Id } : null,

                    });
                foreach (var gisGmp in gisGmps)
                {
                    try
                    {
                        if (gisGmp.TypeLicenseRequest == Enums.TypeLicenseRequest.NotSet)
                        {
                            if (gisGmp.Protocol != null)
                            {
                                var payments = PayRegDomain.GetAll()
                                    .Where(x => x.GisGmp.Id == gisGmp.Id && !x.IsPayFineAdded);
                                if (payments.Any())
                                {
                                    var document = DocumentGjiDomain.GetAll()
                                        .Where(x => x.Id == gisGmp.Protocol.Id).FirstOrDefault();
                                    if (document.TypeDocumentGji == TypeDocumentGji.Protocol ||
                                        document.TypeDocumentGji == TypeDocumentGji.Protocol197 ||
                                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                                        document.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                                        document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                                        document.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                                    {
                                        foreach (var payment in payments)
                                        {
                                            var docChild = DocumentGjiChildrenDomain.GetAll()
                                                .Where(x => x.Parent == document).FirstOrDefault();
                                            if (docChild != null && !ResolutionPayFineDomain.GetAll().Where(x => x.DocumentNum == payment.PaymentId).Any())
                                            {
                                                ResolutionPayFine payFine = new ResolutionPayFine();
                                                payFine.TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp;
                                                payFine.Amount = payment.Amount;
                                                payFine.DocumentNum = payment.PaymentId;
                                                payFine.DocumentDate = payment.PaymentDate;
                                                payFine.GisUip = payment.PayerId;
                                                payFine.Resolution = ResolutionDomain.GetAll().Where(x => x == docChild.Children).First();
                                                ResolutionPayFineDomain.Save(payFine);
                                                payment.IsPayFineAdded = true;
                                                PayRegDomain.Update(payment);
                                                ResolutionDomain.Update(ResolutionDomain.GetAll().Where(x => x == docChild.Children).First());
                                                num++;
                                            }
                                        }
                                    }
                                    else if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                                    {
                                        foreach (var payment in payments)
                                        {
                                            var resolution = ResolutionDomain.GetAll()
                                                .Where(y => y == document).FirstOrDefault();
                                            if (resolution != null && !ResolutionPayFineDomain.GetAll().Where(x => x.DocumentNum == payment.PaymentId).Any())
                                            {
                                                ResolutionPayFine payFine = new ResolutionPayFine();
                                                payFine.TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp;
                                                payFine.Amount = payment.Amount;
                                                payFine.DocumentNum = payment.PaymentId;
                                                payFine.DocumentDate = payment.PaymentDate;
                                                payFine.GisUip = payment.PayerId;
                                                payFine.Resolution = resolution;
                                                ResolutionPayFineDomain.Save(payFine);
                                                payment.IsPayFineAdded = true;
                                                PayRegDomain.Update(payment);
                                                if (!resolution.PaymentDate.HasValue || (resolution.PaymentDate < payFine.DocumentDate))
                                                {
                                                    resolution.PaymentDate = payFine.DocumentDate;
                                                }
                                                ResolutionDomain.Update(resolution);
                                                num++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (gisGmp.TypeLicenseRequest == Enums.TypeLicenseRequest.First)
                        {
                            if (gisGmp.ManOrgLicenseRequest != null)
                            {
                                var manOrgLicReq = ManOrgLicenseRequestDomain.GetAll()
                                    .Where(x => x.Id == gisGmp.ManOrgLicenseRequest.Id).FirstOrDefault();
                                var payments = PayRegDomain.GetAll()
                                    .Where(x => x.GisGmp.Id == gisGmp.Id && !x.IsPayFineAdded);
                                foreach (var payment in payments)
                                {
                                    //if (!ManOrgRequestProvDocDomain.GetAll().Where(x => x.Number == payment.PaymentId).Any())
                                    //{
                                    ManOrgRequestProvDoc provDoc = new ManOrgRequestProvDoc();
                                    provDoc.Date = payment.PaymentDate;
                                    provDoc.Number = payment.PaymentId;
                                    provDoc.LicRequest = manOrgLicReq;
                                    provDoc.LicProvidedDoc = LicenseProvidedDocDomain.GetAll().Where(x => x.Code == "10").FirstOrDefault(); // документ об оплате
                                    ManOrgRequestProvDocDomain.Save(provDoc);
                                    payment.IsPayFineAdded = true;
                                    PayRegDomain.Update(payment);
                                    if (manOrgLicReq.ConfirmationOfDuty.Length == 0)
                                    {
                                        manOrgLicReq.ConfirmationOfDuty = "Платёж из ГИС ГМП " + payment.PaymentId + " от " + payment.PaymentDate.Value.ToShortDateString();
                                    }
                                    else
                                    {
                                        manOrgLicReq.ConfirmationOfDuty += ", платёж из ГИС ГМП " + payment.PaymentId + " от " + payment.PaymentDate.Value.ToShortDateString();
                                    }
                                    ManOrgLicenseRequestDomain.Update(manOrgLicReq);
                                    num++;
                                    //}
                                }
                            }
                        }
                        else if (gisGmp.TypeLicenseRequest == Enums.TypeLicenseRequest.Copy || gisGmp.TypeLicenseRequest == Enums.TypeLicenseRequest.Reissuance)
                        {
                            if (gisGmp.LicenseReissuance != null)
                            {
                                var licReissuance = LicenseReissuanceDomain.GetAll()
                                    .Where(x => x.Id == gisGmp.LicenseReissuance.Id).FirstOrDefault();
                                var payments = PayRegDomain.GetAll()
                                    .Where(x => x.GisGmp.Id == gisGmp.Id && !x.IsPayFineAdded);
                                foreach (var payment in payments)
                                {
                                    //if (!LicenseReissuanceProvDocDomain.GetAll().Where(x => x.Number == payment.PaymentId).Any())
                                    //{
                                    LicenseReissuanceProvDoc provDoc = new LicenseReissuanceProvDoc();
                                    provDoc.Date = payment.PaymentDate;
                                    provDoc.Number = payment.PaymentId;
                                    provDoc.LicenseReissuance = licReissuance;
                                    provDoc.LicProvidedDoc = LicenseProvidedDocDomain.GetAll().Where(x => x.Code == "10").FirstOrDefault(); // документ об оплате
                                    LicenseReissuanceProvDocDomain.Save(provDoc);
                                    payment.IsPayFineAdded = true;
                                    PayRegDomain.Update(payment);
                                    num++;
                                    //}
                                    if (licReissuance.ConfirmationOfDuty.Length == 0)
                                    {
                                        licReissuance.ConfirmationOfDuty = "Платёж из ГИС ГМП " + payment.PaymentId + " от " + payment.PaymentDate.Value.ToShortDateString();
                                    }
                                    else
                                    {
                                        licReissuance.ConfirmationOfDuty += ", платёж из ГИС ГМП " + payment.PaymentId + " от " + payment.PaymentDate.Value.ToShortDateString();
                                    }
                                    LicenseReissuanceDomain.Update(licReissuance);
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        continue;
                    }
                }
                operationlog += "Заполнение оплат штрафов завершено, в базу добавлено " + num + " новых оплат";
                return new BaseDataResult(true, operationlog);
            }
            catch (Exception e)
            {
                operationlog += e.InnerException;
                return new BaseDataResult(false, $"Ошибка: {e.InnerException}");
            }
        }

        private void CreateCalcRequests()
        {
            //получили список протоколов по которым были отправлены начисления
            var gisGmps = GisGmpDomain.GetAll()
         //   .Where(x => x.RequestState != Enums.RequestState.Error)
            .Where(x=> x.CalcDate > DateTime.Now.AddMonths(-30))
            .Where(x=> x.Protocol != null)
            .Select(x => x.Protocol.Id).Distinct().ToList();            

            var possibleResolutions = ResolutionDomain.GetAll()
                .Where(x => x.State.FinalState)
                .Where(x => x.InLawDate.HasValue && x.InLawDate.Value<=DateTime.Now)
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value > DateTime.Now.AddMonths(-18))
                .Where(x => x.PenaltyAmount.HasValue && x.PenaltyAmount > 0)
                .Select(x => x.Id).Distinct().ToList();

            var possibleProtocolIds = DocumentGjiChildrenDomain.GetAll()
                .Where(x=> possibleResolutions.Contains(x.Children.Id))
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution && !gisGmps.Contains(x.Parent.Id))
                .Select(x => x.Parent.Id).Distinct().ToList();

            foreach (long id in possibleProtocolIds)
            {
                try
                {
                    CreateCalcRequest(id);
                }
                catch
                {
                    
                }
            }
            
        }
        private void CreateCalcRequest(long resid)
        {
            var gisGmp = GetPayerInfo(resid);
            if (string.IsNullOrEmpty(gisGmp.KBK))
            {
                try
                {
                    GisGmpDomain.Save(gisGmp);
                    operationlog += $"постановление по протоколу/постановлению прокуратуры {gisGmp.Protocol.DocumentNumber} от {gisGmp.Protocol.DocumentDate} обработан";
                }
                catch (Exception e)
                {
                    operationlog += $"постановление по протоколу/постановлению прокуратуры {gisGmp.Protocol.DocumentNumber} от {gisGmp.Protocol.DocumentDate} ошибка {e.Message} {e.StackTrace}";
                }
            }
            else
            {
                try
                {
                    GisGmpDomain.Save(gisGmp);
                    operationlog += $"постановление по протоколу/постановлению прокуратуры {gisGmp.Protocol.DocumentNumber} от {gisGmp.Protocol.DocumentDate} обработан";
                }
                catch (Exception e)
                {
                    operationlog += $"постановление по протоколу/постановлению прокуратуры {gisGmp.Protocol.DocumentNumber} от {gisGmp.Protocol.DocumentDate} ошибка {e.Message} {e.StackTrace}";
                }
                BaseParams baseParams = new BaseParams();
                baseParams.Params.Add("taskId", gisGmp.Id);
              //  _taskManager.CreateTasks(new SendCalcRequestTaskProvider(_container), baseParams);
            }
        }

        private GisGmp GetPayerInfo(long protocolData)
        {
            decimal result = 0;
            string reasonVal = "";
            decimal ammount = 0;
            string oktmo = "";
            string innVal = "";
            string kppVal = "";
            string kbk = "";
            string seriesfl = "";
            string numberfl = "";
            PhysicalPersonDocType doctype = null;
            PayerType typeVal = PayerType.Juridical;
            GISGMPPayerStatus payerstate = null;
            DocumentGji doc = DocumentGjiDomain.Get(protocolData);

            try
            {
                if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.Protocol)
                {
                    Protocol entity = ProtocolDomain.Get(protocolData);
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();

                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.DocumentNumber;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;
                            if (entity.PhysicalPersonDocType != null)
                            {
                                doctype = new PhysicalPersonDocType
                                {
                                    Code = entity.PhysicalPersonDocType.Code,
                                    Name = entity.PhysicalPersonDocType.Name,
                                    Id = entity.PhysicalPersonDocType.Id
                                };
                                seriesfl = entity.PhysicalPersonDocumentSerial;
                                numberfl = entity.PhysicalPersonDocumentNumber;
                            }

                        }
                        try
                        {
                            kbk = ProtocolArticleLawDomain.GetAll()
                                   .FirstOrDefault(x => x.Protocol == entity).ArticleLaw.KBK;
                        }
                        catch
                        { }

                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {
                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.Protocol197)
                {
                    Protocol197 entity = Protocol197Domain.Get(protocolData);
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.DocumentNumber;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;
                            if (entity.PhysicalPersonDocType != null)
                            {
                                doctype = new PhysicalPersonDocType
                                {
                                    Code = entity.PhysicalPersonDocType.Code,
                                    Name = entity.PhysicalPersonDocType.Name,
                                    Id = entity.PhysicalPersonDocType.Id
                                };
                                seriesfl = entity.PhysicalPersonDocumentSerial;
                                numberfl = entity.PhysicalPersonDocumentNumber;
                            }

                        }
                        try
                        {
                            kbk = Protocol197ArticleLawDomain.GetAll()
                                   .FirstOrDefault(x => x.Protocol197 == entity).ArticleLaw.KBK;
                        }
                        catch
                        { }

                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {
                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.ProtocolRSO)
                {
                    ProtocolRSO entity = ProtocolRSODomain.Get(protocolData);
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    try
                    {
                        kbk = ProtocolRSOArticleLawDomain.GetAll()
                               .FirstOrDefault(x => x.ProtocolRSO == entity).ArticleLaw.KBK;
                    }
                    catch
                    { }
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.DocumentNumber;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;

                        }
                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {

                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }

                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.ProtocolMhc)
                {
                    ProtocolMhc entity = ProtocolMhcDomain.Get(protocolData);
                    try
                    {
                        kbk = ProtocolMhcArticleLawDomain.GetAll()
                               .FirstOrDefault(x => x.ProtocolMhc == entity).ArticleLaw.KBK;
                    }
                    catch
                    { }
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.DocumentNumber;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;

                        }
                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {

                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                {
                    ResolPros entity = ResolProsDomain.Get(protocolData);
                    try
                    {
                        kbk = ResolProsArticleLawDomain.GetAll()
                               .FirstOrDefault(x => x.ResolPros == entity).ArticleLaw.KBK;
                    }
                    catch
                    { }
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.DocumentNumber;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;
                            if (resol.PhysicalPersonDocType != null)
                            {
                                doctype = new PhysicalPersonDocType
                                {
                                    Code = resol.PhysicalPersonDocType.Code,
                                    Name = resol.PhysicalPersonDocType.Name,
                                    Id = resol.PhysicalPersonDocType.Id
                                };
                                seriesfl = resol.PhysicalPersonDocumentSerial;
                                numberfl = resol.PhysicalPersonDocumentNumber;
                            }

                        }
                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {

                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }

            if (typeVal == PayerType.Physical)
            {
                payerstate = GISGMPPayerStatusDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "13");
            }
            if (typeVal == PayerType.Juridical)
            {
                payerstate = GISGMPPayerStatusDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "01");
            }
            if (typeVal == PayerType.IP)
            {
                payerstate = GISGMPPayerStatusDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "08");
            }
            //ОКТМО всегда 75701000 (задача трелло Формирование начисления штрафа в обмене данными с ГИС ГМП)
            oktmo = "75701000";
            var data = new GisGmp {
                Protocol = doc,
                INN = innVal,
                KPP = kppVal,
                PayerType = typeVal,
                KBK = kbk,
                OKTMO = oktmo,
                TotalAmount = ammount,
                BillFor = reasonVal,
                PhysicalPersonDocType = doctype,
                DocumentSerial = seriesfl,
                DocumentNumber = numberfl,
                GISGMPPayerStatus = payerstate,
                CalcDate = DateTime.Now,
                BillDate = DateTime.Now,
                GisGmpChargeType = GisGmpChargeType.First,
                IdentifierType = IdentifierType.INN,
                IsRF = true,
                TypeLicenseRequest = TypeLicenseRequest.NotSet,
                Inspector = GetDefaultInspector()};
                return data;
        }

        private Inspector GetDefaultInspector()
        {
            return InspectorDomain.GetAll().FirstOrDefault(x=> x.IsHead && x.Active && x.Description == "Отдел финансов, бухгалтерского учета и закупок");
        }
    }
}
