namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Decision
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    public class DecisionViewModel : TatarstanDisposalViewModel<TatarstanDecision>
    {
        public override IDataResult Get(IDomainService<TatarstanDecision> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var decision = domainService.Get(id);
            var result = this.GetDtoWithDefaultProperties(decision).CopyIdenticalProperties<TatarstanDisposalDto, DecisionDto>();

            result.SendToErknm = string.IsNullOrEmpty(decision.ErknmGuid) ? YesNo.No : YesNo.Yes;
            result.ErknmId = decision.ErknmGuid;
            result.ErknmRegistrationNumber = decision.ErknmRegistrationNumber;
            result.ErknmRegistrationDate = decision.ErknmRegistrationDate;
            result.DecisionPlace = decision.DecisionPlace;
            result.SubmissionDate = decision.SubmissionDate;
            result.ReceiptDate = decision.ReceiptDate;
            result.UsingMeansRemoteInteraction = decision.UsingMeansRemoteInteraction;
            result.InfoUsingMeansRemoteInteraction = decision.InfoUsingMeansRemoteInteraction;
            
            return new BaseDataResult(result);
        }

        private class DecisionDto : TatarstanDisposalDto
        {
            public YesNo SendToErknm { get; set; }
            public string ErknmRegistrationNumber { get; set; }
            public DateTime? ErknmRegistrationDate { get; set; }
            public FiasAddress DecisionPlace { get; set; }
            public string ErknmId { get; set; }
            public DateTime? SubmissionDate { get; set; }
            public DateTime? ReceiptDate { get; set; }
            public YesNoNotSet UsingMeansRemoteInteraction { get; set; }
            public string InfoUsingMeansRemoteInteraction { get; set; }
        }
    }
}