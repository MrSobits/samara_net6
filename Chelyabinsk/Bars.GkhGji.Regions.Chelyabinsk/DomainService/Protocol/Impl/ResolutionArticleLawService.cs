namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;

    public class ResolutionArticleLawService : IResolutionArticleLawService
    {

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var documentId = baseParams.Params.GetAs<long>("documentId");
                    var articleIds = baseParams.Params.GetAs<List<long>>("articleIds");

                    var serviceArticles = Container.Resolve<IDomainService<ResolutionArtLaw>>();
                    var serviceResolution = Container.Resolve<IDomainService<Resolution>>();
                    var serviceArticleLaw = Container.Resolve<IDomainService<ArticleLawGji>>();

                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в один и тотже протокол)
                    var listIds =
                        serviceArticles.GetAll()
                            .Where(x => x.Resolution.Id == documentId)
                            .Select(x => x.ArticleLawGji.Id)
                            .Distinct()
                            .ToList();

                    var resolution = serviceResolution.Load(documentId);

                    foreach (var id in articleIds)
                    {
                        // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                        if (listIds.Contains(id))
                            continue;

                        // Если такой статьи еще нет, то добалвяем
                        var newObj = new ResolutionArtLaw
                        {
                                Resolution = resolution,
                                ArticleLawGji = serviceArticleLaw.Load(id)
                            };

                        serviceArticles.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
            }
        }

        public IDataResult GetListResolution(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var gisgmpdocsServ = this.Container.Resolve<IDomainService<GisGmp>>();
            var docidList = gisgmpdocsServ.GetAll()
                .Where(x => x.Protocol != null)
                .Select(x => x.Protocol.Id).Distinct().ToList();
            var resolutionList = ResolutionDomain.GetAll()
                .Where(x => x.ObjectCreateDate > DateTime.Now.AddYears(-2))
                .Where(x => x.PenaltyAmount.HasValue)
                .Select(x=> x.Id).Distinct().ToList();
            var avalProt = DocumentGjiChildrenDomain.GetAll()
                .Where(x => resolutionList.Contains(x.Children.Id))
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Protocol
                // || x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Resolution
                || x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Protocol197
                || x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ProtocolMhc
                || x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ResolutionProsecutor)
            .Select(x => x.Parent.Id).Distinct().ToList();


            var data = DocumentGjiDomain.GetAll()
                .Where(x => !x.State.StartState)
                .Where(x=> !docidList.Contains(x.Id))
                .Where(x=> avalProt.Contains(x.Id))             
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDocumentGji
                })
  //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }


        public IDataResult GetListDisposal(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var giserpdocsServ = this.Container.Resolve<IDomainService<GISERP>>();
            var docidList = giserpdocsServ.GetAll()
                .Where(x => x.Disposal != null)
                .Select(x => x.Disposal.Id).Distinct().ToList();

            var data = DocumentGjiDomain.GetAll()
                .Where(x=> x.Inspection.Contragent != null)
                .Where(x=> x.ObjectCreateDate>= DateTime.Now.AddMonths(-3))
                .Where(x => !x.State.StartState)
                .Where(x => !docidList.Contains(x.Id))
                .Where(x => x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Disposal)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDocumentGji,
                    State = x.State.Name,
                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult GetListDecision(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var giserpdocsServ = this.Container.Resolve<IDomainService<ERKNM>>();
            var docidList = giserpdocsServ.GetAll()
                .Where(x => x.Disposal != null)
                .Select(x => x.Disposal.Id).Distinct().ToList();

            var data = DocumentGjiDomain.GetAll()
                .Where(x => x.Inspection.Contragent != null)
                .Where(x => x.ObjectCreateDate >= DateTime.Now.AddMonths(-12))
                .Where(x => !x.State.StartState)
                .Where(x => !docidList.Contains(x.Id))
                .Where(x => x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDocumentGji,
                    State = x.State.Name,
                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}