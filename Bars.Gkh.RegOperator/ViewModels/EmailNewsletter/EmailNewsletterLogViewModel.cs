namespace Bars.Gkh.RegOperator.ViewModels.EmailNewsletter
{
    using B4;
    using Bars.Gkh.RegOperator.Entities;
    using System.Linq;

    public class EmailNewsletterLogViewModel : BaseViewModel<EmailNewsletterLog>
    {
        public override IDataResult List(IDomainService<EmailNewsletterLog> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var emailNewsletterId = loadParam.Filter.GetAs<long>("emailNewsletterId");

            var data = domainService.GetAll()
                .Where(x => x.EmailNewsletter.Id == emailNewsletterId)
                .Select(x => new
                {
                    x.Id,
                    x.EmailNewsletter,
                    x.Destination,
                    x.Log,
                    x.Success
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
