namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;

    public class WarningInspectionInterceptor : InspectionGjiInterceptor<WarningInspection>
    {
        public override IDataResult BeforeCreateAction(IDomainService<WarningInspection> service, WarningInspection entity)
        {
            entity.TypeBase = TypeBase.GjiWarning;

            var maxNum = service.GetAll()
                .Where(x => x.TypeBase == entity.TypeBase)
                .Select(x => x.InspectionNum).Max();

            entity.InspectionNum = maxNum.ToInt() + 1;
            entity.InspectionNumber = entity.InspectionNum.ToStr();

            return base.BeforeCreateAction(service, entity);
        }
    }
}