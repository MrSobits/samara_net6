namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActionIsolated
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class ActActionIsolatedInterceptor: ActCheckServiceInterceptor<ActActionIsolated>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActActionIsolated> service, ActActionIsolated entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActActionIsolated> service, ActActionIsolated entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeUpdateAction(service, entity);
        }
    }
}