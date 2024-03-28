namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Contracts.Params;

    using Entities.ValueObjects;
    using Entities;
    using Gkh.Entities;

    public interface IRealityObjectPaymentAccountRepository
    {
        RealityObjectPaymentAccount GetByRealtyObject(RealityObject realityObject);

        IQueryable<RealityObjectPaymentAccount> GetByRealtyObjects(IEnumerable<RealityObject> realityObjects);

        IQueryable<RealityObjectPaymentAccount> GetSourceAccountsForTransfers(IEnumerable<Transfer> transfers);

        IQueryable<RealityObjectPaymentAccount> GetTargetAccountsForTransfers(IEnumerable<Transfer> transfers);

        GenericListResult<object> ListByPersonalAccountOwner(BaseParams @params);

        void SaveOrUpdate(RealityObjectPaymentAccount account);
    }
}