using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.UserActionRetention.LogMap;

namespace Bars.GkhRF.LogMap.Provider
{
    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<ContractRfLogMap>();
            container.Add<LimitCheckLogMap>();
            container.Add<PaymentLogMap>();
            container.Add<RequestTransferRfLogMap>();
            container.Add<TransferRfLogMap>();
        }
    }
}