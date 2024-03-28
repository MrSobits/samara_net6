namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using B4.Utils;
    using DomainService.PersonalAccount;
    using Entities;
    using Enums;
    using Gkh.Domain;

    public class CloseAccountOperation : PersonalAccountOperationBase
    {
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }
        public IDomainService<PersonalAccountPayment> PersonalAccountPaymentDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IStateProvider StateProvider { get; set; }
        public IPersonalAccountOperationService OperationService { get; set; }

       
        public static string Key
        {
            get { return "CloseAccountOperation"; }
        }

        public override string Name
        {
            get { return "Закрытие"; }
        }

        public override string Code
        {
            get { return Key; }
        }

        public override IDataResult Execute(BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAsId("accId");
            var closeDate = baseParams.Params.GetAs<DateTime>("closeDate");
            var acc = PersonalAccountDomain.Get(accId);

            return CloseAccount(acc, PersonalAccountChangeType.Close, closeDate);
        }

        private IDataResult CloseAccount(BasePersonalAccount acc, PersonalAccountChangeType type, DateTime? closeDate = null)
        {
            if (acc.State.FinalState)
            {
                return new BaseDataResult(false, "Счет уже закрыт!");
            }

            if (acc.AreaShare > 0)
            {
                return new BaseDataResult(false, "Доля собственности должна быть равна нулю!");
            }

            var chargedTariff = PersonalAccountChargeDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == acc.Id && x.IsFixed)
                .GroupBy(x => 1)
                .Select(x => new
                {
                    Recalc = x.Sum(y => (decimal?)y.RecalcByBaseTariff),
                    ChargeTariff = x.Sum(y => (decimal?)y.ChargeTariff)
                })
                .AsEnumerable()
                .FirstOrDefault();

            var paymentTariff = PersonalAccountPaymentDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == acc.Id)
                .Where(x => x.Type == PaymentType.Basic)
                .Sum(x => (decimal?)x.Sum) ?? 0;

            var debtTotal = chargedTariff.Return(x => (x.Recalc ?? 0) + (x.ChargeTariff ?? 0)) - paymentTariff;

            var entityInfo = StateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

            var stateQuery = StateDomain.GetAll().Where(x => x.TypeId == entityInfo.TypeId);

            if (debtTotal > 0)
            {
                if (acc.State.Name != null && (acc.State.Code == "2" || acc.State.Code == "3"))
                {
                    return new BaseDataResult(false, "Имеется долг. Счет уже закрыт с долгом!");
                }

                var closeState = stateQuery.FirstOrDefault(x => x.Name.ToLower() == "закрыт с долгом");

                if (closeState == null)
                {
                    return new BaseDataResult(false, "Для лицевого счета не определен статус с названием \"Закрыт с долгом\"");
                }

                acc.State = closeState;
            }
            else
            {
                var closeState = stateQuery.FirstOrDefault(x => x.FinalState && x.Name.ToLower() == "закрыт");

                if (closeState == null)
                {
                    return new BaseDataResult(false, "Для лицевого счета не определен конечный статус с названием \"Закрыт\"");
                }

                acc.State = closeState;
            }

            acc.SetCloseDate(closeDate ?? DateTime.Now);

            this.OperationService.CreateAccountHistory(acc, type, null);

            PersonalAccountDomain.Update(acc);

            return new BaseDataResult(true, "Счет закрыт успешно!");
        }
    }
}