﻿namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Castle.Windsor;
    using Gkh.Authentification;

    public class EDSDocumentService : IEDSDocumentService
    {
        public IWindsorContainer Container { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IDataResult ListEDSDocuments(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var inspId = baseParams.Params.GetAs("inspId", 0L);
            var disposalAnnexServ = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            var decisionAnnexServ = this.Container.Resolve<IDomainService<DecisionAnnex>>();
            var actCheckServ = this.Container.Resolve<IDomainService<ActCheckAnnex>>();
            var actCheckDefinitionServ = this.Container.Resolve<IDomainService<ActCheckDefinition>>();
            var prescroptionServ = this.Container.Resolve<IDomainService<PrescriptionAnnex>>();
            var protocolService = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            var protocol197Service = this.Container.Resolve<IDomainService<Protocol197Annex>>();
            var resolutionService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            var appCitsAdmonService = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();
            if (inspId > 0)
            {
                var data = disposalAnnexServ.GetAll()
               .Where(x => x.Disposal.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList();

                data.AddRange(decisionAnnexServ.GetAll()
               .Where(x => x.Decision.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(actCheckServ.GetAll()
               .Where(x => x.ActCheck.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(actCheckDefinitionServ.GetAll()
             .Where(x => x.ActCheck.Inspection.Id == inspId)
             .Where(x => x.SignedFile != null)
             .Select(x => new
             {
                 x.Id,
                 TypeAnnex = TypeAnnex.ActDefinition,
                 x.DocumentDate,
                 Name = GetDefinitionName(x.TypeDefinition),
                 x.SignedFile
             }).ToList());

                data.AddRange(prescroptionServ.GetAll()
              .Where(x => x.Prescription.Inspection.Id == inspId)
              .Where(x => x.SignedFile != null)
              .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice)
              .Select(x => new
              {
                  x.Id,
                  x.TypeAnnex,
                  x.DocumentDate,
                  x.Name,
                  x.SignedFile
              }).ToList());

                data.AddRange(protocolService.GetAll()
               .Where(x => x.Protocol.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(protocol197Service.GetAll()
            .Where(x => x.Protocol197.Inspection.Id == inspId)
            .Where(x => x.SignedFile != null)
            .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice && x.TypeAnnex != TypeAnnex.Prescription)
            .Select(x => new
            {
                x.Id,
                x.TypeAnnex,
                x.DocumentDate,
                x.Name,
                x.SignedFile
            }).ToList());

                data.AddRange(resolutionService.GetAll()
               .Where(x => x.Resolution.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && x.TypeAnnex != TypeAnnex.DisposalNotice && x.TypeAnnex != TypeAnnex.PrescriptionNotice)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(appCitsAdmonService.GetAll()
              .Where(x => x.AppealCits != null)
              .Where(x => x.AppealCits.ZonalInspection.Id == inspId)
              .Where(x => x.SignedFile != null)
              .Select(x => new
              {
                  x.Id,
                  TypeAnnex = TypeAnnex.AppealCitsAdmonition,
                  x.DocumentDate,
                  Name = x.DocumentName,
                  x.SignedFile
              }).ToList());

                var result = data.AsQueryable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeAnnex,
                        x.DocumentDate,
                        x.Name,
                        x.SignedFile
                    }).Filter(loadParams, Container);

                var totalCount = result.Count();

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);

            }
            else
            {
                return null;
            }


            //              .OrderBy(x=>x.Id).Distinct()
            //.Filter(loadParams, Container);


        }

        public IDataResult ListEDSNotice(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var inspId = baseParams.Params.GetAs("inspId", 0L);
            var disposalAnnexServ = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            var decisionAnnexServ = this.Container.Resolve<IDomainService<DecisionAnnex>>();
            var actCheckServ = this.Container.Resolve<IDomainService<ActCheckAnnex>>();
            var prescroptionServ = this.Container.Resolve<IDomainService<PrescriptionAnnex>>();
            var protocolService = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            var resolutionService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            var protocolNotification = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            var protocol197Notification = this.Container.Resolve<IDomainService<Protocol197Annex>>();
            if (inspId > 0)
            {
                var data = disposalAnnexServ.GetAll()
               .Where(x => x.Disposal.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.DisposalNotice || x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList();

                data.AddRange(decisionAnnexServ.GetAll()
               .Where(x => x.Decision.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.DisposalNotice || x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(actCheckServ.GetAll()
               .Where(x => x.ActCheck.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
              .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.DisposalNotice || x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(prescroptionServ.GetAll()
              .Where(x => x.Prescription.Inspection.Id == inspId)
              .Where(x => x.SignedFile != null)
           .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.DisposalNotice || x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
              .Select(x => new
              {
                  x.Id,
                  x.TypeAnnex,
                  x.DocumentDate,
                  x.Name,
                  x.SignedFile
              }).ToList());

                data.AddRange(protocolService.GetAll()
               .Where(x => x.Protocol.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.DisposalNotice || x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(protocol197Notification.GetAll()
              .Where(x => x.Protocol197.Inspection.Id == inspId)
              .Where(x => x.SignedFile != null)
              .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
              .Select(x => new
              {
                  x.Id,
                  x.TypeAnnex,
                  x.DocumentDate,
                  x.Name,
                  x.SignedFile
              }).ToList());

                data.AddRange(resolutionService.GetAll()
               .Where(x => x.Resolution.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
                .Where(x => x.TypeAnnex != TypeAnnex.NotSet && (x.TypeAnnex == TypeAnnex.DisposalNotice || x.TypeAnnex == TypeAnnex.PrescriptionNotice || x.TypeAnnex == TypeAnnex.ProtocolNotification))
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());


                var result = data.AsQueryable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeAnnex,
                        x.DocumentDate,
                        x.Name,
                        x.SignedFile
                    }).Filter(loadParams, Container);

                var totalCount = result.Count();

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);

            }
            else
            {
                return null;
            }


            //              .OrderBy(x=>x.Id).Distinct()
            //.Filter(loadParams, Container);


        }

        public IDataResult ListEDSMotivRequst(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var inspId = baseParams.Params.GetAs("inspId", 0L);
            var disposalAnnexServ = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            var decisionAnnexServ = this.Container.Resolve<IDomainService<DecisionAnnex>>();
            var actCheckServ = this.Container.Resolve<IDomainService<ActCheckAnnex>>();
            var prescroptionServ = this.Container.Resolve<IDomainService<PrescriptionAnnex>>();
            var protocolService = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            var resolutionService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            if (inspId > 0)
            {
                var data = disposalAnnexServ.GetAll()
               .Where(x => x.Disposal.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex == TypeAnnex.MotivRequest)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList();

                data.AddRange(decisionAnnexServ.GetAll()
               .Where(x => x.Decision.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex == TypeAnnex.MotivRequest)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(actCheckServ.GetAll()
               .Where(x => x.ActCheck.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
              .Where(x => x.TypeAnnex == TypeAnnex.MotivRequest)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(prescroptionServ.GetAll()
              .Where(x => x.Prescription.Inspection.Id == inspId)
              .Where(x => x.SignedFile != null)
           .Where(x => x.TypeAnnex == TypeAnnex.MotivRequest)
              .Select(x => new
              {
                  x.Id,
                  x.TypeAnnex,
                  x.DocumentDate,
                  x.Name,
                  x.SignedFile
              }).ToList());

                data.AddRange(protocolService.GetAll()
               .Where(x => x.Protocol.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
               .Where(x => x.TypeAnnex == TypeAnnex.MotivRequest)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                data.AddRange(resolutionService.GetAll()
               .Where(x => x.Resolution.Inspection.Id == inspId)
               .Where(x => x.SignedFile != null)
                .Where(x => x.TypeAnnex == TypeAnnex.MotivRequest)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.DocumentDate,
                   x.Name,
                   x.SignedFile
               }).ToList());

                var result = data.AsQueryable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeAnnex,
                        x.DocumentDate,
                        x.Name,
                        x.SignedFile
                    }).Filter(loadParams, Container);

                var totalCount = result.Count();

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);

            }
            else
            {
                return null;
            }


            //              .OrderBy(x=>x.Id).Distinct()
            //.Filter(loadParams, Container);


        }
        public IDataResult ListEDSDocumentsForSign(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                return null;

            Inspector insp = thisOperator.Inspector;
            var disposalAnnexServ = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            var decisionAnnexServ = this.Container.Resolve<IDomainService<DecisionAnnex>>();
            var resolutionService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            var appCitAdmonService = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();
            var appCitAnswerService = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var appCitRequestService = this.Container.Resolve<IDomainService<AppealCitsRequest>>();

            if (insp != null)
            {
                var data = disposalAnnexServ.GetAll()
               .Where(x => x.Disposal.IssuedDisposal == insp)
               .Where(x => x.SignedFile == null)
               .Where(x => x.TypeAnnex == TypeAnnex.Disposal)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.Disposal.DocumentDate,
                   x.Disposal.DocumentNumber,
                   DocumentGjiId = x.Disposal.Id,
                   SignController = "DisposalAnnex",
                   x.File
               }).ToList();

                data.AddRange(decisionAnnexServ.GetAll()
               .Where(x => x.Decision.IssuedDisposal == insp)
               .Where(x => x.SignedFile == null)
               .Where(x => x.TypeAnnex == TypeAnnex.Decision)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.Decision.DocumentDate,
                   x.Decision.DocumentNumber,
                   DocumentGjiId = x.Decision.Id,
                   SignController = "DecisionAnnex",
                   x.File
               }).ToList());

                data.AddRange(resolutionService.GetAll()
               .Where(x => x.Resolution.Official != null)
               .Where(x => x.Resolution.Official == insp)
               .Where(x => x.SignedFile == null)
               .Where(x => x.TypeAnnex == TypeAnnex.Resolution)
               .Select(x => new
               {
                   x.Id,
                   x.TypeAnnex,
                   x.Resolution.DocumentDate,
                   x.Resolution.DocumentNumber,
                   DocumentGjiId = x.Resolution.Id,
                   SignController = "ResolutionAnnex",
                   x.File
               }).ToList());

                data.AddRange(appCitAdmonService.GetAll()
               .Where(x => x.Inspector != null)
               .Where(x => x.Inspector == insp)
               .Where(x => x.SignedFile == null)
               .Select(x => new
               {
                   x.Id,
                   TypeAnnex = TypeAnnex.AppealCitsAdmonition,
                   x.DocumentDate,
                   x.DocumentNumber,
                   DocumentGjiId = x.AppealCits != null ? x.AppealCits.Id : x.Id,
                   SignController = "AppealCitsAdmonitionSign",
                   x.File
               }).ToList());

                data.AddRange(appCitAnswerService.GetAll()
               .Where(x => x.Executor != null || x.Signer != null)
               .Where(x => x.Executor == insp || x.Signer == insp)
               .Where(x => x.SignedFile == null)
               .Where(x => x.File != null)
               .Select(x => new
               {
                   x.Id,
                   TypeAnnex = TypeAnnex.AppealCitsAnswer,
                   x.DocumentDate,
                   x.DocumentNumber,
                   DocumentGjiId = x.AppealCits != null ? x.AppealCits.Id : x.Id,
                   SignController = "AppealCitsAnswer",
                   x.File
               }).ToList());

                data.AddRange(appCitRequestService.GetAll()
               .Where(x => x.Signer != null)
               .Where(x => x.Signer == insp)
               .Where(x => x.SignedFile == null)
               .Where(x => x.File != null)
               .Select(x => new
               {
                   x.Id,
                   TypeAnnex = TypeAnnex.AppealCitsRequest,
                   x.DocumentDate,
                   x.DocumentNumber,
                   DocumentGjiId = x.AppealCits != null ? x.AppealCits.Id : x.Id,
                   SignController = "AppealCitsRequest",
                   x.File
               }).ToList());

                var result = data.AsQueryable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeAnnex,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.SignController,
                        x.File
                    }).Filter(loadParams, Container);

                var totalCount = result.Count();

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);

            }
            else
            {
                return null;
            }


            //              .OrderBy(x=>x.Id).Distinct()
            //.Filter(loadParams, Container);


        }

        public IDataResult GetListGjiDoc(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var inspId = baseParams.Params.GetAs("inspId", 0L);

            var documentGjiDomail = this.Container.Resolve<IDomainService<DocumentGji>>();

            try
            {
                var result = documentGjiDomail.GetAll()
                    .Where(x => x.Inspection.Id == inspId)
               .Select(x => new
               {
                   x.Id,
                   x.DocumentNumber,
                   x.DocumentDate,
                   x.TypeDocumentGji
               }).Filter(loadParams, Container);

                var totalCount = result.Count();

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            catch
            {
                return null;
            }
            finally
            {
                this.Container.Release(documentGjiDomail);
            }

        }

        private string GetDefinitionName(TypeDefinitionAct type)
        {
            switch (type)
            {
                case TypeDefinitionAct.RefusingProsecute:
                    return "Об отказе в возбуждении дела";
                case TypeDefinitionAct.TransportationToProtocol:
                    return "О доставлении лица для составления протокола";
                default:
                    return "";
            }
        }


    }
}
