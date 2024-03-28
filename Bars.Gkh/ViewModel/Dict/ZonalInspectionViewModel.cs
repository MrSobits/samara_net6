namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    // Заглушка для того чтобы в существующих регионах там где от него наследовались не полетело 
    public class ZonalInspectionViewModel : ZonalInspectionViewModel<ZonalInspection>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в ZonalInspectionViewModel<T>
    }

    // Класс переделан для того, чтобы в регионах можно было расширять сущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class ZonalInspectionViewModel<T> : BaseViewModel<T>
        where T: ZonalInspection
    {
        public override IDataResult List(IDomainService<T> domain, BaseParams baseParams)
        {
            var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            try
            {
                var ids = baseParams.Params.GetAs("Id", string.Empty);
                var listIds = !string.IsNullOrWhiteSpace(ids)
                    ? ids.Split(',').Select(id => id.ToLong()).ToArray()
                    : new long[0];

                var inspectorIds = baseParams.Params.GetAs("inspectorIds", string.Empty);
                var inspectorIdsList = !string.IsNullOrWhiteSpace(inspectorIds)
                    ? inspectorIds.Split(',').Select(id => id.ToLong()).ToArray()
                    : new long[0];

                var useAuthFilter = baseParams.Params.Get("useAuthFilter", false);
                if (useAuthFilter)
                {
                    var user = userManager.GetActiveOperator();
                    var inspector = user?.Inspector;
                    if (inspector.IsNotNull())
                    {
                        inspectorIdsList = new[] { inspector.Id };
                    }
                }

                var zonalInspectionInspectorsList = zonalInspectionInspectorDomain.GetAll()
                    .Where(x => inspectorIdsList.Contains(x.Inspector.Id))
                    .Select(x => x.ZonalInspection.Id).ToList();

                return domain.GetAll()
                    .WhereIf(listIds.IsNotEmpty(), x => listIds.Contains(x.Id))
                    .WhereIf(zonalInspectionInspectorsList.IsNotEmpty(), x => zonalInspectionInspectorsList.Contains(x.Id))
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
            finally
            {
                this.Container.Release(zonalInspectionInspectorDomain);
                this.Container.Release(userManager);
            }
        }
    }
}