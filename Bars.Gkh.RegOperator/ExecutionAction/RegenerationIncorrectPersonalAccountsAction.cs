namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.AccountNumberGenerator;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    public class RegenerationIncorrectPersonalAccountsAction : BaseExecutionAction
    {
        public override string Description
            => "Перегенерация ЛС, при неверном количестве символов: 9 или 17 знаков в зависисимости от параметров (КАМАЧАТКА)";

        public override string Name => "Перегенерация ЛС, при неверном количестве символов (КАМЧАТКА)";

        public override Func<IDataResult> Action => this.RegenerationIncorrectPersonalAccounts;

        private BaseDataResult RegenerationIncorrectPersonalAccounts()
        {
            var basePersonalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var regopParamsDomain = this.Container.ResolveDomain<RegoperatorParam>();

            TypeAccountNumber typeAccountNumber = 0;

            using (this.Container.Using(regopParamsDomain, basePersonalAccountDomain))
            {
                typeAccountNumber = regopParamsDomain.GetAll()
                    .FirstOrDefault(x => x.Key == "TypeAccountNumber")
                    .Return(x => x.Value)
                    .Return(x => x.ToEnum(TypeAccountNumber.Short));

                var generator = this.Container.Resolve<IAccountNumberGenerator>(
                    typeAccountNumber == TypeAccountNumber.Long
                        ? "LongAccountNumberGenerator"
                        : "ShortAccountNumberGenerator");

                var incorrectPersAccounts = new List<BasePersonalAccount>();

                switch (typeAccountNumber)
                {
                    case TypeAccountNumber.Short:
                        incorrectPersAccounts = basePersonalAccountDomain.GetAll().Where(x => x.PersonalAccountNum.Length > 9).ToList();
                        break;
                    case TypeAccountNumber.Long:
                        incorrectPersAccounts = basePersonalAccountDomain.GetAll().Where(x => x.PersonalAccountNum.Length < 17).ToList();
                        break;
                }

                foreach (var incorrectPersAccount in incorrectPersAccounts)
                {
                    generator.Generate(incorrectPersAccount);
                    basePersonalAccountDomain.Save(incorrectPersAccount);
                }
            }

            return new BaseDataResult();
        }
    }
}