namespace Bars.Gkh.Regions.Tatarstan.DomainService.States.TransitionRules
{
    using System.Linq.Dynamic;
    using System.Linq.Dynamic.Core;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using Castle.Windsor;

    /// <summary>
    /// Проверяет существование документа должника ЖКУ для типа
    /// </summary>
    public class UtilityDebtorDocumentExistsRule : IClwTransitionRule
    {
        private readonly IWindsorContainer container;

        public UtilityDebtorDocumentExistsRule(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Проверка возможности сформировать документ
        /// </summary>
        /// <param name="docType">Тип документа для формирования</param>
        /// <param name="claimWork">Основание ПИР</param>
        public bool CanCreateDocumentOfType(ClaimWorkDocumentType docType, BaseClaimWork claimWork)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            if (claimWork is UtilityDebtorClaimWork)
            {
                var repo = this.GetRepo(docType);

                using (this.container.Using(repo))
                {
                    var anyDoc = repo.GetAll()
                        .Where("ClaimWork.Id == @0", claimWork.Id)
                        .Where("DocumentType == @0", docType)
                        .Any();

                    return !anyDoc;
                }
            }
            return true;
        }

        private IRepository GetRepo(ClaimWorkDocumentType docType)
        {
            if (docType == ClaimWorkDocumentType.ExecutoryProcess)
            {
                return this.container.ResolveRepository<ExecutoryProcess>();
            }

            if (docType == ClaimWorkDocumentType.SeizureOfProperty)
            {
                return this.container.ResolveRepository<SeizureOfProperty>();
            }

            if (docType == ClaimWorkDocumentType.DepartureRestriction)
            {
                return this.container.ResolveRepository<DepartureRestriction>();
            }

            return this.container.ResolveRepository<DocumentClw>();
        }

        /// <summary>
        /// Проверка возможности сформировать документ
        /// </summary>
        /// <param name="docType">Тип документа для формирования</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public bool CanCreateDocumentOfType(ClaimWorkDocumentType docType, BaseClaimWork claimWork, bool useCache)
        {
            return this.CanCreateDocumentOfType(docType, claimWork);
        }
    }
}