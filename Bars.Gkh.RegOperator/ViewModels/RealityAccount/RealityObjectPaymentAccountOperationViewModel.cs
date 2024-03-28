namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;

    using B4.Utils;

    using Entities;
    using Enums;
    using Gkh.Domain;

    public class RealityObjectPaymentAccountOperationViewModel : BaseViewModel<RealityObjectPaymentAccountOperation>
    {
        public override IDataResult List(IDomainService<RealityObjectPaymentAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var isCredit = loadParams.Filter.GetAs<bool>("isCredit");
            var accountId = baseParams.Params.GetAsId("accId");

            if (accountId == 0)
            {
                accountId = loadParams.Filter.GetAsId("accId");
            }

            var data =
                domainService.GetAll()
                    .Where(x => x.Account.Id == accountId)
                    .Select(x => new
                    {
                        x.Id,
                        AccId = x.Account.Id,
                        x.OperationType,
                        x.Date,
                        x.OperationStatus,
                        x.OperationSum
                    })
                    .WhereIf(isCredit,
                        x =>
                            x.OperationType == PaymentOperationType.OutcomeAccountPayment
                            || x.OperationType == PaymentOperationType.ExpenseLoan
                            || x.OperationType == PaymentOperationType.OutcomeLoan
                            || x.OperationType == PaymentOperationType.OpeningAcc
                            || x.OperationType == PaymentOperationType.CashService
                            || x.OperationType == PaymentOperationType.CreditPayment
                            || x.OperationType == PaymentOperationType.CreditPercentPayment
                            || x.OperationType == PaymentOperationType.GuaranteesObtainPayment
                            || x.OperationType == PaymentOperationType.GuaranteesForCredit
                            || x.OperationType == PaymentOperationType.CancelPayment
                            || x.OperationType == PaymentOperationType.UndoPayment)
                    .WhereIf(!isCredit,
                        x =>
                            x.OperationType != PaymentOperationType.OutcomeAccountPayment
                            && x.OperationType != PaymentOperationType.ExpenseLoan
                            && x.OperationType != PaymentOperationType.OutcomeLoan
                            && x.OperationType != PaymentOperationType.CashService
                            && x.OperationType != PaymentOperationType.OpeningAcc
                            && x.OperationType != PaymentOperationType.CreditPayment
                            && x.OperationType != PaymentOperationType.CreditPercentPayment
                            && x.OperationType != PaymentOperationType.GuaranteesObtainPayment
                            && x.OperationType != PaymentOperationType.GuaranteesForCredit
                            && x.OperationType != PaymentOperationType.CancelPayment
                            && x.OperationType != PaymentOperationType.UndoPayment)
                    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}