namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;

    using Castle.Windsor;

    /// <summary>
    /// Поставщик данных для печати договора протокола решений
    /// </summary>
    public class LoanDisposalDataProvider : BaseCollectionDataProvider<LoanDisposalInfo>
    {
        public LoanDisposalDataProvider(IWindsorContainer container, long loanId)
            : base(container)
        {
            LoanId = loanId;
        }

        public override string Name
        {
            get { return "Распоряжение о проведении заимствований"; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public long LoanId { get; set; }

        protected override IQueryable<LoanDisposalInfo> GetDataInternal(BaseParams baseParams)
        {
            var result = new List<LoanDisposalInfo>();

            var loanDoamin = Container.ResolveDomain<RealityObjectLoan>();
            var configProvider = Container.Resolve<IGkhConfigProvider>();
            var walletDomain = Container.ResolveDomain<RealityObjectLoanWallet>();
            var userIdentity = Container.Resolve<IUserIdentity>();
            var userRepository = Container.ResolveRepository<User>();

            try
            {
                var loan = loanDoamin.Get(LoanId);
                var user = userRepository.GetAll().FirstOrDefault(x => x.Id == userIdentity.UserId);

                var loanDisposalInfo = new LoanDisposalInfo();
                var disposalInfo = GetDisposal(loan, user, configProvider);
                loanDisposalInfo.Распоряжение = new List<DisposalInfo> {disposalInfo};
                loanDisposalInfo.Займы = GetLoans(walletDomain, loan);
                
                result.Add(loanDisposalInfo);
            }
            finally
            {
                Container.Release(loanDoamin);
                Container.Release(configProvider);
                Container.Release(walletDomain);
                Container.Release(userIdentity);
                Container.Release(userRepository);
            }

            return result.AsQueryable();
        }

        private List<LoanInfo> GetLoans(IDomainService<RealityObjectLoanWallet> walletDomain, RealityObjectLoan loan)
        {
            var loans = walletDomain.GetAll()
                .Where(x => x.Loan.Id == LoanId).AsEnumerable()
                .Select((x, index) => new LoanInfo
                {
                    ID = x.Loan.Id,
                    НомерПП = (index + 1).ToString(),
                    ДатаЗайма = x.Loan.LoanDate.ToString("dd.MM.yyyy"),
                    ИсточникЗайма = x.TypeSourceLoan.GetEnumMeta().Display,
                    КраткосрочнаяПрограмма = loan.ProgramCr.Name,
                    Сумма = x.Loan.LoanSum.ToString(),
                    Погашено = x.Loan.LoanReturnedSum.ToString(),
                    Задолженность = (x.Loan.LoanSum - x.Loan.LoanReturnedSum).ToString(),
                    ФактДатаВозврата = x.Loan.FactEndDate.HasValue ? x.Loan.FactEndDate.Value.ToString("dd.MM.yyyy") : ""
                });
            return loans.ToList();
        }

        private DisposalInfo GetDisposal(RealityObjectLoan loan, User user, IGkhConfigProvider configProvider)
        {
            DisposalInfo disposalInfo = new DisposalInfo
            {
                LoanId = LoanId,
                Номер = loan.DocumentNum.ToString(),
                Дата = DateTime.Now.ToString("dd.MM.yyyy"),
                Регион = loan.LoanTaker.RealityObject.Municipality.RegionName,
                МР = loan.LoanTaker.RealityObject.Municipality.ParentMo.With(x => x.Name),
                МО = loan.LoanTaker.RealityObject.Municipality.Name,
                АдресЗанимателя = loan.LoanTaker.RealityObject.Address,
                ФИОКонтроль = user != null ? user.Name : string.Empty,
                ФИОДолжЛица = user != null ? user.Name : string.Empty,
                Должность = string.Empty,
                УровеньЗайма = configProvider.Get<RegOperatorConfig>().GeneralConfig.LoanConfig.Level.GetEnumMeta().Display
            };

            return disposalInfo;
        }
    }
}