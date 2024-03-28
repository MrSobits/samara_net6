namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Modules.ClaimWork.DomainService;

    public class PretensionInterceptor : EmptyDomainInterceptor<PretensionClw>
    {
        public override IDataResult AfterDeleteAction(IDomainService<PretensionClw> service, PretensionClw entity)
        {
            var clwService = Container.ResolveAll<IBaseClaimWorkService>()
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

            return Success();
        }
    }
}