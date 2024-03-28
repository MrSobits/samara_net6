using Bars.B4.Modules.NHibernateChangeLog;

namespace Bars.Gkh.Overhaul.LogMap.Provider
{
    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<PaymentSizeCrLogMap>();
            container.Add<RealityObjectStructuralElementLogMap>();
            container.Add<CreditOrgLogMap>();
            container.Add<ContragentBankCreditOrgLogMap>();
            container.Add<RealityObjectStructuralElementAttributeValueLogMap>();
        }
    }
}