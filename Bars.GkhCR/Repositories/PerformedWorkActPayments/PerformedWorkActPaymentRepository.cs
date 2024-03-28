namespace Bars.GkhCr.Repositories.PerformedWorkActPayments
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Repositories;
    using Bars.GkhCr.Entities;

    public class PerformedWorkActPaymentRepository : BaseDomainRepository<PerformedWorkActPayment>, IPerformedWorkActPaymentRepository
    {
        private readonly IDomainService<PerformedWorkActPayment> _domain;

        public PerformedWorkActPaymentRepository(IDomainService<PerformedWorkActPayment> domain)
        {
            _domain = domain;
        }

        public IEnumerable<PerformedWorkActPayment> GetOpenedPaymentsByTransferGuids(IEnumerable<string> transferGuids)
        {
            return _domain.GetAll().Where(x => transferGuids.Contains(x.TransferGuid));
        }

    }
}
