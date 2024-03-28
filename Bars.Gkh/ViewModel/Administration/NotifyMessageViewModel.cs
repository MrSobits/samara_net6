namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Administration.Notification;
    using Bars.Gkh.Utils;

    public class NotifyMessageViewModel : BaseViewModel<NotifyMessage>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<NotifyMessage> domainService, BaseParams baseParams)
        {
            var today = DateTime.Today;
            return domainService.GetAll()
                .Where(x => !x.IsDelete)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.ObjectEditDate,
                    IsActual = x.StartDate <= today && today <= x.EndDate
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}