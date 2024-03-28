namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class EmailListsViewModel : BaseViewModel<EmailLists>
    {
        public override IDataResult List(IDomainService<EmailLists> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.AnswerNumber,
                    x.AppealDate,
                    x.AppealNumber,
                    x.FileInfo,
                    x.MailTo,
                    x.SendDate
                }).OrderByDescending(x=> x.Id)
             .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
