namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class BankStatementService : IBankStatementService
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<BankStatement> GetFilteredByOperator()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            return Container.Resolve<IDomainService<BankStatement>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(contragentIds.Count > 0, x => x.ManagingOrganization != null && contragentIds.Contains(x.ManagingOrganization.Contragent.Id));
        }
    }
}