namespace Bars.Gkh.Modules.ClaimWork
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Абстрактное правило создания документа ПИР
    /// </summary>
    public abstract class BaseClaimWorkDocRule : IClaimWorkDocRule
    {
        public virtual string Id { get; }

        public virtual string Description { get; }

        public virtual string ActionUrl { get; }

        public virtual ClaimWorkDocumentType ResultTypeDocument { get; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public virtual void SetParams(BaseParams baseParams)
        {
        }

        /// <inheritdoc />
        public abstract IDataResult CreateDocument(BaseClaimWork claimWork);

        /// <inheritdoc />
        public virtual IDataResult CreateDocument(IEnumerable<BaseClaimWork> claimWorks)
        {
            var result = this.FormDocument(claimWorks);
            TransactionHelper.InsertInManyTransactions(this.Container, result, 1000, true, true);
            return new BaseDataResult();
        }

        /// <inheritdoc />
        public abstract IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true);

        /// <inheritdoc />
        public virtual IDataResult ValidationRule(BaseClaimWork claimWork)
        {
            return new BaseDataResult();
        }
    }
}