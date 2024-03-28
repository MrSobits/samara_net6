namespace Bars.Gkh.RegOperator.ViewModels
{
    using B4;
    using Gkh.Decisions.Nso.Entities.Decisions;
    using Gkh.Domain;

    public class GovDecisionViewModel : Gkh.Decisions.Nso.ViewModel.GovDecisionViewModel
    {
        public override IDataResult Get(IDomainService<GovDecision> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            var payDecision = new PaymentAndFundDecisions();

            if (obj.RealityObject != null)
            {
                payDecision.Init(Container, obj.RealityObject.Municipality.Id);
            }
            
            return new BaseDataResult(new
            {
                obj.Id,
                obj.AuthorizedPerson,
                obj.AuthorizedPersonPhone,
                obj.Destroy,
                obj.DestroyDate,
                obj.DateStart,
                obj.FundFormationByRegop,
                obj.MaxFund,
                obj.ProtocolDate,
                obj.ProtocolFile,
                obj.ProtocolNumber,
                RealityObject = obj.RealityObject.Id,
                obj.RealtyManagement,
                obj.Reconstruction,
                obj.ReconstructionEnd,
                obj.ReconstructionStart,
                obj.State,
                obj.TakeApartsForGov,
                obj.TakeApartsForGovDate,
                obj.TakeLandForGov,
                obj.TakeLandForGovDate,
                PaymentAndFundDecisions = payDecision,
                obj.LetterNumber,
                obj.LetterDate,
                obj.NpaName,
                obj.NpaDate,
                obj.NpaNumber,
                obj.TypeInformationNpa,
                obj.TypeNpa,
                obj.TypeNormativeAct,
                obj.NpaContragent,
                obj.NpaFile,
                obj.NpaStatus,
                obj.NpaCancellationReason
            });
        }
    }
}