namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.Notification;
    using Bars.Gkh.Utils;

    public class NotifyPermissionViewModel : BaseViewModel<NotifyPermission>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<NotifyPermission> domainService, BaseParams baseParams)
        {
            var messageId = baseParams.Params.GetAsId("messageId");

            return domainService.GetAll()
                .Where(x => x.Message.Id == messageId)
                .Select(x => new
                {
                    x.Id,
                    Role = x.Role.Id,
                    RoleName = x.Role.Name
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}