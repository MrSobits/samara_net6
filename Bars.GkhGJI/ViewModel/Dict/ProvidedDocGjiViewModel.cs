namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ProvidedDocGjiViewModel : BaseViewModel<ProvidedDocGji>
    {
        public override IDataResult List(IDomainService<ProvidedDocGji> domainService, BaseParams baseParams)
        {
            var disposalTypeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var typeSurveyProvidedDocumentGjiDomain = Container.Resolve<IDomainService<TypeSurveyProvidedDocumentGji>>();

            var loadParam = baseParams.GetLoadParam();

            var disposalId = baseParams.Params.GetAs("disposalId", 0L);

            List<long> typeSurveyDocList;

            using (Container.Using(disposalTypeSurveyDomain, typeSurveyProvidedDocumentGjiDomain))
            {
                typeSurveyDocList =
                    disposalTypeSurveyDomain.GetAll()
                        .Join(typeSurveyProvidedDocumentGjiDomain.GetAll(), x => x.TypeSurvey.Id, y => y.TypeSurvey.Id, (x, y) => new { DisposalTypeSurvey = x, TypeSurveyProvidedDocumentGji = y })
                        .Where(x => x.DisposalTypeSurvey.Disposal.Id == disposalId)
                        .Select(x => x.TypeSurveyProvidedDocumentGji.ProvidedDocGji.Id)
                        .Distinct()
                        .ToList();
            }

            var data = domainService.GetAll().WhereIf(typeSurveyDocList.Count > 0, x => typeSurveyDocList.Contains(x.Id)).Select(x => new { x.Id, x.Name, x.Code }).Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}