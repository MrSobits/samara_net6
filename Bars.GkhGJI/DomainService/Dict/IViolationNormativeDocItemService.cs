using System.Linq;
using Bars.Gkh.Entities.Dicts;
using NHibernate.Persister.Entity;

namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IViolationNormativeDocItemService
    {
        IDataResult ListTree(BaseParams baseParams);

        IDataResult SaveNormativeDocItems(BaseParams baseParams);

        IDataResult UpdaeteViolationsByNpd(IQueryable<NormativeDocItem> npdQuery = null);
    }
}