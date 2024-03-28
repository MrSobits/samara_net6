namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsAnswerAttachmentViewModel: BaseViewModel<AppealCitsAnswerAttachment>
    {
        public override IDataResult List(IDomainService<AppealCitsAnswerAttachment> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var answerId = loadParams.Filter.GetAsId("answerId");

            return domainService.GetAll()
                .Where(x => x.AppealCitsAnswer.Id == answerId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.FileInfo
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}