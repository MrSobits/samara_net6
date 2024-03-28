namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Linq;

    public partial class ThirdPartyPersonalAccountImport
    {
        //string.Format("{0}#{1}#{2}#{3}#{4}", x.Place, x.Street, x.House, x.Housing, x.Letter)
        protected Dictionary<string, RealityObject> RobjectCache;

        //кэш лицевых счетов, по номеру счета
        protected Dictionary<string, BasePersonalAccount> AccountCache;

        //кэш помещений, id жилого дома - номер помещения - помещение
        protected Dictionary<long, Dictionary<string, RoomInfo>> RoomCache;

        protected Dictionary<long, decimal> RoomAreaShareCache;

        protected Dictionary<DateTime, ChargePeriod> PeriodCache;

        protected Dictionary<string, Dictionary<long, PersonalAccountPeriodSummary>> AccountSummaryCache;

        protected Dictionary<long, Dictionary<long, RealityObjectChargeAccountOperation>> RoChargeAccountSummaryCache;

        protected Dictionary<long, RealityObjectChargeAccount> RoChargeAccountCache;

        protected State PersonalAccountStartState;

        private readonly Dictionary<Room, DateTime> roomLogToCreateDict = new Dictionary<Room, DateTime>();
        private readonly Dictionary<BasePersonalAccount, DateTime> accountAreaShareLogToCreateDict = new Dictionary<BasePersonalAccount, DateTime>();
        private readonly Dictionary<BasePersonalAccount, DateTime> accountOpenDateLogToCreateDict = new Dictionary<BasePersonalAccount, DateTime>();

        private List<long> roIds = new List<long>();

        public IRepository<RealityObject> RobjectRepo { get; set; }

        public IRepository<Room> RoomRepo { get; set; }

        public IRepository<BasePersonalAccount> AccountRepo { get; set; }

        public IRepository<ChargePeriod> ChargePeriod { get; set; }

        public IRepository<PersonalAccountPeriodSummary> AccountSummaryRepo { get; set; }

        public IRepository<RealityObjectChargeAccountOperation> RoChargeAccountSummaryRepo { get; set; }

        public IRepository<RealityObjectChargeAccount> RoChargeAccountRepo { get; set; }

        public IRepository<EntityLogLight> EntityLogLightRepo { get; set; }

        public IRepository<User> UserRepo { get; set; }

        public IUserIdentity Identity { get; set; }

        public IMassPersonalAccountDtoService AccountDtoService { get; set; }

        protected void InitCache(List<DataRecord> records)
        {
            this.AccountDtoService.InitCache();
            var typeId = this.Container.Resolve<IStateProvider>().GetStatefulEntityInfo(typeof(BasePersonalAccount)).TypeId;

            var stateDomain = this.Container.ResolveDomain<State>();
            using (this.Container.Using(stateDomain))
            {
                this.accountStatuses = stateDomain.GetAll()
                    .Where(x => x.TypeId == typeId)
                    .ToDictionary(x=>x.Name.ToLower());
            }

            this.PersonalAccountStartState = this.accountStatuses.Values
                .FirstOrDefault(x => x.StartState);

            if (this.PersonalAccountStartState == null)
            {
                throw new ValidationException("Для лицевых счетов не задан начальный статус");
            }

            this.roIds = records.Where(x => x.Robject != null).Select(x => x.Robject.Id).Distinct().ToList();

            this.RoomCache = this.RoomRepo.GetAll()
                .Where(x => this.roIds.Contains(x.RealityObject.Id))
                .Where(x => x.RoomNum != null)
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.RoomNum,
                        Room = x,
                        AreaShare = (decimal?) this.AccountRepo.GetAll()
                            .Where(y => y.Room == x && !y.State.FinalState)
                            .Sum(y => y.AreaShare)
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    y => y
                        .GroupBy(x => x.RoomNum)
                        .ToDictionary(
                            x => x.Key,
                            z => z.Select(x => new RoomInfo {Room = x.Room, AreaShare = x.AreaShare ?? 0}).First()));

            this.PeriodCache = this.ChargePeriod.GetAll()
                .Select(
                    x => new
                    {
                        x.StartDate,
                        Period = x
                    })
                .AsEnumerable()
                .ToDictionary(x => x.StartDate.Date, y => y.Period);

            this.AccountCache = this.AccountRepo.GetAll()
                .Where(x => this.roIds.Contains(x.Room.RealityObject.Id))
                .Where(x => x.PersAccNumExternalSystems != null)
                .Fetch(x => x.AccountOwner)
                .Fetch(x => x.State)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .ThenFetch(x => x.Municipality)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .ThenFetch(x => x.MoSettlement)
                .ToDictionary(x => x.PersAccNumExternalSystems);

            this.AccountSummaryCache = this.AccountSummaryRepo.GetAll()
                .Where(x => this.roIds.Contains(x.PersonalAccount.Room.RealityObject.Id))
                .Where(x => x.PersonalAccount.PersAccNumExternalSystems != null)
                .Select(
                    x => new
                    {
                        x.PersonalAccount.PersAccNumExternalSystems,
                        PeriodId = x.Period.Id,
                        Summary = x
                    })
                .AsEnumerable()
                .Where(x => !x.PersAccNumExternalSystems.IsEmpty())
                .GroupBy(x => x.PersAccNumExternalSystems)
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.PeriodId, z => z.Summary));

            this.RoChargeAccountSummaryCache = this.RoChargeAccountSummaryRepo.GetAll()
                .Where(x => this.roIds.Contains(x.Account.RealityObject.Id))
                .Select(
                    x => new
                    {
                        RoId = x.Account.RealityObject.Id,
                        PeriodId = x.Period.Id,
                        Summary = x
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    y => y
                        .GroupBy(x => x.PeriodId)
                        .ToDictionary(x => x.Key, z => z.Select(x => x.Summary).First()));

            this.RoChargeAccountCache = this.RoChargeAccountRepo.GetAll()
                .Where(x => this.roIds.Contains(x.RealityObject.Id))
                .ToDictionary(x => x.RealityObject.Id);
        }

        protected class RoomInfo
        {
            public Room Room { get; set; }

            public decimal AreaShare { get; set; }
        }
    }
}