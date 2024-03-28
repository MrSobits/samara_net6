namespace Sobits.RosReg.ViewModel
{
    using System.Linq;

    using Bars.B4;

    using Sobits.RosReg.Entities;

    public class ExtractEgrnRightViewModel : BaseViewModel<ExtractEgrnRight>
    {
        public override IDataResult List(IDomainService<ExtractEgrnRight> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(
                    x => new
                    {
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
        public override IDataResult Get(IDomainService<ExtractEgrnRight> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                    });
            }

            return new BaseDataResult();
        }
    }
}