namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="KvarProxy"/>
    /// </summary>
    public class KvarSelectorService : BaseProxySelectorService<KvarProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, KvarProxy> GetCache()
        {
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(viewAccOwnershipHistoryRepository))
            {
                var query = viewAccOwnershipHistoryRepository.GetAllDto(this.FilterService.PeriodId)
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.Id, this.FilterService.PersAccIds)
                    .WhereIf(this.UseIncremental, x => this.StartEditDate < x.ObjectEditDate)
                    .WhereIf(this.UseIncremental, x => x.ObjectEditDate < this.EndEditDate)
                    .Filter(this.FilterService.PersAccFilter, this.Container);

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        /// <inheritdoc />
        protected override ICollection<KvarProxy> GetAdditionalCache()
        {
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(viewAccOwnershipHistoryRepository))
            {
                var query = viewAccOwnershipHistoryRepository.GetAllDto(this.FilterService.PeriodId)
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds);

                return this.GetProxies(query);
            }
        }

        protected virtual IList<KvarProxy> GetProxies(IQueryable<ViewAccOwnershipHistoryDto> query)
        {
            var chargePeriodRepository = this.Container.Resolve<IChargePeriodRepository>();

            var personalAccountChangeRepository = this.Container.ResolveRepository<PersonalAccountChange>();
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();
            var roomRepository = this.Container.ResolveRepository<Room>();
            var indAccOwnerRepository = this.Container.ResolveRepository<IndividualAccountOwner>();
            var realityObjectDecisionProtocolProxyService = this.Container.Resolve<IRealityObjectDecisionProtocolProxyService>();

            using (this.Container.Using(
                chargePeriodRepository,
                personalAccountChangeRepository,
                realityObjectRepository,
                roomRepository,
                indAccOwnerRepository,
                realityObjectDecisionProtocolProxyService))
            {
                var contragentDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>().ProxyListCache
                    .ToDictionary(x => x.Key, x => x.Value.Contragent?.ExportId);

                var chargePeriod = chargePeriodRepository.Get(this.FilterService.PeriodId);
                var accountList = query.ToList();

                var persAccByRoomDict = accountList.GroupBy(x => x.RoomId)
                    .ToDictionary(x => x.Key, x => (int?) x.Count());

                var protocolCache = realityObjectDecisionProtocolProxyService.GetAllBothProtocolByFormat(realityObjectRepository.GetAll()
                        .Where(x => query.Any(y => y.RoId == x.Id)))
                    .ToDictionary(x => x.Key,
                        x => x.Value
                            .WhereIf(chargePeriod?.EndDate != null, y => y.ProtocolDate < chargePeriod.EndDate)
                            .OrderByDescending(y => y.ProtocolDate)
                            .Select(y => new
                            {
                                y.ExportId,
                                y.DecisionType
                            })
                            .FirstOrDefault());

                var roomCache = roomRepository.GetAll()
                    .Where(x => query.Any(y => y.RoomId == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.RoomsCount,
                        x.LivingArea,
                    })
                    .ToDictionary(x => x.Id);

                var count = accountList.Count;
                var take = 5000;
                var proxyList = new List<KvarProxy>();

                for (var skip = 0; skip < count; skip += take)
                {
                    var accountPart = accountList
                        .Skip(skip)
                        .Take(take)
                        .ToList();

                    var ownerIds = accountPart.Where(x => x.OwnerType == PersonalAccountOwnerType.Individual)
                        .Select(x => x.OwnerId)
                        .ToList();

                    var accountsCloseIds = accountPart.Where(y => y.CloseDate.HasValue).Select(x => x.Id).ToList();
                    var physOwnerDict = indAccOwnerRepository.GetAll()
                        .WhereContainsBulked(x => x.Id, ownerIds, take)
                        .Select(x => new
                        {
                            x.Id,
                            x.FirstName,
                            x.SecondName,
                            x.Surname,
                            x.BirthDate
                        })
                        .ToDictionary(x => x.Id);

                    //для закрытых лс ищем причину закрытия
                    var closeInfoList = personalAccountChangeRepository.GetAll()
                        .Where(x => x.ChangeType == PersonalAccountChangeType.Close || x.ChangeType == PersonalAccountChangeType.MergeAndClose)
                        .WhereContainsBulked(x => x.PersonalAccount.Id, accountsCloseIds, take)
                        .Select(x => new
                        {
                            x.PersonalAccount.Id,
                            x.ActualFrom,
                            CloseDate = (DateTime?)x.PersonalAccount.CloseDate,
                            ChangeType = (PersonalAccountChangeType?)x.ChangeType,
                            ConditionHouse = (ConditionHouse?)x.PersonalAccount.Room.RealityObject.ConditionHouse,
                            x.Description,
                            Date = (DateTime?)x.Date
                        })
                        .ToList();

                    var closeInfoDict = closeInfoList
                        .Select(x => new
                        {
                            x.Id,
                            CloseDate = x.ActualFrom ?? x.CloseDate,
                            CloseReasonType = x.ChangeType == PersonalAccountChangeType.MergeAndClose
                                ? "8"
                                : x.ConditionHouse == ConditionHouse.Razed
                                    ? "7"
                                    : "6",
                            x.Description,
                            x.Date
                        })
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Date).First());

                    proxyList.AddRange(accountPart.Select(x =>
                    {
                        var closeInfo = closeInfoDict.Get(x.Id);
                        var protocolInfo = protocolCache.Get(x.RoId);
                        var reasonType = protocolInfo?.DecisionType;
                        var protocolId = protocolInfo?.ExportId;
                        var physOwner = physOwnerDict.Get(x.OwnerId);
                        var persAccCount = persAccByRoomDict.Get(x.RoomId);
                        var roomInfo = roomCache.Get(x.RoomId);
                        var roomCount = roomInfo?.RoomsCount;

                        var regopContragentId = ContragentRschetSelectorService.RegopContragentId.Value;

                        return new KvarProxy
                        {
                            Id = x.Id,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RealityObjectId = x.RoId,
                            Surname = physOwner?.Surname,
                            FirstName = physOwner?.FirstName,
                            SecondName = physOwner?.SecondName,
                            BirthDate = physOwner?.BirthDate,
                            OpenDate = x.OpenDate,
                            CloseDate = closeInfo?.CloseDate,
                            CloseReasonType = closeInfo?.CloseReasonType,
                            CloseReason = closeInfo?.Description,
                            ResidentCount = persAccCount,
                            RoomCount = roomCount > 0 ? roomCount : default(int?),
                            Area = x.Area,
                            LivingArea = roomInfo?.LivingArea,
                            PrincipalContragentId = contragentDict.Get(x.RoId),
                            PersonalAccountType = 3,
                            IndividualOwner = x.OwnerType == PersonalAccountOwnerType.Individual ? x.OwnerId : (long?)null,
                            OwnerId = x.OwnerId,
                            ContragentId = x.LegalOwnerExportId,
                            State = x.State.ToUpper() == "ОТКРЫТ" ? 1 : 2,

                            CashPaymentCenterContragentId = regopContragentId,

                            //KVAROPENREASON
                            ReasonType = reasonType == CoreDecisionType.Government ? 6 : reasonType == CoreDecisionType.Owners ? 5 : (int?) null,
                            KapremDecisionId = protocolId,

                            //KVARACCOM
                            PremisesId = x.RoomId,
                            Share = x.AreaShare * 100
                        };
                    }));
                }

                return proxyList.GroupBy(x => x.Id).Select(x => x.OrderByDescending(y => y.OpenDate).FirstOrDefault()).ToList();
            }
        }
    }
}
