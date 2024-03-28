namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionInterceptor : EmptyDomainInterceptor<ActCheckAction>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<ActCheckAction> service, ActCheckAction entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.CreationPlace);

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<ActCheckAction> service, ActCheckAction entity)
        {
            Utils.DeleteEntityFiasAddress(entity, this.Container);

            return this.Success();
        }
    }
}