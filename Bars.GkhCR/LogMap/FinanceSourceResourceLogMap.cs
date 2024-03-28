using Bars.B4.Utils;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhCr.Entities;

     public class FinanceSourceResourceLogMap : AuditLogMap<FinanceSourceResource>
    {
         public FinanceSourceResourceLogMap()
        {
            Name("Средства источника финансирования");

            Description(x => x.ReturnSafe(y => y.ObjectCr.RealityObject.Address));

            MapProperty(x => x.FinanceSource, "FinanceSource", "Разрез финансирования",obj => obj.Return(x => x.Name));
            //MapProperty(x => x.BudgetMu, "BudgetMu", "Бюджет МО");
            MapProperty(x => x.BudgetSubject, "BudgetSubject", "Бюджет субъекта");
            //MapProperty(x => x.OwnerResource, "OwnerResource", "Средства собственника");
            MapProperty(x => x.FundResource, "FundResource", "Средства фонда");
        }
    }
}
