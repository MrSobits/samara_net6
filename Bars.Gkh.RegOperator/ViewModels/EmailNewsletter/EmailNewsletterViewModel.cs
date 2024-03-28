namespace Bars.Gkh.RegOperator.ViewModels.EmailNewsletter
{
    using B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Domain;
    using System.Linq;

    public class EmailNewsletterViewModel : BaseViewModel<EmailNewsletter>
    {
        public override IDataResult List(IDomainService<EmailNewsletter> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.SendDate,
                    x.Header,
                    x.Body,
                    x.Destinations,
                    x.Success,
                    x.Attachment,
                    x.Sender
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
