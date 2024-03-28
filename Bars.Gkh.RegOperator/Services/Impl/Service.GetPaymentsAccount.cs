namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Services.DataContracts.PaymentsAccount;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Utils;

    public partial class Service
    {
        /// <summary>
        /// Получить все оплаты по ЛС
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public PaymentsAccountResponse GetPaymentsAccount(string accountNumber)
        {
            var service = this.Container.Resolve<IPersonalAccountService>();
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var unconfirmedPaymentsDomain = this.Container.ResolveDomain<UnconfirmedPayments>();

            try
            {
                var accountId = accountDomain.GetAll().Where(x => x.PersonalAccountNum == accountNumber).Select(x => x.Id).FirstOrDefault();

                if (accountId == 0)
                {
                    return new PaymentsAccountResponse() {Result = Result.DataNotFound};
                }

                var payments = service.ListPayments(accountId).Select(
                    x => new PaymentsAccount
                    {
                        NumDocument = string.IsNullOrEmpty(x.DocumentNum)? "N/A": x.DocumentNum,
                        DateDocument = x.DocumentDate.HasValue ? x.DocumentDate.Value.ToShortDateString() : x.PaymentDate.ToShortDateString(),
                        DatePay = x.PaymentDate.ToShortDateString(),
                        TypePay = x.Reason,
                        Sum = x.Amount.RegopRoundDecimal(2),
                        NamePa = x.PaymentAgentName,
                        PaymentState = "Обработан"
                    }).ToList();
                try
                {

                    payments.AddRange
                        (
                        unconfirmedPaymentsDomain.GetAll()
                        .Where(x => x.PersonalAccount.Id == accountId)
                        .Where(x=> x.PaymentDate.HasValue && x.PaymentDate.Value > DateTime.Now.AddDays(-5))
                        .Where(x => x.IsConfirmed == Gkh.Enums.YesNo.No)
                        .Select(x => new PaymentsAccount
                        {
                            DateDocument = x.ObjectCreateDate.ToShortDateString(),
                            DatePay = x.ObjectCreateDate.ToShortDateString(),
                            NamePa = x.BankName,
                            NumDocument = x.Guid,
                            Sum = x.Sum,
                            PaymentState = "В обработке",
                            TypePay = "Оплата взносов КР"
                        })
                        );
                }
                catch(Exception e)
                {

                }

                return new PaymentsAccountResponse
                {
                    PaymentsAccount = payments.ToArray(),
                    Result = Result.NoErrors
                };
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(accountDomain);
                this.Container.Release(unconfirmedPaymentsDomain);                
            }
        }
    }
}