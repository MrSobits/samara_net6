namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Entities;

    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.DomainService;

    public class NotificationInterceptor : EmptyDomainInterceptor<NotificationClw>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<NotificationClw> service, NotificationClw entity)
        {
            var documentDomain = this.Container.ResolveDomain<DocumentClwAccountDetail>();
            using (this.Container.Using(documentDomain))
            {
                documentDomain.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => documentDomain.Delete(x));
            }
            return this.Success();
        }
        
        public override IDataResult AfterDeleteAction(IDomainService<NotificationClw> service, NotificationClw entity)
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
            return Success();
        }
    }
}