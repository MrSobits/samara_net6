namespace Bars.GkhGji.ViewModel.SurveyPlan
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities.SurveyPlan;

    public class SurveyPlanContragentAttachmentViewModel : BaseViewModel<SurveyPlanContragentAttachment>
    {
        public override IDataResult List(
            IDomainService<SurveyPlanContragentAttachment> domainService,
            BaseParams baseParams)
        {
            var contragentId = baseParams.Params.GetAsId("contragentId");
            if (contragentId < 1)
            {
                return BaseDataResult.Error("Не передан идентификатор контрагента");
            }

            var loadParams = GetLoadParam(baseParams);
            var attachments =
                domainService.GetAll()
                             .Where(x => x.SurveyPlanContragent.Id == contragentId)
                             .Filter(loadParams, Container)
                             .Order(loadParams);

            return new ListDataResult(attachments.Paging(loadParams), attachments.Count());
        }
    }
}