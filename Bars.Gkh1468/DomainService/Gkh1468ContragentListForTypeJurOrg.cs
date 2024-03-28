namespace Bars.Gkh1468.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class Gkh1468ContragentListForTypeJurOrg : IContragentListForTypeJurOrg
    {
        public IDomainService<PublicServiceOrg> ManOrgDomain { get; set; }

        public void GetQueryableForTypeJurOrg(ref IQueryable<Contragent> query, TypeJurPerson type)
        {
            switch (type)
            {
                // Поставщик ресурсов
                case TypeJurPerson.ResourceCompany: query = query.Where(x => ManOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    break;
            }
        }
    }
}
