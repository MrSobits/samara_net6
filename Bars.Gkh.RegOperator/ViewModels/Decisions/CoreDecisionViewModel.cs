namespace Bars.Gkh.RegOperator.ViewModels.Decisions
{
    using System.Linq;

    using B4;
    using Entities.Decisions;
    using Gkh.Domain;

    public sealed class CoreDecisionViewModel : BaseViewModel<CoreDecision>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<CoreDecision> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAsId("roId");

            var data = domainService.GetAll()
                .Where(item => (item.GovDecision != null && item.GovDecision.RealityObject.Id == objectId) || (item.UltimateDecision != null && item.UltimateDecision.Protocol.RealityObject.Id == objectId))
                .Select(item => new
                {
                    item.Id,
                    DocumentType = item.DecisionType,
                    DocumentNum = item.GovDecision != null ? item.GovDecision.ProtocolNumber : item.UltimateDecision.Protocol.DocumentNum,
                    ProtocolDate = item.GovDecision != null ? item.GovDecision.ProtocolDate : item.UltimateDecision.Protocol.ProtocolDate,
                    State = item.GovDecision != null ? item.GovDecision.State : item.UltimateDecision.Protocol.State
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.ToArray().Count());
        }
    }
}