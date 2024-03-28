namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Entities;

    public class RosRegExtractOwnerViewModel : BaseViewModel<RosRegExtractOwner>
    {
        public override IDataResult List(IDomainService<RosRegExtractOwner> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
               .Select(x => new
               {
                   x.Id,
                   x.gov_id,
                   x.org_id,
                   x.pers_id
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
        public override IDataResult Get(IDomainService<RosRegExtractOwner> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.gov_id,
                        obj.org_id,
                        obj.pers_id
                    });
            }
            return new BaseDataResult();
        }
    }
}
