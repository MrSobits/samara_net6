namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class NonResidentialPlacementViewModel : BaseViewModel<NonResidentialPlacement>
    {
        public override IDataResult List(IDomainService<NonResidentialPlacement> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var disclosureInfoRealityObj = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().GetAll()
                    .Where(x => x.Id == disclosureInfoRealityObjId)
                    .Select(x => new { x.Id, x.RealityObject, x.PeriodDi })
                    .FirstOrDefault();

            if (disclosureInfoRealityObj == null)
            {
                return new ListDataResult();
            }

            var periodDi = disclosureInfoRealityObj.PeriodDi;

            var data = domainService
                .GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                /* Фильтрация по датам документа. Период дата начала - дата конца документа должен пересекаться
                 с периодом дата начала - дата конца периода раскрытия */
                .Where(
                    x =>
                    periodDi != null
                    && (((x.DateStart.HasValue && periodDi.DateStart.HasValue && (x.DateStart.Value >= periodDi.DateStart.Value) || !periodDi.DateStart.HasValue)
                            && (periodDi.DateEnd.HasValue && x.DateStart.HasValue && (periodDi.DateEnd.Value >= x.DateStart.Value) || !periodDi.DateEnd.HasValue))
                            || ((x.DateStart.HasValue && periodDi.DateStart.HasValue && (periodDi.DateStart.Value >= x.DateStart.Value) || !x.DateStart.HasValue)
                                && (x.DateEnd.HasValue && periodDi.DateStart.HasValue
                                    && (x.DateEnd.Value >= periodDi.DateStart.Value) || !x.DateEnd.HasValue))))
                .Select(x => new
                {
                    x.Id,
                    x.TypeContragentDi,
                    x.ContragentName,
                    x.Area,
                    x.DateStart,
                    x.DateEnd,
                    x.DocumentDateApartment,
                    x.DocumentNumApartment,
                    x.DocumentNameApartment,
                    x.DocumentDateCommunal,
                    x.DocumentNumCommunal,
                    x.DocumentNameCommunal
                })
                .Filter(loadParams, this.Container);
            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);
            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}