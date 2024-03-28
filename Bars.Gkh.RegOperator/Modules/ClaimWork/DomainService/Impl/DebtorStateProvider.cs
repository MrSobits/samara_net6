namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class DebtorStateProvider : IDebtorStateProvider
    {
        public IGkhConfigProvider ConfigProvider { get; set; }

        public IRepository<DocumentClw> DocumentClwRepository { get; set; }

        public IRepository<DebtorClaimWork> DebtorClaimWorkRepository { get; set; }

        public IEnumerable<IClwTransitionRule> Rules { get; set; }

        public ReadOnlyDictionary<ClaimWorkDocumentType, DebtorState> FormedStates { get; }
            = new ReadOnlyDictionary<ClaimWorkDocumentType, DebtorState>(
                new Dictionary<ClaimWorkDocumentType, DebtorState>
                {
                    { ClaimWorkDocumentType.Notification, DebtorState.NotificationFormed },
                    { ClaimWorkDocumentType.Pretension, DebtorState.PretensionFormed },
                    { ClaimWorkDocumentType.Lawsuit, DebtorState.PetitionFormed },
                    { ClaimWorkDocumentType.CourtOrderClaim, DebtorState.CourtOrderClaimFormed }
                });
        public ReadOnlyDictionary<ClaimWorkDocumentType, DebtorState> AvaiableStates { get; }
            = new ReadOnlyDictionary<ClaimWorkDocumentType, DebtorState>(
                new Dictionary<ClaimWorkDocumentType, DebtorState>
                {
                    { ClaimWorkDocumentType.Notification, DebtorState.NotificationNeeded },
                    { ClaimWorkDocumentType.Pretension, DebtorState.PretensionNeeded },
                    { ClaimWorkDocumentType.Lawsuit, DebtorState.LawsuitNeeded },
                    { ClaimWorkDocumentType.CourtOrderClaim, DebtorState.LawsuitNeeded }
                });
        private readonly object locker = new object();

        private IDictionary<long, DebtorState> cache = null;

        private IDictionary<long, DebtorState> Cache {
            get
            {
                if (this.cache == null)
                {
                    lock (this.locker)
                    {
                        if (this.cache == null)
                        {
                            this.InitCache(this.DebtorClaimWorkRepository.GetAll());
                        }
                    }
                }

                return this.cache;
            }
        }

        private void InitCache(IQueryable<DocumentClw> documentClwQuery)
        {
            var newCache = documentClwQuery
                .Select(x => new
                {
                    x.ClaimWork.Id,
                    x.DocumentType
                })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.DocumentType)
                .ToDictionary(x => x.Key, y => y.SafeMax(x => this.FormedStates.Get(x)));

            Interlocked.Exchange(ref this.cache, newCache);
        }

        /// <inheritdoc />
        public void InitCache(IQueryable<DebtorClaimWork> debtorQuery)
        {
            this.InitCache(this.DocumentClwRepository.GetAll()
                .Where(x => debtorQuery.Any(y => y.Id == x.ClaimWork.Id)));
        }

        /// <inheritdoc />
        public void InitCache(IEnumerable<long> debtorIds)
        {
            this.InitCache(this.DocumentClwRepository.GetAll()
                .WhereContainsBulked(x => x.ClaimWork.Id, debtorIds, 10000));
        }

        public DebtorState GetState(DebtorClaimWork debtor, IEnumerable<ClaimWorkDocumentType> avaiableDocs)
        {
            if (debtor.IsDebtPaid)
            {
                return DebtorState.PaidDebt;
            }

            var maxLevelDoc = this.Cache.Get(debtor.Id);

            var nextAvaiable = avaiableDocs.Where(x => this.AvaiableStates.ContainsKey(x))
                .SafeMin(x => this.AvaiableStates.Get(x), maxLevelDoc);

            return nextAvaiable;
        }
    }
}