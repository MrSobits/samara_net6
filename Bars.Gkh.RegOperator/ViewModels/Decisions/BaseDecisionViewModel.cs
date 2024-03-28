namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Gkh.Domain;

    /// <summary>
    /// Вью модель для <see cref="BaseDecisionProtocol"/>
    /// </summary>
    public class BaseDecisionProtocolViewModel : BaseViewModel<BaseDecisionProtocol>
    {
        /// <summary>Получить объект </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса </returns>
        public override IDataResult Get(IDomainService<BaseDecisionProtocol> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
            
            return new BaseDataResult(new
            {
                entity.Id,
                entity.ProtocolNumber,
                entity.ProtocolDate,
                entity.DateStart,
                entity.Description,
                entity.ProtocolFile,
                entity.State,
                RealityObject = entity.RealityObject.Id
            });
        }

        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<BaseDecisionProtocol> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var roId = baseParams.Params.GetAsId("objectId");

            var result = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId);

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
        }
    }
}