namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System.Linq;

    public class MKDLicRequestAnswerAttachmentViewModel : BaseViewModel<MKDLicRequestAnswerAttachment>
    {
        public override IDataResult List(IDomainService<MKDLicRequestAnswerAttachment> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var answerId = loadParams.Filter.GetAsId("answerId");

            var data = domainService.GetAll()
                .Where(x => x.MKDLicRequestAnswer.Id == answerId)
                .Select(x => new
                {
                    x.Id,
                    x.MKDLicRequestAnswer,
                    x.Name,
                    x.Description,
                    x.FileInfo
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);

            return data;
        }
    }
}