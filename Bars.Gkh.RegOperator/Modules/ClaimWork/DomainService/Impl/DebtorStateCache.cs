namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Кэш смены состояний ПИР
    /// </summary>
    public class DebtorStateCache : IDebtorStateCache
    {
        private readonly IWindsorContainer container;
        private bool initialized;
        private readonly object _lock = new object();

        /// <summary>
        /// .ctor
        /// </summary>
        public DebtorStateCache(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public void Init(IEnumerable<long> claimWorkIds)
        {
            lock (this._lock)
            {
                if (this.initialized)
                {
                    return;
                }

                this.InitInternal(claimWorkIds);

                this.initialized = true;
            }
        }


        private void InitInternal(IEnumerable<long> claimWorkIds)
        {
            var documentClwDomain = this.container.ResolveDomain<DocumentClw>();
            var courtOrderClaimDomain = this.container.ResolveDomain<CourtOrderClaim>();
            var petitionDomain = this.container.ResolveDomain<Petition>();

            try
            {
                this.courtOrderClaimCache = courtOrderClaimDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .GroupBy(x => x.ClaimWork.Id)
                    .ToDictionary(x => x.Key, 
                        x => x.GroupBy(y => y.DocumentType)
                            .ToDictionary(y => y.Key, y => y.First()));

                this.petitionClaimCache = petitionDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .GroupBy(x => x.ClaimWork.Id)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.DocumentType)
                            .ToDictionary(y => y.Key, y => y.First()));

                this.documentClwCache = documentClwDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .GroupBy(x => x.ClaimWork.Id)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.DocumentType)
                            .ToDictionary(y => y.Key, y => y.First()));
            }
            finally
            {
                this.container.Release(documentClwDomain);
                this.container.Release(courtOrderClaimDomain);
                this.container.Release(petitionDomain);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (this._lock)
            {
                this.initialized = false;

                this.documentClwCache.Clear();
                this.courtOrderClaimCache.Clear();
                this.petitionClaimCache.Clear();
            }
        }

        /// <inheritdoc />
        public IEnumerable<ClaimWorkDocumentType> GetCreatedDocTypes(BaseClaimWork claimWork)
        {
            return this.documentClwCache.Get(claimWork.Id)?.Keys ?? Enumerable.Empty<ClaimWorkDocumentType>();
        }

        /// <inheritdoc />
        public bool DocumentClwExists(BaseClaimWork claimWork, ClaimWorkDocumentType type)
        {
            return this.documentClwCache.Get(claimWork.Id)?.Get(type) != null;
        }

        /// <inheritdoc />
        public bool CourtOrderClaimExists(BaseClaimWork claimWork, ClaimWorkDocumentType type)
        {
            return this.courtOrderClaimCache.Get(claimWork.Id)?.Get(type) != null;
        }

        /// <inheritdoc />
        public bool LawsuitExists(BaseClaimWork claimWork, ClaimWorkDocumentType type)
        {
            return this.petitionClaimCache.Get(claimWork.Id)?.Get(type) != null;
        }

        /// <inheritdoc />
        public CourtOrderClaim GetCourtClaim(BaseClaimWork claimWork)
        {
            return this.courtOrderClaimCache.Get(claimWork.Id)?.Select(x => x.Value).FirstOrDefault();
        }

        private Dictionary<long, Dictionary<ClaimWorkDocumentType, DocumentClw>> documentClwCache 
            = new Dictionary<long, Dictionary<ClaimWorkDocumentType, DocumentClw>>();

        private Dictionary<long, Dictionary<ClaimWorkDocumentType, CourtOrderClaim>> courtOrderClaimCache 
            = new Dictionary<long, Dictionary<ClaimWorkDocumentType, CourtOrderClaim>>();

        private Dictionary<long, Dictionary<ClaimWorkDocumentType, Petition>> petitionClaimCache 
            = new Dictionary<long, Dictionary<ClaimWorkDocumentType, Petition>>();
    }
}
