namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Views;
    using Bars.Gkh.Repositories.ChargePeriod;

    using NHibernate.Linq;

    /// <summary>
    /// ReadOnly репозиторий истории смены абонента ЛС в разрезе периода
    /// </summary>
    public class ViewAccOwnershipHistoryRepository : IViewAccOwnershipHistoryRepository, IRepository<ViewAccountOwnershipHistory>
    {
        public IRepository<BasePersonalAccount> PersAccRepos { get; set; }
        public IRepository<ViewAccountOwnershipHistory> ViewPersAccRepos { get; set; }
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Метод формирования запроса за текущий период
        /// </summary>
        /// <inheritdoc />
        public IQueryable<ViewAccountOwnershipHistory> GetAll()
        {
            var currPeriod = this.ChargePeriodRepository.GetCurrentPeriod();
            return this.PersAccRepos.GetAll()
                .Fetch(x => x.AccountOwner)
                .Select(x => new ViewAccountOwnershipHistory
                {
                    Id = x.Id,
                    PersonalAccount = x,
                    AccountOwner = x.AccountOwner,
                    Period = currPeriod
                });
        }

        /// <inheritdoc />
        public IQueryable<ViewAccountOwnershipHistory> GetAll(long periodId)
        {
            return this.ViewPersAccRepos.GetAll()
                .Where(x => x.Period.Id == periodId);
        }

        /// <inheritdoc />
        public IQueryable<ViewAccOwnershipHistoryDto> GetAllDto(long periodId = 0)
        {
            return periodId == 0 ? this.FromActualRecords() : this.FromView(periodId);
        }

        private IQueryable<ViewAccOwnershipHistoryDto> FromView(long periodId)
        {
            return this.ViewPersAccRepos.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Select(x => new ViewAccOwnershipHistoryDto
                {
                    Id = x.PersonalAccount.Id,
                    PersonalAccountNum = x.PersonalAccount.PersonalAccountNum,
                    ObjectEditDate = x.PersonalAccount.ObjectEditDate,
                    OwnerId = x.AccountOwner.Id,
                    OwnerType = x.AccountOwner.OwnerType,
                    AccountOwner = x.AccountOwner.Name,
                    LegalOwnerExportId = (x.AccountOwner as LegalAccountOwner).Contragent.ExportId,
                    MunicipalityId = x.PersonalAccount.Room.RealityObject.Municipality.Id,
                    RoId = x.PersonalAccount.Room.RealityObject.Id,
                    RoomId = x.PersonalAccount.Room.Id,
                    Municipality = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                    Address = x.PersonalAccount.Room.RealityObject.Address,
                    RoomAddress =
                                x.PersonalAccount.Room.RealityObject.Address + ", кв. " + x.PersonalAccount.Room.RoomNum
                                + (x.PersonalAccount.Room.ChamberNum != string.Empty && x.PersonalAccount.Room.ChamberNum != null
                                    ? ", ком. " + x.PersonalAccount.Room.ChamberNum
                                    : string.Empty),
                    
                    Area = x.PersonalAccount.Room.Area,
                    AreaShare = x.PersonalAccount.AreaShare,
                    OpenDate = x.PersonalAccount.OpenDate,
                    CloseDate = x.PersonalAccount.CloseDate != DateTime.MinValue ? (DateTime?)x.PersonalAccount.CloseDate : null,
                    State = x.PersonalAccount.State.Name,
                    AccountFormationVariant = x.PersonalAccount.Room.RealityObject.AccountFormationVariant ?? CrFundFormationType.NotSelected
                });
        }
        
        private IQueryable<ViewAccOwnershipHistoryDto> FromActualRecords()
        {
            return this.PersAccRepos.GetAll()
                .Select(x => new ViewAccOwnershipHistoryDto
                {
                    Id = x.Id,
                    PersonalAccountNum = x.PersonalAccountNum,
                    ObjectEditDate = x.ObjectEditDate,
                    OwnerId = x.AccountOwner.Id,
                    OwnerType = x.AccountOwner.OwnerType,
                    AccountOwner = x.AccountOwner.Name,
                    LegalOwnerExportId = (x.AccountOwner as LegalAccountOwner).Contragent.ExportId,
                    MunicipalityId = x.Room.RealityObject.Municipality.Id,
                    RoId = x.Room.RealityObject.Id,
                    RoomId = x.Room.Id,
                    Municipality = x.Room.RealityObject.Municipality.Name,
                    Address = x.Room.RealityObject.Address,
                    RoomAddress =
                        x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum
                        + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null
                            ? ", ком. " + x.Room.ChamberNum
                            : string.Empty),

                    Area = x.Room.Area,
                    AreaShare = x.AreaShare,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate != DateTime.MinValue ? (DateTime?)x.CloseDate : null,
                    State = x.State.Name,
                    AccountFormationVariant = x.Room.RealityObject.AccountFormationVariant ?? CrFundFormationType.NotSelected
                });
        }

        /// <inheritdoc />
        public void Evict(object entity)
        {
            this.ViewPersAccRepos.Evict(entity);
        }

        /// <inheritdoc />
        public void Evict(ViewAccountOwnershipHistory entity)
        {
            this.ViewPersAccRepos.Evict(entity);
        }

        /// <inheritdoc />
        object IRepository.Get(object id)
        {
            return this.Get(id);
        }

        /// <inheritdoc />
        IQueryable IRepository.GetAll()
        {
            return this.GetAll();
        }

        /// <inheritdoc />
        object IRepository.Load(object id)
        {
            return this.Load(id);
        }

        /// <inheritdoc />
        public ViewAccountOwnershipHistory Get(object id)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        public ViewAccountOwnershipHistory Load(object id)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        public void Save(ViewAccountOwnershipHistory value)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        public void Update(ViewAccountOwnershipHistory value)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        void IRepository<ViewAccountOwnershipHistory>.Delete(object id)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        void IRepository.Delete(object id)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        public void Save(object value)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }

        /// <inheritdoc />
        public void Update(object value)
        {
            throw new NotSupportedException("Use IRepository<AccountOwnershipHistory>");
        }
    }
}