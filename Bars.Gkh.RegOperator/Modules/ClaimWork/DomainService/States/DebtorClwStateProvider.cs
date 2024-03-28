namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Modules.ClaimWork.Contracts;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Провайдер состояний для основания ПИР
    /// </summary>
    public class DebtorClwStateProvider : IClwStateProvider
    {
        private readonly DebtorDocumentMeta[] meta;
        private readonly DebtorClaimWorkConfig configSection;
        private readonly State defaultState;
        private readonly State finalState;
        private readonly IRepository<DocumentClw> documentRepo;
        private readonly IDebtorStateCache stateCache;


        public DebtorClwStateProvider(
            IGkhConfigProvider configProv,
            IStateRepository stateRepo,
            IRepository<DocumentClw> documentRepo,
            IDebtorStateCache cache)
        {
            this.configSection = configProv.Get<DebtorClaimWorkConfig>();
            this.documentRepo = documentRepo;
            this.stateCache = cache;

            this.defaultState = stateRepo
                .GetAllStates<DebtorClaimWork>(x => x.StartState)
                .FirstOrDefault();

            this.finalState = stateRepo
                .GetAllStates<DebtorClaimWork>(x => x.FinalState)
                .FirstOrDefault();

            this.meta = this.CreateMeta();
        }

        private Dictionary<long, List<DocumentClw>> documentDict; 

        #region Implementation of IClwStateProvider

        public virtual void InitCache(IEnumerable<long> claimWorksIds)
        {
            this.stateCache.Init(claimWorksIds);
            this.documentDict = this.documentRepo.GetAll()
                .WhereContainsBulked(x => x.ClaimWork.Id, claimWorksIds)
                .GroupBy(x => x.ClaimWork.Id)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        public virtual void Clear()
        {
            this.stateCache.Clear();
            this.documentDict?.Clear();
        }

        /// <summary>
        /// Получить возможные создания документы
        /// </summary>
        /// <param name="claimWork"></param>
        /// <param name="rules"></param>
        public List<ClaimWorkDocumentType> GetAvailableTransitions(BaseClaimWork claimWork, IEnumerable<IClwTransitionRule> rules, bool useCache)
        {
            ArgumentChecker.NotNullAssignableTo(claimWork, typeof(DebtorClaimWork), "claimWork");
            ArgumentChecker.NotNullOrEmpty(rules, "rules");

            var clw = (DebtorClaimWork)claimWork;

            return this.FilterByClaimWork(clw)
                .Select(x => x.DocType)
                .Where(x => rules.All(r => r.CanCreateDocumentOfType(x, clw, useCache)))
                .ToList();
        }

        /// <summary>
        /// Получить следующее возможное состояние для основания ПИР
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="rules">Валидаторы перехода</param>
        /// <param name="stateSelector">Фильтр статусов</param>
        /// <param name="useCache">Использовать кэш</param>
        public State GetNextState(BaseClaimWork claimWork, IEnumerable<IClwTransitionRule> rules,
            IClwStateSelector stateSelector, bool useCache)
        {
            ArgumentChecker.NotNullAssignableTo(claimWork, typeof(DebtorClaimWork), "claimWork");

            if (claimWork.IsDebtPaid && claimWork.DebtPaidDate.HasValue)
            {
                return this.finalState;
            }

            return this.defaultState;
        }
        #endregion

        #region Private methods

        private DebtorDocumentMeta[] CreateMeta()
        {
            return new[]
            {
                new DebtorDocumentMeta(
                    ClaimWorkDocumentType.Notification,
                    cfg => cfg.DebtNotification.NotifFormationKind != DocumentFormationType.NoForm,
                    null)
                {
                    StateConfig = new StateConfig
                    {
                        NeededState = new StateEntry(ClaimWorkStates.NotificationNeeded, 1),
                        FormedState = new StateEntry(ClaimWorkStates.NotificationFormed, 2)
                    }
                },

                new DebtorDocumentMeta(
                    ClaimWorkDocumentType.Pretension,
                    cfg => cfg.Pretension.PretensionFormationKind != DocumentFormationType.NoForm,
                    null)
                {
                    StateConfig = new StateConfig
                    {
                        NeededState = new StateEntry(ClaimWorkStates.PretensionNeeded, 1),
                        FormedState = new StateEntry(ClaimWorkStates.PretensionFormed, 2)
                    }
                },

                new DebtorDocumentMeta(ClaimWorkDocumentType.CourtOrderClaim, null, c => this.PetitionIsAbsent(c.Id))
                {
                    StateConfig = new StateConfig
                    {
                        NeededState = new StateEntry(ClaimWorkStates.LawsuitNeeded, 1),
                        FormedState = new StateEntry(ClaimWorkStates.CourtOrderClaimFormed, 2)
                    }
                },

                new DebtorDocumentMeta(ClaimWorkDocumentType.Lawsuit, null, c => this.CourtOrderClaimIsAbsentOrDeclined(c.Id))
                {
                    StateConfig = new StateConfig
                    {
                        NeededState = new StateEntry(ClaimWorkStates.PetitionNeeded, 1),
                        FormedState = new StateEntry(ClaimWorkStates.PetitionFormed, 2)
                    }
                },

                new DebtorDocumentMeta(ClaimWorkDocumentType.RestructDebt,
                    cfg => cfg.RestructDebt.RestructDebtFormKind != DocumentFormationKind.NoForm,
                    null),

                new DebtorDocumentMeta(ClaimWorkDocumentType.RestructDebtAmicAgr,
                    cfg => cfg.RestructDebtAmicAgr.RestructDebtFormKind != DocumentFormationKind.NoForm,
                    x => this.IsAllowRestructDebtAmicableAgreement(x.Id))
            };
        }

        private bool CourtOrderClaimIsAbsentOrDeclined(long claimWorkId)
        {
            var courtClaim = this.documentDict?.Get(claimWorkId)?
                .FirstOrDefault(x => x.DocumentType == ClaimWorkDocumentType.CourtOrderClaim) as CourtOrderClaim;

            return courtClaim == null
                || courtClaim.ResultConsideration == LawsuitResultConsideration.Denied
                || courtClaim.ObjectionArrived == YesNo.Yes;
        }

        private bool PetitionIsAbsent(long claimWorkId)
        {
            return !this.documentDict?.Get(claimWorkId)?
                .Any(x => x.DocumentType == ClaimWorkDocumentType.Lawsuit)
                ?? true;
        }

        private bool IsAllowRestructDebtAmicableAgreement(long claimWorkId)
        {
            return this.documentDict?.Get(claimWorkId)?.Any(x =>
            {
                var doc = (x as Petition);

                if (doc?.LawsuitDocType == LawsuitDocumentType.AmicableAgreement
                    && doc.DebtSumApproved.HasValue
                    && doc.PenaltyDebtApproved.HasValue)
                {
                    return true;
                }
                return false;
            }) ?? false;
        }


        private DebtorTypeConfig GetConfig(DebtorClaimWork clw)
        {
            var config = clw.DebtorType == DebtorType.Legal
                ? this.configSection.Legal
                : this.configSection.Individual;

            return config;
        }

        private DebtorDocumentMeta[] FilterByClaimWork(DebtorClaimWork claimWork)
        {
            var config = this.GetConfig(claimWork);

            var result = this.meta
                //.Where(x => x.IsAvaliableForCreation(config, claimWork))
                .ToArray();

            return result;
        }

        #endregion

        private class DebtorDocumentMeta : DocumentMeta
        {
            private readonly Func<DebtorTypeConfig, bool> configHandler;
            private readonly Func<DebtorClaimWork, bool> claimworkHandler;

            public DebtorDocumentMeta(ClaimWorkDocumentType docType,
                Func<DebtorTypeConfig, bool> configHandler, Func<DebtorClaimWork, bool> claimworkHandler)
                : base(docType)
            {
                this.configHandler = configHandler;
                this.claimworkHandler = claimworkHandler;
            }

            public bool IsAvaliableForCreation(DebtorTypeConfig config, DebtorClaimWork claimWork)
            {
                bool result = (this.configHandler == null || this.configHandler(config))
                       && (this.claimworkHandler == null || this.claimworkHandler(claimWork));
                return result;
            }
        }
    }
}