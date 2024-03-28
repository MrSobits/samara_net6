using System.Linq;

namespace Bars.Gkh.DomainService
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public interface IContragentListForTypeJurOrg
    {
        void GetQueryableForTypeJurOrg(ref IQueryable<Contragent> query, TypeJurPerson type);
    }
}