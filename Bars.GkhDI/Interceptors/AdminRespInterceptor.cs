namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class AdminRespInterceptor : EmptyDomainInterceptor<AdminResp>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<AdminResp> service, AdminResp entity)
        {
            var actionsService = this.Container.Resolve<IDomainService<Actions>>();
            var actionsIds = actionsService.GetAll().Where(x => x.AdminResp.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in actionsIds)
            {
                actionsService.Delete(id);
            }

            return Success();
        }
    }
}
