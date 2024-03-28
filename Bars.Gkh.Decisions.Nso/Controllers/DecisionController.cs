namespace Bars.Gkh.Decisions.Nso.Controllers
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Domain;
    using Domain.Decisions;
    using Entities;

    public class DecisionController : BaseController
    {
        public DecisionController(IDomainService<GenericDecision> decisionDomain)
        {
            _decisionDomain = decisionDomain;
        }

        protected IDecisionContainer DecisionContainer
        {
            get
            {
                return _decisionContainer ?? (_decisionContainer = Resolve<IDecisionContainer>());
            }
        }

        public ActionResult GetDecisionTypes(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAs<long>("protocolId");

            if (protocolId == 0)
            {
                return JsFailure("Нет данных по протоколу!");
            }

            var genericDecision =
                _decisionDomain.GetAll()
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .FirstOrDefault(x => x.Protocol.Id == protocolId && x.IsActual);

            if (genericDecision == null)
            {
                var defaultDecisions =
                    DecisionContainer.DecisionTransfers
                        .Return(x => x.Keys.Where(k => k.DecisionType.IsDefault))
                        .ToList();

                var decisionTypes = defaultDecisions.GroupBy(x => x.DecisionType).Select(x => new
                {
                    x.Key.Name,
                    x.Key.Code,
                    x.Key.Js
                });

                return JsSuccess(decisionTypes);
            }

            var decision =
                DecisionContainer.AllDecisions.Return(
                    x => x.FirstOrDefault(d => d.Code == genericDecision.DecisionCode));

            if (decision == null)
            {
                return JsFailure("Не найдено последнее принятое решение");
            }

            var transfers =
                DecisionContainer.DecisionTransfers.Return(
                    x => x.ContainsKey(decision) ? x[decision] : new Collection<IDecision>());

            var grouped = transfers.GroupBy(x => x.DecisionType).Select(x => new
            {
                x.Key.Name,
                x.Key.Code,
                x.Key.Js
            });

            return JsSuccess(grouped);
        }

        public ActionResult GetDecisions(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAs<long>("protocolId");
            var decisionTypeCode = baseParams.Params.GetAs<string>("decisionTypeCode");

            if (decisionTypeCode.IsEmpty())
            {
                return JsFailure("Нет данных по виду решения!");
            }

            if (protocolId == 0)
            {
                return JsFailure("Нет данных по протоколу!");
            }

            var genericDecisionCode =
                _decisionDomain.GetAll()
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .FirstOrDefault(x => x.Protocol.Id == protocolId && x.IsActual).Return(x => x.DecisionCode, RootDecision.RootCode);

            var currentDecision =
                DecisionContainer.AllDecisions.Return(x => x.FirstOrDefault(d => d.Code == genericDecisionCode));

            if (currentDecision == null)
            {
                return JsFailure("Не найдено текущее решение!");
            }

            var possibleTransfers =
                DecisionContainer.DecisionTransfers.Return(
                    x =>
                        x.ContainsKey(currentDecision)
                            ? x[currentDecision].Where(d => d.DecisionType.Code == decisionTypeCode)
                            : new Collection<IDecision>())
                    .Select(x => new
                    {
                        code = x.Code,
                        name = x.Name
                    });

            return JsSuccess(possibleTransfers);
        }

        public ActionResult ApplyDecision(BaseParams baseParams)
        {
            var genericDecision = baseParams.Params.ReadClass<GenericDecision>();

            var protocoId = baseParams.Params.GetAs<long>("Protocol");

            var findDecisionByTypeCode = baseParams.Params.GetAs<bool>("byTypeCode");

            var decisionTypeCode = baseParams.Params.GetAs<string>("typeCode");

            if (genericDecision == null)
            {
                return JsFailure("Неверные входные данные! Отсутствует базоваое решение!");
            }

            genericDecision.Protocol = genericDecision.Protocol ?? Container.Resolve<IDomainService<RealityObjectDecisionProtocol>>().Load(protocoId);

            if (genericDecision.Protocol == null)
            {
                return JsFailure("Неверные входные данные! Отсутствует протокол!");
            }

            var decisionType =
                DecisionContainer.AllTypes.Return(x => x.FirstOrDefault(d => d.Code == decisionTypeCode));

            if (decisionType == null)
            {
                return JsFailure("Не удалось найти вид решение по коду: {0}!".FormatUsing(decisionTypeCode));
            }

            if (findDecisionByTypeCode)
            {
                var code =
                    DecisionContainer.DecisionTransfers.Keys.Union(
                        DecisionContainer.DecisionTransfers.Values.SelectMany(x => x))
                        .FirstOrDefault(x => x.DecisionType.Code == decisionTypeCode)
                        .Return(x => x.Code);

                genericDecision.DecisionCode = code;
            }

            if (baseParams.Files.Any())
            {
                genericDecision.File = Container.Resolve<IFileManager>().SaveFile(baseParams.Files.FirstOrDefault().Value);
            }

            var name = DecisionContainer.AllDecisions.FirstOrDefault(x => x.Code == genericDecision.DecisionCode)
                .Return(x => x.Name);

            baseParams.Params.Add("DecisionName", name);
            
            decisionType.AcceptDecicion(genericDecision, baseParams);
            _decisionDomain.Save(genericDecision);

            return JsSuccess();
        }

        public ActionResult GetHistoryTree(BaseParams baseParams)
        {
            var result = Resolve<IDecisionHistoryService>().GetHistoryTree(baseParams);

            return new JsonNetResult(result.Data);
        }

        public ActionResult GetJobYearsHistory(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IDecisionHistoryService>().GetJobYearsHistory(baseParams);

            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        private readonly IDomainService<GenericDecision> _decisionDomain;
        private IDecisionContainer _decisionContainer;
    }
}