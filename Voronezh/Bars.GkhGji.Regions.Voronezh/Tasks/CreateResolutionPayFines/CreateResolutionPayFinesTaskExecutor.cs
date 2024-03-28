using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bars.GkhGji.Enums;
using Bars.Gkh.Entities;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    public class CreateResolutionPayFinesTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<PayReg> PayRegDomain { get; set; }

        public IDomainService<GisGmp> GisGmpDomain { get; set; }

        public IDomainService<ResolutionPayFine> ResolutionPayFineDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

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
                return new BaseDataResult(true, "Заполнение оплат штрафов завершено, в базу добавлено " + num + " новых оплат");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"Ошибка: {e.InnerException}");
            }
        }
    }
}
