namespace Bars.Gkh.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RosRegExtractBigOwnerViewModel : BaseViewModel<RosRegExtractBigOwner>
    {
        public override IDataResult List(IDomainService<RosRegExtractBigOwner> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var parentId = loadParams.Filter.GetAs("parentId", 0L);
            var data = domain.GetAll()
               .Where(x=> x.ExtractId==parentId)
               .Select(x => new
               {
                   x.Id,
                   x.ExtractId,
                   x.OwnerName,
                   x.AreaShareNum,
                   x.AreaShareDen,
                   x.RightNumber
        })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<RosRegExtractBigOwner> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.ExtractId,
                        obj.OwnerName,
                        obj.AreaShareNum,
                        obj.AreaShareDen,
                        obj.RightNumber
                    });
            }
            return new BaseDataResult();
        }
    }
}
