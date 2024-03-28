namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников
    /// </summary>
    public class ProtocolossSelectorService : BaseProxySelectorService<ProtocolossProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ProtocolossProxy> GetCache()
        {
            var propertyOwnerProtocolsRepository = this.Container.ResolveRepository<PropertyOwnerProtocols>();
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(propertyOwnerProtocolsRepository,
                viewAccOwnershipHistoryRepository))
            {
                var persAccQuery = viewAccOwnershipHistoryRepository.GetAllDto(this.FilterService.PeriodId)
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.Id, this.FilterService.PersAccIds)
                    .Filter(this.FilterService.PersAccFilter, this.Container);

                var query = propertyOwnerProtocolsRepository.GetAll()
                    .Where(x => persAccQuery.Any(y => y.RoId == x.RealityObject.Id))
                    .Where(x => this.FilterService.ExportDate > x.DocumentDate);

                return this.GetData(query)
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());
            }
        }

        /// <inheritdoc />
        protected override ICollection<ProtocolossProxy> GetAdditionalCache()
        {
            var propertyOwnerProtocolsRepository = this.Container.ResolveRepository<PropertyOwnerProtocols>();

            using (this.Container.Using(propertyOwnerProtocolsRepository))
            {
                var query = propertyOwnerProtocolsRepository.GetAll()
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds);

                return this.GetData(query);
            }
        }

        private ICollection<ProtocolossProxy> GetData(IQueryable<PropertyOwnerProtocols> query)
        {
            var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>().ProxyListCache;

            return query.Where(x => x.TypeProtocol == PropertyOwnerProtocolType.FormationFund
                    || x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard)
                .Fetch(x => x.DocumentFile)
                .Select(x => new
                {
                    x.Id,
                    RealityObjectId = (long?)x.RealityObject.Id,
                    x.DocumentDate,
                    x.DocumentNumber,
                    VotingForm = x.FormVoting,
                    EndDate = x.EndDateDecision,
                    DecisionPlace = x.PlaceDecision,
                    MeetingPlace = x.PlaceMeeting,
                    MeetingDateTime = x.DateMeeting,
                    VoteStartDateTime = x.VotingStartDate,
                    VoteEndDateTime = x.VotingEndDate,
                    ReceptionProcedure = x.OrderTakingDecisionOwners,
                    ReviewProcedure = x.OrderAcquaintanceInfo,
                    x.AnnuaLMeeting,
                    x.LegalityMeeting,
                    Status = x.VotingStatus,
                    x.DocumentFile
                })
                .AsEnumerable()
                .Select(x => new ProtocolossProxy
                {
                    Id = x.Id,
                    RealityObjectId = x.RealityObjectId,
                    ContragentId = manOrgDict.Get(x.RealityObjectId ?? 0)?.Contragent?.ExportId,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    StartDate = x.DocumentDate,
                    VotingForm = x.VotingForm.ToIntNullable(),
                    EndDate = x.VotingForm == FormVoting.Extramural || x.VotingForm == FormVoting.FullTime 
                        ? x.EndDate
                        : default(DateTime?),
                    DecisionPlace = x.VotingForm == FormVoting.Extramural || x.VotingForm == FormVoting.FullTime 
                        ? x.DecisionPlace
                        : default(string),
                    MeetingPlace = x.VotingForm == FormVoting.Intramural || x.VotingForm == FormVoting.FullTime 
                        ? x.MeetingPlace
                        : default(string),
                    MeetingDateTime = x.VotingForm == FormVoting.Intramural || x.VotingForm == FormVoting.FullTime 
                        ? x.MeetingDateTime
                        : default(DateTime?),
                    VoteStartDateTime = x.VotingForm == FormVoting.ExtramuralUsingSystem 
                        ? x.VoteStartDateTime
                        : default(DateTime?),
                    VoteEndDateTime = x.VotingForm == FormVoting.ExtramuralUsingSystem 
                        ? x.VoteEndDateTime
                        : default(DateTime?),
                    ReceptionProcedure = x.VotingForm == FormVoting.ExtramuralUsingSystem 
                        ? x.ReceptionProcedure
                        : default(string),
                    ReviewProcedure = x.VotingForm == FormVoting.ExtramuralUsingSystem 
                        ? x.ReviewProcedure
                        : default(string),
                    IsAnnualMeeting = x.AnnuaLMeeting == YesNo.Yes ? 1 : 2,
                    IsCompetencyMeeting = x.LegalityMeeting == LegalityMeeting.Lawfully ? 1 : 2,
                    Status = this.GetStatus(x.Status),
                    AttachmentFile = x.DocumentFile
                })
                .GroupBy(x => x.RealityObjectId)
                .Select(x => x.OrderByDescending(y => y.DocumentDate).FirstOrDefault())
                .ToList();
        }

        private int? GetStatus(VotingStatus? status)
        {
            switch (status)
            {
                case VotingStatus.Placed:
                    return 1;
                case VotingStatus.CanceledLastChanges:
                    return 2;
                case VotingStatus.Removed:
                    return 3;
                default:
                    return null;
            }
        }
    }
}