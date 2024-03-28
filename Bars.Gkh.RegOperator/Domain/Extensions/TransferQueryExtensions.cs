namespace Bars.Gkh.RegOperator.Domain.Extensions
{
    using System.Linq;
    using B4.Utils.Annotations;
    using Entities.ValueObjects;
    using Repository.RealityObjectAccount;

    public static class TransferQueryExtensions
    {
        public static IQueryable<TransferDto> TranslateToDto(this IQueryable<Transfer> query)
        {
            ArgumentChecker.NotNull(query, "query");

            return query
                .Select(x => new TransferDto
                {
                    Id = x.Id,
                    OwnerId = x.Owner.Id,
                    Reason = x.Reason ?? x.Originator.Reason,
                    OperationDate = x.PaymentDate,
                    Amount = x.Operation.CanceledOperation != null || x.IsReturnLoan
                        ? -1 * x.Amount
                        : x.Amount,
                    OriginatorName = x.OriginatorName
                });
        }
    }
}