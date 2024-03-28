namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlListTypicalQuestionViewModel : BaseViewModel<ControlListTypicalQuestion>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ControlListTypicalQuestion> domainService, BaseParams baseParams)
        {
            var needWithoutMandatory = baseParams.Params.GetAs<bool>("needWithoutMandatory");
            var mandatoryReqId = baseParams.Params.GetAsId("mandatoryReqId");

            return domainService.GetAll()
                .WhereIf(needWithoutMandatory, x=> x.MandatoryRequirement == null)
                .WhereIf(mandatoryReqId != default(long), x => x.MandatoryRequirement.Id == mandatoryReqId)
                .Select(x =>new
                {
                    x.Id,
                    x.Question,
                    x.NormativeDoc,
                    x.MandatoryRequirement,
                    NpaName = x.NormativeDoc.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
