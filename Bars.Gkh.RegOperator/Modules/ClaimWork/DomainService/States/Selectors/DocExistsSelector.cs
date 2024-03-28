namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.Selectors
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Селектор, который оставляет только статусы, говорящие о том, что документ создан
    /// </summary>
    public class DocExistsSelector : IClwStateSelector
    {
        private IClwStateSelector next;
        private readonly IRepository<DocumentClw> documentRepo;
        private readonly IDebtorStateCache debtorStateCache;

        public DocExistsSelector(IWindsorContainer container)
        {
            this.documentRepo = container.ResolveRepository<DocumentClw>();
            this.debtorStateCache = container.Resolve<IDebtorStateCache>(); 
        }

        #region Implementation of IClwStateSelector

        /// <summary>
        /// Установить следующий обработчик в цепочке вызовов
        /// </summary>
        /// <param name="nextSelector">Следующий фильтр статусов</param>
        public void SetSuccessor(IClwStateSelector nextSelector)
        {
            this.next = nextSelector;
        }

        /// <summary>
        /// Отфильтровать статусы, доступные для основания ПИР
        /// </summary>
        /// <param name="statesToFilter">Начальный список статусов, который должен быть отфильтрован</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="documentMeta">Содержит сопоставление между типом документа и статусами</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public void Filter(List<State> statesToFilter, BaseClaimWork claimWork, IEnumerable<DocumentMeta> documentMeta, bool useCache = false)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            if (claimWork is DebtorClaimWork)
            {
                IList<ClaimWorkDocumentType> createdDocType = null;

                if (useCache)
                {
                    createdDocType = documentMeta.Where(x => this.debtorStateCache.DocumentClwExists(claimWork, x.DocType))
                        .Select(x => x.DocType)
                        .ToList();
                }
                else
                {
                    var docTypes = documentMeta.Select(x => x.DocType).ToArray();
                    createdDocType = this.documentRepo.GetAll()
                        .Where(x => x.ClaimWork.Id == claimWork.Id)
                        .WhereContainsBulked(x => x.DocumentType, docTypes)
                        .Select(x => x.DocumentType)
                        .ToList();
                }
            
                if (createdDocType.IsNotEmpty())
                {
                    var metaList = documentMeta.Where(x => x.StateConfig != null)
                        .Where(x => createdDocType.Contains(x.DocType))
                        .Select(x => x.StateConfig.NeededState.Name)
                        .Union(
                            documentMeta.Where(x => createdDocType.Contains(x.DocType))
                                .Where(x => x.StateConfig != null)
                                .Select(x => x.StateConfig.FormedState.Name))
                        .ToList();
                    statesToFilter
                        .RemoveAll(x => metaList.Contains(x.Name) && !x.FinalState);
                }
            }

            if (this.next != null)
            {
                this.next.Filter(statesToFilter, claimWork, documentMeta, useCache);
            }
        }

        #endregion
    }
}