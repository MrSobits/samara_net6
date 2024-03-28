namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Contracts;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Tatarstan.DomainService.States.TransitionRules;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    using NHibernate.Linq;

    public class UtilityDebtorClaimWorkService : BaseClaimWorkService<UtilityDebtorClaimWork>
    {
        private readonly IClwStateProvider clwStateProvider;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="clwStateProvider">Интерфейс провайдера состояний для основания ПИР</param>
        public UtilityDebtorClaimWorkService(IClwStateProvider clwStateProvider)
        {
            this.clwStateProvider = clwStateProvider;
        }

        /// <summary>
        /// Тип претензионной работы
        /// </summary>
        public override ClaimWorkTypeBase ClaimWorkTypeBase
        {
            get { return ClaimWorkTypeBase.UtilityDebtor; }
        }

        /// <summary>
        /// Вернуть список оснований
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="isPaging">Пагинация необходима</param>
        /// <param name="totalCount">Общее количество</param>
        /// <returns>Список оснований</returns>
        public override IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var domainService = this.Container.ResolveDomain<UtilityDebtorClaimWork>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var data = domainService.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        Municipality = x.RealityObject.Municipality.Name,
                        Settlement = x.RealityObject.MoSettlement.Name,
                        x.RealityObject.Address,
                        x.RealityObject,
                        x.AccountOwner,
                        x.OwnerType,
                        x.PersonalAccountState,
                        x.PersonalAccountNum,
                        x.ChargeDebt,
                        x.PenaltyDebt,
                        x.CountDaysDelay,
                        x.IsDebtPaid,
                        x.DebtPaidDate
                    })
                    .Filter(loadParam, this.Container);

                totalCount = data.Count();

                if (isPaging)
                {
                    data = data.Order(loadParam).Paging(loadParam);
                }
                else
                {
                    data = data.Order(loadParam);
                }

                return data.ToList();
            }
            finally
            {
                this.Container.Release(domainService);
            }
        }
        
        /// <summary>
        /// Метод возвращает список необходимых документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Типы документов</returns>
        public override IEnumerable<ClaimWorkDocumentType> GetNeedDocs(BaseParams baseParams)
        {
            var docClwRepo = this.Container.ResolveRepository<DocumentClw>();
            var clwDomain = this.Container.ResolveDomain<UtilityDebtorClaimWork>();

            try
            {
                var claimWorkId = baseParams.Params.GetAsId("claimWorkId");
                var claimWork = clwDomain.Get(claimWorkId);

                return this.clwStateProvider.GetAvailableTransitions(claimWork, this.GetNeedDocRules(docClwRepo));
            }
            finally
            {
                this.Container.Release(clwDomain);
                this.Container.Release(docClwRepo);
            }
        }

        /// <summary>
        /// Вернуть список необходимых документов
        /// </summary>
        /// <param name="docClwRepo">Репозиторий базовых документов ПиР</param>
        /// <returns>Результат запроса</returns>
        public List<IClwTransitionRule> GetNeedDocRules(
            IRepository<DocumentClw> docClwRepo)
        {
            return new List<IClwTransitionRule>
            {
                new UtilityDebtorDocumentExistsRule(this.Container)
            };
        }

        /// <summary>
        /// Массовое создание документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат работы</returns>
        public override IDataResult MassCreateDocs(BaseParams baseParams)
        {
            var typeDoc = baseParams.Params.GetAs<ClaimWorkDocumentType>("typeDocument");
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            var utilityDebtorClaimWorkDomain = this.Container.ResolveDomain<UtilityDebtorClaimWork>();
            var documentClwDomain = this.Container.ResolveDomain<DocumentClw>();
            var docRules = this.Container.ResolveAll<IClaimWorkDocRule>();

            try
            {
                var rule = docRules.FirstOrDefault(x => x.ResultTypeDocument == typeDoc);

                if (rule != null)
                {
                    var states = this.GetStatesByDocType(typeDoc);

                    var claimWorks = utilityDebtorClaimWorkDomain.GetAll()
                        .Fetch(x => x.RealityObject)
                        .Where(x => states.Contains(x.State.Name))
                        .Where(x => ids.Contains(x.Id))
                        .Where(x => !documentClwDomain.GetAll()
                            .Any(y => y.ClaimWork.Id == x.Id && y.DocumentType == typeDoc))
                        .ToList();

                    if (claimWorks.Count != ids.Count())
                    {
                        return new BaseDataResult(false, "Статус записи не соответствует статусу задолженности");
                    }

                    rule.CreateDocument(claimWorks);
                }

                return new BaseDataResult(true, "Документы успешно созданы");
            }
            finally
            {
                this.Container.Release(docRules);
                this.Container.Release(utilityDebtorClaimWorkDomain);
                this.Container.Release(documentClwDomain);
            }
        }

        /// <summary>
        /// Вернуть типы документов для создания
        /// </summary>
        /// <returns></returns>
        public override IDataResult GetDocsTypeToCreate()
        {
            var result = new List<ClaimWorkDocTypeProxy>();
            result.Add(new ClaimWorkDocTypeProxy(ClaimWorkDocumentType.ExecutoryProcess));//пока так
            return new BaseDataResult(result);
        }

        private List<string> GetStatesByDocType(ClaimWorkDocumentType doctype)
        {
            switch (doctype)
            {
                case ClaimWorkDocumentType.ExecutoryProcess:
                case ClaimWorkDocumentType.SeizureOfProperty:
                case ClaimWorkDocumentType.DepartureRestriction:
                    return new List<string>
                        {
                            ClaimWorkStates.UtilityDebtorDraft,
                            ClaimWorkStates.UtilityDebtorFsspInWork,
                            ClaimWorkStates.UtilityDebtorFsspApproved,
                            ClaimWorkStates.UtilityDebtorMsaInWork,
                            ClaimWorkStates.UtilityDebtorMsaApproved
                        };
            }

            return new List<string>();
        }

        private class ClaimWorkDocTypeProxy
        {
            public ClaimWorkDocTypeProxy(ClaimWorkDocumentType type)
            {
                this.Type = type;
                this.Name = type.GetEnumMeta().Display;
            }

            public ClaimWorkDocumentType Type { get; private set; }

            public string Name { get; private set; }
        }
    }
}