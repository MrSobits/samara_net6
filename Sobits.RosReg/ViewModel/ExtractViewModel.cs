namespace Sobits.RosReg.ViewModel
{
    using System.Linq;

    using Bars.B4;

    using Sobits.RosReg.Entities;

    public class ExtractViewModel : BaseViewModel<Extract>
    {
        public override IDataResult List(IDomainService<Extract> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.CreateDate,
                        x.Type,
                        x.IsParsed,
                        x.IsActive
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <inheritdoc />
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<Extract> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.CreateDate,
                        obj.Type,
                        obj.IsParsed,
                        obj.IsActive
                    });
            }

            return new BaseDataResult();
        }
    }
}