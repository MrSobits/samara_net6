namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Проверка существования документа более старшего по уровню
    /// </summary>
    public class IncludeHiLevelDocRule : IClwTransitionRule
    {
        private readonly IRepository<DocumentClw> documentRepo;
        private readonly IDebtorStateCache debtorStateCache;
        private readonly IDictionary<ClaimWorkDocumentType, int> weightsMatrix;

        public IncludeHiLevelDocRule(IWindsorContainer container)
        {
            this.documentRepo = container.ResolveRepository<DocumentClw>();
            this.debtorStateCache = container.Resolve<IDebtorStateCache>();

            this.weightsMatrix = new Dictionary<ClaimWorkDocumentType, int>
            {
                { ClaimWorkDocumentType.Notification, 1 },
                { ClaimWorkDocumentType.Pretension, 2 },
                { ClaimWorkDocumentType.Lawsuit, 4 },
                { ClaimWorkDocumentType.CourtOrderClaim, 3 }
            };
        }

        /// <summary>
        /// Проверка возможности сформировать документ
        /// </summary>
        /// <param name="docType">Тип документа для формирования</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public bool CanCreateDocumentOfType(ClaimWorkDocumentType docType, BaseClaimWork claimWork, bool useCache = false)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            var debtorClw = claimWork as DebtorClaimWork;
            if (debtorClw != null)
            {
                var maxCreatedDoc = 0;
                if (useCache)
                {
                    maxCreatedDoc = this.debtorStateCache.GetCreatedDocTypes(debtorClw)
                        .SafeMax(x => this.weightsMatrix.Get(x));
                }
                else
                {
                    maxCreatedDoc = this.documentRepo.GetAll()
                        .Where(x => x.ClaimWork.Id == debtorClw.Id)
                        .Select(x => x.DocumentType)
                        .AsEnumerable()
                        .SafeMax(x => this.weightsMatrix.Get(x));
                }
                return true;
                return maxCreatedDoc < this.weightsMatrix.Get<ClaimWorkDocumentType, int>(docType, 100);
            }
            return true;
        }
    }
}