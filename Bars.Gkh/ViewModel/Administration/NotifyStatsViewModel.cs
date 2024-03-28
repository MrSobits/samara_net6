namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.Notification;
    using Bars.Gkh.Utils;

    public class NotifyStatsViewModel : BaseViewModel<NotifyStats>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<NotifyStats> domainService, BaseParams baseParams)
        {
            var messageId = baseParams.Params.GetAsId("messageId");

            return domainService.GetAll()
                .Where(x => x.Message.Id == messageId)
                .Select(x => new
                {
                    x.Id,
                    x.ObjectCreateDate,
                    x.User.Login,
                    x.User.Name,
                    x.ClickButton
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}