namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Entities;

    public class RosRegExtractGovViewModel : BaseViewModel<RosRegExtractGov>
    {
        public override IDataResult List(IDomainService<RosRegExtractGov> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var parentId = loadParams.Filter.GetAs("parentId", 0L);
            var rosRegExtractDomain = this.Container.Resolve<IDomainService<RosRegExtract>>();
            var rosregExtract = rosRegExtractDomain.GetAll()
               .Where(x => x.desc_id.Id == parentId)
               .Where(x => x.right_id != null && x.right_id.owner_id != null && x.right_id.owner_id.gov_id != null)
               .Select(x => new
              {
                   x.right_id.owner_id.gov_id.Id,
                   x.right_id.owner_id.gov_id.Gov_Code_SP,
                   x.right_id.owner_id.gov_id.Gov_Content,
                   x.right_id.owner_id.gov_id.Gov_Name,
                   x.right_id.owner_id.gov_id.Gov_OKATO_Code,
                   x.right_id.owner_id.gov_id.Gov_Country,
                   x.right_id.owner_id.gov_id.Gov_Address
               })
                .Filter(loadParams, Container);

            return new ListDataResult(rosregExtract.Order(loadParams).Paging(loadParams).ToList(), rosregExtract.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<RosRegExtractGov> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Gov_Code_SP,
                        obj.Gov_Content,
                        obj.Gov_Name,
                        obj.Gov_OKATO_Code,
                        obj.Gov_Country,
                        obj.Gov_Address
                    });
            }
            return new BaseDataResult();
        }
    }
}
