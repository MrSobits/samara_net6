namespace Bars.GkhGji.ViewModel.Email
{
    using Bars.B4;
    using Bars.GkhGji.Entities.Email;
    using System.Linq;

    public class EmailGjiViewModel : BaseViewModel<EmailGji>
    {
        public override IDataResult List(IDomainService<EmailGji> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(
                x => new
                {
                    x.Id,
                    x.From,
                    x.SenderInfo,
                    x.Theme,
                    x.EmailGjiSource,
                    x.EmailDate,
                    x.GjiNumber,
                    x.EmailType,
                    x.Registred,
                    x.Description,
                    x.EmailDenailReason,
                    x.EmailPdf
                }).Filter(loadParams, Container); 

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}