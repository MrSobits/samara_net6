namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using AccountNumberGenerator;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;

    using Castle.MicroKernel;

    using Gkh.Domain.Cache;

    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Utils;

    public class AccountNumberService : IAccountNumberService
    {
        private readonly IWindsorContainer _сontainer;

        private readonly TypeAccountNumber _typeAccountNumber;

        private IAccountNumberGenerator _generator;

        public AccountNumberService(IWindsorContainer сontainer)
        {
            _сontainer = сontainer;
            if (_typeAccountNumber.As<int>() == 0)
            {
                var regopParamsDomain = сontainer.ResolveDomain<RegoperatorParam>();

                using (сontainer.Using(regopParamsDomain))
                {
                    _typeAccountNumber = regopParamsDomain.GetAll()
                        .FirstOrDefault(x => x.Key == "TypeAccountNumber")
                        .Return(x => x.Value)
                        .Return(x => x.ToEnum(TypeAccountNumber.Short));
                }
            }
        }

        public void Generate(BasePersonalAccount account)
        {
            var generator = GetGenerator(false);

            generator.Generate(account);
        }

        public void Generate(ICollection<BasePersonalAccount> accounts)
        {
            var generator = GetGenerator();

            generator.Generate(accounts);
        }

        private IAccountNumberGenerator GetGenerator(bool useCache = true)
        {
            return _generator
                   ?? (_generator =
                       _сontainer.Resolve<IAccountNumberGenerator>(
                           _typeAccountNumber == TypeAccountNumber.Long
                               ? "LongAccountNumberGenerator"
                               : "ShortAccountNumberGenerator",
                           new Arguments {  {"useCache", useCache},
                               {
                                   "cache", _сontainer.Resolve<GkhCache>()}
                               }));
        }
    }
}