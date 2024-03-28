namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class RegOpContragentListForTypeJurOrg : IContragentListForTypeJurOrg
    {
        public IDomainService<Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator> RegOpDomain { get; set; }

        public void GetQueryableForTypeJurOrg(ref IQueryable<Contragent> query, TypeJurPerson type)
        {

            if (type == TypeJurPerson.RegOp)
            {
                query = query.Where(x => RegOpDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
            }
        }
    }
}
