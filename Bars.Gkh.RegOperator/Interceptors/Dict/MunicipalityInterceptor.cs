namespace Bars.Gkh.RegOperator.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;

    /// <summary>
    /// Интерцептор <see cref="Municipality"/> в РегОператоре
    /// </summary>
    public class MunicipalityInterceptor : EmptyDomainInterceptor<Municipality>
    {
        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<Municipality> service, Municipality entity)
        {
            DomainEvents.Raise(new MunicipalityChangeEvent(entity));
            return base.AfterUpdateAction(service, entity);
        }
    }
}