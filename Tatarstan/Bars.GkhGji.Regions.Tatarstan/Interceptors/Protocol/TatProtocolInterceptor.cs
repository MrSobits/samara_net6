namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Protocol
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;

    /// <summary>
    /// Interceptor для <see cref="TatProtocol"/>
    /// </summary>
    public class TatProtocolInterceptor : ProtocolServiceInterceptor<TatProtocol>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<TatProtocol> service, TatProtocol entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlace);

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<TatProtocol> service, TatProtocol entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlace);

            return base.BeforeUpdateAction(service, entity);
        }
    }
}