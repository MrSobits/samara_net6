namespace Bars.GkhCr.Repositories.PerformedWorkActPayments
{
    using System.Collections.Generic;
    using Bars.Gkh.Repositories;
    using Bars.GkhCr.Entities;

    public interface IPerformedWorkActPaymentRepository: IDomainRepository<PerformedWorkActPayment>
    {
        IEnumerable<PerformedWorkActPayment> GetOpenedPaymentsByTransferGuids(IEnumerable<string> transferGuids);

    }
}
