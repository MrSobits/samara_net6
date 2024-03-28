namespace Bars.Gkh.RegOperator.Regions.Tatarstan
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
            References.Add(new EntityReference
            {
                ReferenceName = "Учет платежных документов по начислениям и оплатам на КР",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<ConfirmContribution>>();
                        service.GetAll().Where(x => x.ManagingOrganization.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
            });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "Платежный документ по начислениям и оплатам на КР",
                    BaseEntity = typeof(RealityObject),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<ConfirmContributionDoc>>();
                        service.GetAll().Where(x => x.RealityObject.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "Перечисления по найму",
                    BaseEntity = typeof(BasePersonalAccount),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<TransferHire>>();
                        service.GetAll().Where(x => x.Account.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "Перечисления по найму",
                    BaseEntity = typeof(TransferRfRecord),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<TransferHire>>();
                        service.GetAll().Where(x => x.Account.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });
        }
    }
}