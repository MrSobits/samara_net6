namespace Bars.Gkh.Regions.Tatarstan.DomainService.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    /// <summary>
    /// Провайдер должников по ЖКУ
    /// </summary>
    public class UtilityDebtorClwStateProvider : IClwStateProvider
    {
        private readonly UtilityDebtorDocumentMeta[] meta;

        private readonly IRepository<DocumentClw> documentRepo;

        /// <inheritdoc />
        public UtilityDebtorClwStateProvider(IRepository<DocumentClw> documentRepo)
        {
            this.documentRepo = documentRepo;
            this.meta = this.CreateMeta();
        }

        /// <inheritdoc />
        public void InitCache(IEnumerable<long> claimWorksIds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public List<ClaimWorkDocumentType> GetAvailableTransitions(BaseClaimWork claimWork, IEnumerable<IClwTransitionRule> rules, bool useCache)
        {
            ArgumentChecker.NotNullAssignableTo(claimWork, typeof(UtilityDebtorClaimWork), "claimWork");
            ArgumentChecker.NotNullOrEmpty(rules, "rules");

            var clw = (UtilityDebtorClaimWork)claimWork;

            return this.FilterByClaimWork(clw).Select(x => x.DocType)
                .Where(x => rules.All(r => r.CanCreateDocumentOfType(x, clw)))
                .ToList();
        }

        /// <inheritdoc />
        public State GetNextState(BaseClaimWork claimWork, IEnumerable<IClwTransitionRule> rules, IClwStateSelector stateSelector, bool useCache)
        {
            throw new NotImplementedException();
        }

        private UtilityDebtorDocumentMeta[] CreateMeta()
        {
            return new[]
            {
                new UtilityDebtorDocumentMeta
                (
                    ClaimWorkDocumentType.ExecutoryProcess, null
                ),
                new UtilityDebtorDocumentMeta
                (
                    ClaimWorkDocumentType.DepartureRestriction,
                    d => this.DocumentExists(d.Id, ClaimWorkDocumentType.ExecutoryProcess)
                ),
                new UtilityDebtorDocumentMeta
                (
                    ClaimWorkDocumentType.SeizureOfProperty, 
                    d => this.DocumentExists(d.Id, ClaimWorkDocumentType.ExecutoryProcess)
                )     
            };
        }

        private bool DocumentExists(long claimWorkId, ClaimWorkDocumentType documentType)
        {
            return this.documentRepo.GetAll()
                .Any(x => x.ClaimWork.Id == claimWorkId && x.DocumentType == documentType);
        }

        private UtilityDebtorDocumentMeta[] FilterByClaimWork(UtilityDebtorClaimWork claimWork)
        {
            var result = this.meta
                .Where(x => x.IsAvalibaleForCreation(claimWork))
                .ToArray();
            return result;
        }

        private class UtilityDebtorDocumentMeta : DocumentMeta
        {
            private readonly Func<UtilityDebtorClaimWork, bool> claimworkHandler;

            public UtilityDebtorDocumentMeta(ClaimWorkDocumentType docType, Func<UtilityDebtorClaimWork, bool> claimworkHandler)
                : base(docType)
            {
                this.claimworkHandler = claimworkHandler;
            }

            public bool IsAvalibaleForCreation(UtilityDebtorClaimWork claimWork)
            {
                return (this.claimworkHandler == null || this.claimworkHandler(claimWork));
            }
        }
    }
}