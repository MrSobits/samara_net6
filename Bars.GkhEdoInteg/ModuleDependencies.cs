namespace Bars.GkhEdoInteg
{
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Bars.B4.Utils;

    using Castle.Core.Internal;

    using Entities;
    using Gkh.Entities;
    using GkhGji.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container) : base(container)
        {

        }

        public override IModuleDependencies Init()
        {
            #region AppealCits

            References.Add(new EntityReference
            {
                ReferenceName = "Сопоставление обращения граждан с кодом Эдо",
                BaseEntity = typeof(AppealCits),
                DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<AppealCitsCompareEdo>>();
                        var list = service.GetAll().Where(x => x.AppealCits.Id == id).Select(x => x.Id).ToList();

                        foreach (var value in list)
                        {
                            var logAppId = value;
                            var serviceLogAppCits = Container.Resolve<IDomainService<LogRequestsAppCitsEdo>>();
                            serviceLogAppCits.GetAll()
                                .Where(x => x.AppealCitsCompareEdo.Id == logAppId)
                                .ForEach(x => serviceLogAppCits.Delete(x.Id));

                            service.Delete(value);
                        }
                    }
            });

            #endregion AppealCits

            #region Inspector

            References.Add(new EntityReference
            {
                ReferenceName = "Сопоставление инспекторов с кодом Эдо",
                BaseEntity = typeof(Inspector),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<InspectorCompareEdo>>();
                    var list = service.GetAll().Where(x => x.Inspector.Id == id).Select(x => x.Id).ToList();

                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            #endregion

            #region KindStatementGji

            References.Add(new EntityReference
            {
                ReferenceName = "Сопоставление вида обращения с кодом Эдо",
                BaseEntity = typeof(KindStatementGji),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<KindStatementCompareEdo>>();
                    var list = service.GetAll().Where(x => x.KindStatement.Id == id).Select(x => x.Id).ToList();

                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            #endregion

            #region RevenueFormCompareEdo

            References.Add(new EntityReference
            {
                ReferenceName = "Сопоставление вида обращения с кодом Эдо",
                BaseEntity = typeof(RevenueFormGji),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RevenueFormCompareEdo>>();
                    var list = service.GetAll().Where(x => x.RevenueForm.Id == id).Select(x => x.Id).ToList();

                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            #endregion

            #region RevenueSourceCompareEdo

            References.Add(new EntityReference
            {
                ReferenceName = "Сопоставление вида обращения с кодом Эдо",
                BaseEntity = typeof(RevenueSourceGji),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RevenueSourceCompareEdo>>();
                    var list = service.GetAll().Where(x => x.RevenueSource.Id == id).Select(x => x.Id).ToList();

                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            #endregion

            return this;
        }
    }
}