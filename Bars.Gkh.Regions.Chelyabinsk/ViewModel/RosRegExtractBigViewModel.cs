namespace Bars.Gkh.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RosRegExtractBigViewModel : BaseViewModel<RosRegExtractBig>
    {
        public override IDataResult List(IDomainService<RosRegExtractBig> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
               .Select(x => new
               {
                   x.Id,
                   x.CadastralNumber,
                   x.Address,
                   x.ExtractDate,
                   x.ExtractNumber,
                   x.RoomArea
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
        public override IDataResult Get(IDomainService<RosRegExtractBig> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.CadastralNumber,
                        obj.Address,
                        obj.ExtractDate,
                        obj.ExtractNumber,
                        obj.RoomArea
                    });
            }
            return new BaseDataResult();
        }
    }
}
