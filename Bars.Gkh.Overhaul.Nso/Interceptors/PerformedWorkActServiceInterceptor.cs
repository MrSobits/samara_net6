namespace Bars.GkhCr.Regions.Nso.Interceptors
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Overhaul.Nso.Entities;

    public class PerformedWorkActServiceInterceptor : EmptyDomainInterceptor<PerformedWorkAct>
    {
        public  IDataResult CheckSumAndVolume(PerformedWorkAct act, IDomainService<PerformedWorkAct> service)
        {
            var typeWorkCrVersionStage1Service = Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();

            try
            {
                if (act.TypeWorkCr != null)
                {
                    var sumVolume =
                        service.GetAll()
                               .Where(x => x.TypeWorkCr.Id == act.TypeWorkCr.Id && (act.Id == 0 || x.Id != act.Id))
                               .Select(x => new { x.Sum, x.Volume })
                               .ToArray();

                    var vol = typeWorkCrVersionStage1Service
                                    .GetAll()
                                    .Where(x => x.TypeWorkCr.Id == act.TypeWorkCr.Id)
                                    .Select(x => x.Volume)
                                    .ToList()
                                    .SafeSum(x => x);

                    var actsTotalSum = sumVolume.Select(x => x.Sum).Sum() + act.Sum;
                    var actsTotalVolume = sumVolume.Select(x => x.Volume).Sum() + act.Volume;

                    if (act.TypeWorkCr.Sum < actsTotalSum || vol < actsTotalVolume)
                    {
                        return Failure("Сумма и/или объем по актам выполненных работ по данному виду работы превышают значения, указанные в паспорте объекта КР. Сохранение отменено.");
                    }
                }
            }
            finally
            {
                Container.Release(typeWorkCrVersionStage1Service); 
            }
            
            return Success();
        }
    }
}