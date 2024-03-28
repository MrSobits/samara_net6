namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel.AppealCits
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealCitsExecutantViewModel : Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits.AppealCitsExecutantViewModel
    {
        public override IDataResult Get(IDomainService<AppealCitsExecutant> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domain.GetAll().FirstOrDefault(x => x.Id == id);

            if (obj != null)
            {

                var executantZjiName = string.Empty;
                var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

                if (obj.Executant != null)
                {
                    using (this.Container.Using(zonalInspectionInspectorDomain))
                    {
                        var zjiNames = zonalInspectionInspectorDomain.GetAll()
                            .Where(x => x.Inspector.Id == obj.Executant.Id)
                            .Select(x => x.ZonalInspection.Name)
                            .ToList();

                        executantZjiName = string.Join(", ", zjiNames);
                    }
                }
                else
                {
                    executantZjiName = obj.ZonalInspection?.Name;
                }

                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.AppealCits,
                        obj.Author,
                        obj.Executant,
                        obj.Controller,
                        obj.IsResponsible,
                        obj.OrderDate,
                        obj.PerformanceDate,
                        obj.Description,
                        obj.State,
                        ExecutantZji = executantZjiName,
                        obj.Resolution
                    });
            }

            return new BaseDataResult();
        }
    }
}