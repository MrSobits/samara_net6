namespace Bars.Gkh.Overhaul.DomainService
{
    using System;
    using System.Linq.Expressions;

    using Bars.Gkh.DomainService.Administration.Impl;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Сервис получения информации об изменении полей сущности <see cref="ContragentBankCreditOrg"/> зависимой от <see cref="Contragent"/>
    /// </summary>
    public class ContragentBankCreditOrgChangeLog : BaseInheritEntityChangeLog<ContragentBankCreditOrg>
    {
        /// <inheritdoc />
        protected override Expression<Func<ContragentBankCreditOrg, bool>> GetEntityIdSelector(long entityId)
        {
            return x => x.Contragent.Id == entityId;
        }
    }
}