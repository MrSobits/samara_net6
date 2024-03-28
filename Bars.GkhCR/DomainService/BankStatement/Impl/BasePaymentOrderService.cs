namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using Castle.Windsor;

    using B4;
    using B4.Utils;

    using Entities;
    using Gkh.Authentification;

    public class BasePaymentOrderService : IBasePaymentOrderService
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<BasePaymentOrder> GetFilteredByOperator()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();

            return Container.Resolve<IDomainService<BasePaymentOrder>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.BankStatement.ObjectCr.RealityObject.Municipality.Id));
        }
    }
}