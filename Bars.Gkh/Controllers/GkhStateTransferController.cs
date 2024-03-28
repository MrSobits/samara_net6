namespace Bars.Gkh.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Domain;

    public class GkhStateTransferController : BaseController
    {
        public IGkhRuleChangeStatusService Service { get; set; }

        /// <summary>
        /// Проверить на наличие правил перехода статусов с сохранением пользовательской формы 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult HasGkhStateRule(BaseParams baseParams)
        {
            var typeId = baseParams.Params.GetAs<string>("typeId");
            var newStateId = baseParams.Params.GetAsId("newStateId");
            var entityId = baseParams.Params.GetAsId("entityId");

            var entity = Service.GetEntity(typeId, entityId);
            var gkhStateRules = Service.GetGkhRuleStatus(typeId, newStateId, entity);

            return new JsonNetResult(gkhStateRules.Any());
        }


        /// <summary>
        /// Изменить статус
        /// </summary>
        /// <param name="baseParams"> </param>
        /// <returns></returns>
        public ActionResult StateChange(BaseParams baseParams)
        {
            var typeId = baseParams.Params.GetAs<string>("typeId");
            var newStateId = baseParams.Params.GetAsId("newStateId");
            var entityId = baseParams.Params.GetAsId("entityId");
            var description = baseParams.Params.GetAs<string>("description");

            var stateProvider = Container.Resolve<IStateProvider>();
            var stateDomain = Container.ResolveDomain<State>();
            try
            {
                var newState = stateDomain.Load(newStateId);

                var entity = Service.GetEntity(typeId, entityId);
                var gkhStateRules = Service.GetGkhRuleStatus(typeId, newStateId, entity);

                gkhStateRules.ForEach(x => x.HandleUserParams(baseParams, entity));
                stateProvider.ChangeState(entityId, newState.TypeId, newState, description, true);

                return new JsonNetResult(new {success = true, message = "Статус успешно переведен", newState});
            }
            catch (StateException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
            finally
            {
                Container.Release(stateProvider);
                Container.Release(stateDomain);
            }
        }

    }
}