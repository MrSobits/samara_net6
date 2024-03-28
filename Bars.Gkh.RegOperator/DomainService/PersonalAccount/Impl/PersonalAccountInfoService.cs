namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Linq;
    using B4.Utils;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Enums;
    using Wcf.Contracts.PersonalAccount;
    using Castle.Windsor;

    public class PersonalAccountInfoService : IPersonalAccountInfoService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }

        public IDomainService<PersonalAccountPayment> PersonalAccountPayment { get; set; }

        public PersonalAccountInfoOut GetPersonalAccountInfo(PersonalAccountInfoIn personalAccountInfo)
        {
            var account = GetAccount(personalAccountInfo);

            if (account == null)
            {
                return null;
            }

            var accountChargeInfo = PersonalAccountChargeDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == account.Id && x.IsFixed)
                .Select(x => new
                {
                    x.Penalty,
                    Recalc = x.RecalcByBaseTariff,
                    x.ChargeTariff
                })
                .ToList();

            var accountPaymentInfo = PersonalAccountPayment.GetAll()
                .Where(x => x.BasePersonalAccount.Id == account.Id)
                .Select(x => new
                {
                    x.Sum,
                    x.Type
                })
                .AsEnumerable()
                .GroupBy(x => x.Type)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));

            var paymentTariff = accountPaymentInfo.Get(PaymentType.Basic);
            var paymentPenalty = accountPaymentInfo.Get(PaymentType.Penalty);
            var chargedTariff = accountChargeInfo.Sum(x => (decimal?) (x.ChargeTariff + x.Recalc)).GetValueOrDefault();
            var chargedPenaltyTotal = accountChargeInfo.Sum(x => (decimal?)x.Penalty).GetValueOrDefault();
            var debtTotal = chargedTariff - paymentTariff;
            var debtPenalty = chargedPenaltyTotal - paymentPenalty;

            return new PersonalAccountInfoOut
            {
                AccountNumber = personalAccountInfo.AccountNumber,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate,
                FeeDebtTotal = chargedPenaltyTotal,
                TotalDebt = debtTotal,
                TotalPenaltyDebt = debtPenalty
            };
        }

        protected BasePersonalAccount GetAccount(PersonalAccountInfoIn personalAccountInfo)
        {
            if (personalAccountInfo.AccountNumber.IsEmpty())
            {
                return null;
            }

            BasePersonalAccount account = null;

            var accountService = Container.Resolve<IDomainService<BasePersonalAccount>>();
            var indivAccountOwnerService = Container.ResolveDomain<IndividualAccountOwner>();
            var legalAccountOwnerService = Container.ResolveDomain<LegalAccountOwner>();

            if (!personalAccountInfo.Inn.IsEmpty() && !personalAccountInfo.Kpp.IsEmpty())
            {
                account = accountService.GetAll()
                    .Where(x => x.PersonalAccountNum == personalAccountInfo.AccountNumber)
                .Join(legalAccountOwnerService.GetAll(),
                    x => x.AccountOwner.Id,
                    y => y.Id,
                    (x, y) => new { account = x, owner = y })
                .Where(x => x.owner.Contragent != null && x.account.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal)
                .Where(x => x.owner.Contragent.Inn == personalAccountInfo.Inn)
                .Where(x => x.owner.Contragent.Kpp == personalAccountInfo.Kpp)
                .Select(x => x.account)
                .FirstOrDefault();
            }
            else if(!personalAccountInfo.Fio.IsEmpty())
            {
                var splitedFio = personalAccountInfo.Fio.Split(' ').Select(x => x.ToLower().Trim()).ToArray();

                if (splitedFio.Length >= 3)
                {
                    account = accountService.GetAll()
                        .Join(indivAccountOwnerService.GetAll(),
                            x => x.AccountOwner.Id,
                            y => y.Id,
                            (x, y) => new {account = x, owner = y})
                        .Where(x => x.account.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                        .Select(x => new
                        {
                            x.account,
                            Surname = x.owner.Surname.ToLower().Trim(),
                            FirstName = x.owner.FirstName.ToLower().Trim(),
                            SecondName = x.owner.SecondName.ToLower().Trim()
                        })
                        .Where(x => x.Surname == splitedFio[0])
                        .Where(x => x.FirstName == splitedFio[1])
                        .Where(x => x.SecondName == splitedFio[2])
                        .Select(x => x.account)
                        .FirstOrDefault();
                }
            }

            return account;
        }
    }
}