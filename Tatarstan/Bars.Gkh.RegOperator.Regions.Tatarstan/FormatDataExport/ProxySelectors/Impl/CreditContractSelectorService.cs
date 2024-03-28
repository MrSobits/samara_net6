namespace Bars.Gkh.RegOperator.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
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
            var roLoanRepository = this.Container.ResolveRepository<PropertyOwnerProtocols>();

            using (this.Container.Using(roLoanRepository))
            {
                return this.GetProxies(roLoanRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, CreditContractProxy> GetCache()
        {
            var roLoanRepository = this.Container.ResolveRepository<PropertyOwnerProtocols>();

            using (this.Container.Using(roLoanRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var query = roLoanRepository.GetAll()
                    .Where(x => objectCrQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id));

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<CreditContractProxy> GetProxies(IQueryable<PropertyOwnerProtocols> chargeAccountQuery)
        {
            return chargeAccountQuery.Where(x => x.TypeProtocol == PropertyOwnerProtocolType.Loan)
                .Fetch(x => x.DocumentFile)
                .Select(x => new CreditContractProxy
                {
                    Id = x.Id,
                    Creditor = x.Lender.ExportId,
                    Number = x.DocumentNumber,
                    Date = x.DocumentDate,
                    Amount = x.LoanAmount,

                    File = x.DocumentFile,
                    FileType = 1
                })
                .ToList();
        }
    }
}