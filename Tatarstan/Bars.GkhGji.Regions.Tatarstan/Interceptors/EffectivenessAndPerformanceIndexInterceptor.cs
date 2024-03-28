namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class EffectivenessAndPerformanceIndexInterceptor : EmptyDomainInterceptor<EffectivenessAndPerformanceIndex>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<EffectivenessAndPerformanceIndex> service, EffectivenessAndPerformanceIndex entity)
        {
            var effectivenessAndPerformanceIndexValueService = this.Container.Resolve<IDomainService<EffectivenessAndPerformanceIndexValue>>();

            using (this.Container.Using(effectivenessAndPerformanceIndexValueService))
            {
                return effectivenessAndPerformanceIndexValueService.GetAll()
                    .Any(w => w.EffectivenessAndPerformanceIndex.Id == entity.Id)
                        ? this.Failure($"Показатель <b>{entity.Name}</b> используется в значении показателя эффективности и результативности")
                        : this.Success();
            }
        }
    }
}
