namespace Bars.GkhGji.DomainService
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;

    using Castle.Windsor;

    public class DocumentGjiService : IDocumentGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult StateChange(BaseParams baseParams)
        {
            var newStateId = baseParams.Params.ContainsKey("newStateId") ? baseParams.Params["newStateId"].ToLong() : 0;
            var entityId = baseParams.Params.ContainsKey("entityId") ? baseParams.Params["entityId"].ToLong() : 0;
            var description = baseParams.Params.ContainsKey("description") ? baseParams.Params["description"].ToString() : "";

            if (!(entityId > 0 && newStateId > 0))
            {
                return new BaseDataResult { Success = false, Message = "Ошибка переданных параметров" };
            }

            var newState = Container.Resolve<IDomainService<State>>().Load(newStateId);

            try
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.ChangeState(entityId, newState.TypeId, newState, description, true);
            }
            catch (Exception exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }

            return new BaseDataResult(new { message = "Статус успешно переведен", newState });
        }
    }
}