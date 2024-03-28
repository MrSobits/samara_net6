namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using Entities;
    using Modules.ClaimWork.DomainService;

    public class ActViolIdentificationInterceptor : EmptyDomainInterceptor<ActViolIdentificationClw>
    {
        public override IDataResult AfterDeleteAction(IDomainService<ActViolIdentificationClw> service, ActViolIdentificationClw entity)
        {
            var clwService = this.Container.ResolveAll<IBaseClaimWorkService>()
                .FirstOrDefault(x => x.ClaimWorkTypeBase == entity.ClaimWork.ClaimWorkTypeBase);

            if (clwService != null)
            {
                var newParams = new DynamicDictionary
                {
                    { "id", entity.ClaimWork.Id }
                };
                var baseParams = new BaseParams { Params = newParams };

                clwService.UpdateStates(baseParams);
            }

            return this.Success();
        }
    }
}