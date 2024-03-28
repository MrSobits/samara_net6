namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using NHibernate.Linq;

    /// <summary>
    /// Кредитные договоры/договоры займа (creditcontract.csv)
    /// </summary>
    public class CreditContractSelectorService : BaseProxySelectorService<CreditContractProxy>
    {
        /// <inheritdoc />
        protected override ICollection<CreditContractProxy> GetAdditionalCache()
        {
            var roLoanRepository = this.Container.ResolveRepository<RealityObjectLoan>();

            using (this.Container.Using(roLoanRepository))
            {
                return this.GetProxies(roLoanRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, CreditContractProxy> GetCache()
        {
            var roLoanRepository = this.Container.ResolveRepository<RealityObjectLoan>();

            using (this.Container.Using(roLoanRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var query = roLoanRepository.GetAll()
                    .Where(x => objectCrQuery.Any(y => y.RealityObject.Id == x.LoanTaker.RealityObject.Id));

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<CreditContractProxy> GetProxies(IQueryable<RealityObjectLoan> chargeAccountQuery)
        {
            var roLoanRepository = this.Container.ResolveRepository<RealityObjectLoan>();
            var regOpRepository = this.Container.ResolveRepository<RegOperator>();

            using (this.Container.Using(roLoanRepository, regOpRepository))
            {
                var regopContragentId = regOpRepository.GetAll()
                    .Select(x => (long?) x.Contragent.ExportId)
                    .FirstOrDefault();

                return chargeAccountQuery
                    .Fetch(x => x.Document)
                    .Select(x => new
                    {
                        x.Id,
                        x.LoanDate,
                        x.LoanSum,
                        x.Document,
                        x.DocumentNum
                    })
                    .AsEnumerable()
                    .Select(x => new CreditContractProxy
                    {
                        Id = x.Id,
                        Creditor = regopContragentId,
                        Number = x.LoanDate.ToString("dd.MM.yyyy"),
                        Date = x.LoanDate,
                        Amount = x.LoanSum,

                        File = x.Document,
                        FileType = 1
                    })
                    .ToList();
            }
        }
    }
}