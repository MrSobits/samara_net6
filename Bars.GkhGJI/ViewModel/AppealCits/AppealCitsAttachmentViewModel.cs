namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsAttachmentViewModel: BaseViewModel<AppealCitsAttachment>
    {
        public override IDataResult List(IDomainService<AppealCitsAttachment> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var appealCitizensId = loadParams.Filter.GetAsId("appealCitizensId");

            return domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}