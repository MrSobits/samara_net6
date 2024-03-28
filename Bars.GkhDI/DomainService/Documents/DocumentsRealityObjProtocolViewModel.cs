namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class DocumentsRealityObjProtocolViewModel : BaseViewModel<DocumentsRealityObjProtocol>
    {
        public override IDataResult List(IDomainService<DocumentsRealityObjProtocol> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var serviceDiRealityObj = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var disclosureInfoRealityObj = serviceDiRealityObj.Load(disclosureInfoRealityObjId);

            if (disclosureInfoRealityObj == null)
            {
                return new ListDataResult();
            }

            var data = domainService
                .GetAll()

                // Отображаем файлы только данного раскрытия по текущему году
                .Where(x => x.DisclosureInfoRealityObj == disclosureInfoRealityObj
                    && x.Year >= disclosureInfoRealityObj.PeriodDi.DateStart.Value.Year - 1 && x.Year <= disclosureInfoRealityObj.PeriodDi.DateEnd.Value.Year)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}