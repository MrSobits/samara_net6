using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction
{
    /// <summary>
    /// Прописать поле DocNumDate начислениям
    /// </summary>
    public class FindGisGmpDocNumbersAction : BaseExecutionAction
    {
        public IRepository<GisGmp> GisGmpRepo { get; set; }

        public override string Description => "Заполнение поля DocNumDate начислениям для сопоставления с платежами из ГИС ГМП";

        public override string Name => "Прописать поле DocNumDate начислениям";

        public override Func<IDataResult> Action => FindGisGmpDocNumbers;

        private IDataResult FindGisGmpDocNumbers()
        {
            GisGmpRepo.GetAll().ForEach(x =>
            {
                x.DocNumDate = FindDocNumbers(x);
                GisGmpRepo.Update(x);
            });
            return new BaseDataResult(true, "Строки прописаны");
        }

        private string FindDocNumbers(GisGmp x)
        {
            // x.Protocol.DocumentNumber
            var DocumentGjiChildrenRepo = Container.Resolve<IRepository<DocumentGjiChildren>>();
            var DocumentGjiRepo = Container.Resolve<IRepository<DocumentGji>>();
            var ResolutionRepo = Container.Resolve<IRepository<Resolution>>();
            var Protocol197Repo = Container.Resolve<IRepository<Protocol197>>();
            var ResolutionProsecutorRepo = Container.Resolve<IRepository<ResolPros>>();
            var ResolutionRospotrebnadzorRepo = Container.Resolve<IRepository<ResolutionRospotrebnadzor>>();
            var ManOrgLicenseRequestRepo = Container.Resolve<IRepository<ManOrgLicenseRequest>>();
            var LicenseReissuanceRepo = Container.Resolve<IRepository<LicenseReissuance>>();


            string docNums = "";
            if (x.TypeLicenseRequest == TypeLicenseRequest.NotSet && x.Protocol != null)
            {

                var document = DocumentGjiRepo.GetAll()
                    .Where(y => y == x.Protocol)
                    .Select(y => new
                    {
                        y.Id,
                        y.TypeDocumentGji
                    })
                    .FirstOrDefault();
                if (document != null)
                {

                    if (document.TypeDocumentGji == TypeDocumentGji.Protocol ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                    {
                        var docChildrenParent = DocumentGjiChildrenRepo.GetAll()
                            .Where(y => y.Parent.Id == document.Id)
                            .Select(y => new
                            {
                                y.Id,
                                y.Children
                            }).FirstOrDefault();
                        if (docChildrenParent != null)
                        {
                            var resolution = ResolutionRepo.GetAll()
                                .Where(y => y == docChildrenParent.Children)
                                .Select(y => new
                                {
                                    y.DocumentNumber,
                                    y.DocumentDate,
                                    y.DecisionDate,
                                    y.DecisionNumber,
                                    y.ExecuteSSPNumber,
                                    y.DateExecuteSSP
                                }).FirstOrDefault();
                            if (resolution != null)
                            {
                                if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                                {
                                    docNums = "Постановление " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                                }
                                if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != ""))
                                {
                                    docNums = docNums + ", судебное решение " + resolution.DecisionNumber + " от " + resolution.DecisionDate.Value.ToShortDateString();
                                }
                                if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != ""))
                                {
                                    docNums = docNums + ", ИП " + resolution.ExecuteSSPNumber;// + " от " + resolution.DateExecuteSSP.Value.ToShortDateString();
                                }
                            }

                        }

                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                    {
                        var resolution = ResolutionRepo.GetAll()
                            .Where(y => y.Id == document.Id)
                            .Select(y => new
                            {
                                y.DocumentNumber,
                                y.DocumentDate,
                                y.DecisionDate,
                                y.DecisionNumber,
                                y.ExecuteSSPNumber,
                                y.DateExecuteSSP
                            }).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                            {
                                docNums = "Постановление " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                            }
                            if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != ""))
                            {
                                docNums = docNums + ", судебное решение " + resolution.DecisionNumber + " от " + resolution.DecisionDate.Value.ToShortDateString();
                            }
                            if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != ""))
                            {
                                docNums = docNums + ", ИП " + resolution.ExecuteSSPNumber;// + " от " + resolution.DateExecuteSSP.Value.ToShortDateString();
                            }
                        }
                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Protocol197)
                    {
                        var resolution = Protocol197Repo.GetAll()
                            .Where(y => y.Id == document.Id)
                            .Select(y => new
                            {
                                y.DocumentNumber,
                                y.DocumentDate
                            }).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                            {
                                docNums = "Протокол по статье 19.7 " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                            }
                        }
                    }
                }

            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.First && x.ManOrgLicenseRequest != null)
            {
                var manOrgLicReq = ManOrgLicenseRequestRepo.GetAll()
                    .Where(y => y == x.ManOrgLicenseRequest)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.DateRequest
                    }).FirstOrDefault();
                if (manOrgLicReq != null)
                {
                    if ((manOrgLicReq.RegisterNumber != null) && (manOrgLicReq.RegisterNumber != ""))
                    {
                        docNums = "Заявка на лицензию " + manOrgLicReq.RegisterNumber + " от " + manOrgLicReq.DateRequest.Value.ToShortDateString();
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Reissuance && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.ReissuanceDate
                    }).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.RegisterNumber != null) && (licReissuance.RegisterNumber != ""))
                    {
                        docNums = "Заявка на переоформление лицензии " + licReissuance.RegisterNumber + " от " + licReissuance.ReissuanceDate.Value.ToShortDateString();
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Copy && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.ReissuanceDate
                    }).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.RegisterNumber != null) && (licReissuance.RegisterNumber != ""))
                    {
                        docNums = "Заявка на дубликат лицензии " + licReissuance.RegisterNumber + " от " + licReissuance.ReissuanceDate.Value.ToShortDateString();
                    }
                }
            }
            return docNums;
        }
    }
}
