namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников
    /// </summary>
    public class ProtocolossSelectorService : BaseProxySelectorService<ProtocolossProxy>
    {
        /// <inheritdoc />
        protected override ICollection<ProtocolossProxy> GetAdditionalCache()
        {
            var propertyOwnerProtocolsRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();

            using (this.Container.Using(propertyOwnerProtocolsRepository))
            {
                var query = propertyOwnerProtocolsRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);

                return this.GetData(query);
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, ProtocolossProxy> GetCache()
        {
            var propertyOwnerProtocolsRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(propertyOwnerProtocolsRepository,
                viewAccOwnershipHistoryRepository))
            {
                var persAccQuery = viewAccOwnershipHistoryRepository.GetAllDto(this.FilterService.PeriodId)
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.Id, this.FilterService.PersAccIds)
                    .Filter(this.FilterService.PersAccFilter, this.Container);

                var query = propertyOwnerProtocolsRepository.GetAll()
                    .Where(x => persAccQuery.Any(y => y.RoId == x.RealityObject.Id))
                    .Where(x => this.FilterService.ExportDate > x.ProtocolDate);

                return this.GetData(query)
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());
            }
        }

        private ICollection<ProtocolossProxy> GetData(IQueryable<RealityObjectDecisionProtocol> query)
        {
            return query
                .Fetch(x => x.File)
                .Select(x => new
                {
                    x.ExportId,
                    RealityObjectId = (long?) x.RealityObject.Id,
                    DocumentDate = x.ProtocolDate,
                    DocumentNumber = x.DocumentNum,
                    StartDate = x.DateStart,
                    VotingForm = x.FormVoting,
                    Status = x.VotingStatus,
                    EndDate = x.EndDateDecision,
                    DecisionPlace = x.PlaceDecision,
                    MeetingPlace = x.PlaceMeeting,
                    MeetingDateTime = x.DateMeeting,
                    VoteStartDateTime = x.VotingStartDate,
                    VoteEndDateTime = x.VotingEndDate,
                    ReceptionProcedure = x.OrderTakingDecisionOwners,
                    ReviewProcedure = x.OrderAcquaintanceInfo,
                    x.AnnuaLMeeting,
                    x.HasQuorum,
                    x.File
                })
                .AsEnumerable()
                .Select(x => new ProtocolossProxy
                {
                    Id = x.ExportId,
                    RealityObjectId = x.RealityObjectId,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    StartDate = x.StartDate,
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
                    IsCompetencyMeeting = x.HasQuorum == YesNo.Yes ? 1 : 2,
                    Status = this.GetStatus(x.Status),
                    AttachmentFile = x.File
                })
                .GroupBy(x => x.RealityObjectId)
                .Select(x => x.OrderByDescending(y => y.DocumentDate).FirstOrDefault())
                .ToList();
        }

        protected int? GetStatus(VotingStatus? status)
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