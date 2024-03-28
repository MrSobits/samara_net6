namespace Bars.Gkh.DomainService.Administration.Impl
{
    using System;
    using System.Linq.Expressions;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис получения информации об изменении полей сущности <see cref="ContragentContact"/> зависимой от <see cref="Contragent"/>
    /// </summary>
    public class ContragentContactChangeLog : BaseInheritEntityChangeLog<ContragentContact>
    {
        /// <inheritdoc />
        protected override Expression<Func<ContragentContact, bool>> GetEntityIdSelector(long entityId)
        {
            return x => x.Contragent.Id == entityId;
        }
    }
}