namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules
{
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using System.Linq;
    using System.Linq.Dynamic.Core;

    /// <summary>
    /// Проверяет существование документа для типа
    /// </summary>
    public class DebtorDocumentExistsRule : IClwTransitionRule
    {
        private readonly IWindsorContainer container;
        private readonly IDebtorStateCache debtorStateCache;

        public DebtorDocumentExistsRule(IWindsorContainer container)
        {
            this.container = container;
            this.debtorStateCache = container.Resolve<IDebtorStateCache>();
        }

        #region Implementation of IClwDocumentCreationRule

        /// <summary>
        /// Проверка возможности сформировать документ
        /// </summary>
        /// <param name="docType">Тип документа для формирования</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public bool CanCreateDocumentOfType(ClaimWorkDocumentType docType, BaseClaimWork claimWork, bool useCache = false)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            if (claimWork is DebtorClaimWork)
            {
                if (useCache)
                {
                    return this.CanCreateUsingCache(claimWork, docType);
                }
                
                //using (this.container.Using(repo))
                //{
                //    bool anyDoc = repo.GetAll()
                //        .Where("ClaimWork.Id == @0", claimWork.Id)
                //        .Where("DocumentType == @0", docType)
                //        .Any();

                //    return !anyDoc;
                //}
                var repo = this.GetRepo(docType); 
                
                using (this.container.Using(repo))
                {
                    bool anyDoc = repo.GetAll()
                        .Where("ClaimWork.Id == @0", claimWork.Id)
                        .Where("DocumentType == @0", docType)
                        .Any();

                    return !anyDoc;
                }
            }

            return true;
        }

        #endregion

        private IRepository GetRepo(ClaimWorkDocumentType docType)
        {
            if (docType == ClaimWorkDocumentType.CourtOrderClaim)
            {
                return this.container.ResolveRepository<CourtOrderClaim>();
            }

            if (docType == ClaimWorkDocumentType.Lawsuit)
            {
                return this.container.ResolveRepository<Petition>();
            }

            return this.container.ResolveRepository<DocumentClw>();
        }

        private bool CanCreateUsingCache(BaseClaimWork claimwork, ClaimWorkDocumentType docType)
        {
            if (docType == ClaimWorkDocumentType.CourtOrderClaim)
            {
                return !this.debtorStateCache.CourtOrderClaimExists(claimwork, docType);
            }

            if (docType == ClaimWorkDocumentType.Lawsuit)
            {
                return !this.debtorStateCache.LawsuitExists(claimwork, docType);
            }

            return !this.debtorStateCache.DocumentClwExists(claimwork, docType);
        }
    }
}