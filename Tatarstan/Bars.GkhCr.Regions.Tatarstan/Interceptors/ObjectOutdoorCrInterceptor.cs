namespace Bars.GkhCr.Regions.Tatarstan.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrInterceptor : EmptyDomainInterceptor<ObjectOutdoorCr>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<ObjectOutdoorCr> service, ObjectOutdoorCr entity)
        {
            var stateDomain = this.Container.ResolveDomain<State>();
            using (this.Container.Using(stateDomain))
            {
                entity.State = stateDomain.FirstOrDefault(x => x.TypeId == "object_outdoor_cr" && x.StartState);
                return base.BeforeCreateAction(service, entity);
            }
        }
    }
}
