namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис получения информации по займам
    /// </summary>
    public class RealityObjectLoanRepository : IRealityObjectLoanRepository
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Репозиторий источников займа
        /// </summary>
        public ILoanSourceRepository LoanSourceRepository { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ProgramCr"/>
        /// </summary>
        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Municipality"/>
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public IDataResult ListRealtyObjectNeedLoan(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAsId("programId");
            var municipalityId = baseParams.Params.GetAsId("moId");
            var loadParams = baseParams.GetLoadParam();

            var program = this.ProgramCrDomain.Get(programId);
            var municipality = this.MunicipalityDomain.Get(municipalityId);

            if (program == null)
            {
                return new BaseDataResult(false, "Отсутствует информация о программе");
            }

            if (municipality == null)
            {
                return new BaseDataResult(false, "Отсутствует информация о муниципальном образовании");
            }

           var service = this.GetService();

            using (this.Container.Using(service))
            {
                var data = service.ListRealtyObjectNeedLoan(municipality, program).ToArray();

                var result = data.AsQueryable().Filter(loadParams, this.Container).OrderIf(loadParams.Order.Length == 0, true, x => x.Address);

                var regopInfo = this.LoanSourceRepository.GetRegoperatorSaldo(municipalityId);
                regopInfo.NeedSum = result.SafeSum(x => x.NeedSum);
                regopInfo.BlockedSum += this.GetSaldoNeedLoanRobjects(data, municipalityId);


                var listResult = result.Order(loadParams).Paging(loadParams).ToList();
                
                return new ListRealtyObjectNeedLoanProxyResult
                {
                    Data = listResult,
                    AdditionalData = regopInfo,
                    TotalCount = result.Count()
                };
            }
        }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(RealityObject[] robjects, ProgramCr program)
        {
            var service = this.GetService();

            using (this.Container.Using(service))
            {
                return service.ListRealtyObjectNeedLoan(robjects, program);
            }
        }

        /// <summary>
        /// Метод возвращает сальдо без учета займа на домах, у которых есть ненулевая потребность потребность
        /// </summary>

        private decimal GetSaldoNeedLoanRobjects(RealtyObjectNeedLoan[] realtyObjectNeedLoans, long municipalityId)
        {
            IEnumerable<RealtyObjectNeedLoan> resultFiltered = realtyObjectNeedLoans;

            switch (this.GetLoanLevel())
            {
                case LoanLevel.Municipality:
                    resultFiltered = resultFiltered.Where(x => x.MunicipalityId == municipalityId);
                    break;
                case LoanLevel.Settlement:
                    resultFiltered = resultFiltered.Where(x => x.SettlementId == municipalityId);
                    break;
            }

            var loanDomain = this.Container.ResolveDomain<RealityObjectLoan>();
            decimal roSaldoWithLoans;

            using (this.Container.Using(loanDomain))
            {
                var roIdsToExept = realtyObjectNeedLoans.Select(x => x.Id).ToArray();

                var query = loanDomain.GetAll()
                    .Where(x => x.Operations.Any())
                    .Where(x => x.LoanSum - x.LoanReturnedSum > 0);

                switch (this.GetLoanLevel())
                {
                    case LoanLevel.Municipality:
                        query = query.Where(x => x.LoanTaker.RealityObject.Municipality.Id == municipalityId);
                        break;
                    case LoanLevel.Settlement:
                        query = query.Where(x => x.LoanTaker.RealityObject.MoSettlement.Id == municipalityId);
                        break;
                }

                roSaldoWithLoans = query
                    .Where(x => !roIdsToExept.Contains(x.LoanTaker.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.LoanTaker)
                    .Select(x => x.Key.DebtTotal - x.Key.CreditTotal - x.Sum(y => y.LoanSum - y.LoanReturnedSum))
                    .Where(x => x > 0)
                    .Sum();
            }

            return resultFiltered.Select(x => x.OwnerSum - x.OwnerLoanSum).Where(x => x > 0).SafeSum() + roSaldoWithLoans;
        }

        private IRealtyObjectNeedLoanService GetService()
        {
            var regopLoanConfig = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.LoanConfig;
            var sources = this.Container.ResolveAll<IRealtyObjectNeedLoanService>();

            using (this.Container.Using(sources))
            { 
                return sources.FirstOrDefault(x => x.LoanFormationType == regopLoanConfig.LoanFormationType);
            }
        }

        private LoanLevel GetLoanLevel()
        {
            var regopLoanConfig = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.LoanConfig;
            return regopLoanConfig.Level;
        }
    }
}