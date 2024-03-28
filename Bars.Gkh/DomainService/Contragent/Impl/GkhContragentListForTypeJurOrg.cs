namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class GkhContragentListForTypeJurOrg : IContragentListForTypeJurOrg
    {
        public IDomainService<ManagingOrganization> ManOrgDomain { get; set; }

        public IDomainService<Builder> BuilderDomain { get; set; }

        public IDomainService<LocalGovernment> LocalGovDomain { get; set; }

        public IDomainService<SupplyResourceOrg> SupplyResOrgDomain { get; set; }

        public IDomainService<ServiceOrganization> ServiceOrgDomain { get; set; }

        public IDomainService<PoliticAuthority> PoliticAuthorityDomain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public void GetQueryableForTypeJurOrg(ref IQueryable<Contragent> query, TypeJurPerson type)
        {
            switch (type)
            {
                // Управляющие организации
                case TypeJurPerson.Tsj:
                case TypeJurPerson.ManagingOrganization: query = query.Where(x => ManOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    break;

                // Поставщик коммунальных услуг
                case TypeJurPerson.SupplyResourceOrg: query = query.Where(x => SupplyResOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    break;

                // Органы местного самоуправления
                case TypeJurPerson.LocalGovernment: query = query.Where(x => LocalGovDomain.GetAll().Any(y => y.Contragent.Id == x.Id)); break;

                // Органы государственной власти
                case TypeJurPerson.PoliticAuthority: query = query.Where(x => PoliticAuthorityDomain.GetAll().Any(y => y.Contragent.Id == x.Id)); break;

                // Подрядчики
                case TypeJurPerson.Builder: query = query.Where(x => BuilderDomain.GetAll().Any(y => y.Contragent.Id == x.Id)); break;

                // Поставщик жилищных услуг (ServiceOrganization)
                case TypeJurPerson.ServOrg: query = query.Where(x => ServiceOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id)); break;

                // Организация-арендатор
                case TypeJurPerson.RenterOrg:
                    {
                        // ниче неделаем
                    };
                    break;
            }
        }
    }
}
