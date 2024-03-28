namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using DomainService.PersonalAccount;
    using Entities;
    using Enums;
    using Gkh.Domain;

    [Obsolete]
    public class SetNewOwnerAccountOperation : PersonalAccountOperationBase
    {
        public IWindsorContainer Container { get; set; }
        public IPersonalAccountOperationService OperationService { get; set; }
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountOwner> AccountOwnerDomain { get; set; }
        
        public static string Key
        {
            get { return "SetNewOwnerAccountOperation"; }
        }

        public override string Code
        {
            get { return Key; }
        }

        public override string Name
        {
            get { return "Смена абонента"; }
        }

        public override IDataResult Execute(BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAsId("accId");
            var newOwnerId = baseParams.Params.GetAsId("newOwnerId");
            var actualFrom = baseParams.Params.GetAs<DateTime>("actualFrom");

            var acc = this.PersonalAccountDomain.Get(accId);

            if (acc == null)
            {
                return new BaseDataResult(false, "Не удалось получить лицевой счет");
            }

            if (acc.AccountOwner.Id == newOwnerId)
            {
                return new BaseDataResult(false, "Необходимо выбрать другого владельца счета!");
            }

            var oldOwner = acc.AccountOwner;
            var newOwner = this.AccountOwnerDomain.Get(newOwnerId);

            acc.AccountOwner = newOwner;

            this.Container.InTransaction(
                () =>
                {
                    this.OperationService.CreateAccountHistory(
                        acc,
                        PersonalAccountChangeType.OwnerChange,
                        () => string.Format(
                            "Смена абонента ЛС с \"{0}\" на \"{1}\"",
                            oldOwner.Return(x => x.Name),
                            newOwner.Return(x => x.Name)),
                        actualFrom);

                    this.PersonalAccountDomain.Update(acc);
                });

            return new BaseDataResult(true, "Смена владельца счета - успешно!");
        }
    }
}
