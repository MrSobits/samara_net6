namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Валидатор для проверки соответствия суммы распределения и суммы в слепке
    /// </summary>
    public class PersonalAccountDistributionPaymentDocumentValidator : IDistributionValidator
    {
        public IDomainService<PaymentDocumentSnapshot> DocumentSnapshotDomain { get; set; }

        public IPaymentDocumentService PaymentDocumentService { get; set; }

        public string Code => nameof(TransferCrDistribution);

        public bool IsMandatory => false;

        public bool IsApply => true;
        
        public IDataResult Validate(BaseParams baseParams)
        {
            var snapshotId = baseParams.Params.GetAs<long>("snapshotId");
            var distribSum = baseParams.Params.GetAs<decimal>("distribSum");

            if (baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionView") != SuspenseAccountDistributionParametersView.ByPaymentDocument)
            {
                return new BaseDataResult();
            }

            var snapshotSum = this.PaymentDocumentService.GetPaymentInfoSnapshots(snapshotId)
                .SafeSum(x => x.ChargeSum + x.PenaltySum);

            if (distribSum < snapshotSum)
            {
                return BaseDataResult.Error(
                    $"Распределяемая сумма ({distribSum}) меньше выставленной суммы ({snapshotSum.RegopRoundDecimal(2)}) из квитанции. В этом случае оплате будет присвоен статус \"Частично оплачено\"");
            }
            else
            {
                return BaseDataResult.Error(
                    $"Распределяемая сумма ({distribSum}) больше или равна выставленной суммы ({snapshotSum.RegopRoundDecimal(2)}) из квитанции. В этом случае оплате будет присвоен статус \"Оплачено\"");
            }
        }
    }
}