namespace Bars.GkhGji.ViewModel.Email
{
    using Bars.B4;
    using Bars.GkhGji.Entities.Email;
    using System.Linq;

    public class EmailGjiAttachmentViewModel : BaseViewModel<EmailGjiAttachment>
    {
        public override IDataResult List(IDomainService<EmailGjiAttachment> domain, BaseParams baseParams)
        {          
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("GjiEmail", 0L);
            var data = domain.GetAll()
                .Where(x => x.Message.Id == id)
                .Select(
                x => new
                {
                    x.Id,
                    x.AttachmentFile
                }).Filter(loadParams, Container); 

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}