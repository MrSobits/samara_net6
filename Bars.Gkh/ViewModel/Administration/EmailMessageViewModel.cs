using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.Administration;
using Bars.Gkh.Utils;
using System;
using System.Linq;

namespace Bars.Gkh.ViewModel.Administration
{
    public class EmailMessageViewModel : BaseViewModel<EmailMessage>
    {
        public override IDataResult List(IDomainService<EmailMessage> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.Params;
            var beginDate = loadParams.GetAs<DateTime>("beginDate").Date;
            var endDate = loadParams.GetAs<DateTime>("endDate").Date;

            return domainService.GetAll()
                .Where(x => x.SendingTime.Date >= beginDate && x.SendingTime.Date <= endDate)
                .Select(x => new
                {
                    x.Id,
                    x.EmailMessageType,
                    RecipientName = x.RecipientContragent.Name,
                    x.EmailAddress,
                    x.AdditionalInfo,
                    x.SendingTime,
                    x.SendingStatus,
                    LogFileId = x.LogFile != null ? x.LogFile.Id : default(long?)
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}