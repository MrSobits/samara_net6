namespace Bars.Gkh.Decisions.Nso.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Domain.Decisions;
    using Newtonsoft.Json;

    public class RealityObjectGenericDecisionViewModel : BaseViewModel<GenericDecision>
    {
        public IDecisionContainer DecisionContainer { get; set; }

        public override IDataResult List(IDomainService<GenericDecision> domainService, BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAs<long>("protocolId");

            var onlyActive = baseParams.Params.GetAs<bool>("onlyActive");

            var decisions = DecisionContainer.AllDecisions;
            var types = DecisionContainer.AllTypes;

            var data =
                domainService.GetAll()
                    .Where(x => x.Protocol.Id == protocolId)
                    .WhereIf(onlyActive, x => x.IsActual)
                    .OrderBy(x => x.ObjectCreateDate)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.Protocol,
                        x.IsActual,
                        x.StartDate,
                        x.DecisionCode,
                        x.File,
                        Decision = decisions.FirstOrDefault(y => y.Code == x.DecisionCode),
                        Type = types.FirstOrDefault(y => y.Code == x.DecisionCode),
                        Value = x.JsonObject.Return(JsonConvert.DeserializeObject)
                    })
                    .Select(x => new
                    {
                        x.Id,
                        x.Protocol,
                        x.IsActual,
                        x.StartDate,
                        x.DecisionCode,
                        x.File,
                        x.Value,
                        Name =
                            x.Decision != null
                                ? x.Decision.Return(y => y.DecisionType.Name)
                                : x.Type.Return(y => y.Name),
                        Js = x.Decision != null ? x.Decision.Return(y => y.DecisionType.Js) : x.Type.Return(y => y.Js)
                    });

            return new ListDataResult(data, data.Count()); 
        }
    }
}