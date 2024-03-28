namespace Bars.Gkh.RegOperator.DataProviders
{
    using B4.Utils;

    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Proxy лс для документа на оплату
    /// </summary>
    public class PersonalAccountPaymentDocProxy
    {
        public PersonalAccountPaymentDocProxy()
        {
        }

        public PersonalAccountPaymentDocProxy(BasePersonalAccount account)
        {
            this.Account = account;
            this.Id = account.Id;
            this.OwnerId = account.AccountOwner.Id;
            this.ExternalId = account.PersAccNumExternalSystems;

            this.IsRoomHasNoNumber = account.Room.IsRoomHasNoNumber;
            this.Notation = account.Room.Notation;
            this.RoomNum = account.Room.RoomNum;
            this.RoomType = account.Room.Type;
            this.Area = account.Room.Area;

            this.AreaShare = account.AreaShare;
            this.Tariff = account.Tariff;
            this.ChamberNum = account.Room.ChamberNum;
            this.PersonalAccountNum = account.PersonalAccountNum;
            this.SsWalletGuid = account.SocialSupportWallet.WalletGuid;
            this.RoId = account.Room.RealityObject.Id;
            this.AddressName = account.Room.RealityObject.FiasAddress.AddressName;
            this.PostCode = account.Room.RealityObject.FiasAddress.PostCode;
            this.AreaLivingNotLivingMkd = account.Room.RealityObject.AreaLivingNotLivingMkd;
            this.MunicipalityId = account.Room.RealityObject.Municipality.Id;
            this.Municipality = account.Room.RealityObject.Municipality.Name;
            this.SettlementId = account.Room.RealityObject.MoSettlement.ReturnSafe(x => x.Id);
            this.Settlement = account.Room.RealityObject.MoSettlement.ReturnSafe(x => x.Name);
            this.PlaceName = account.Room.RealityObject.FiasAddress.PlaceName;
            this.StreetName = account.Room.RealityObject.FiasAddress.StreetName;
            this.House = account.Room.RealityObject.FiasAddress.House;
            this.Housing = account.Room.RealityObject.FiasAddress.Housing;
            this.Building = account.Room.RealityObject.FiasAddress.Building;
            this.Letter = account.Room.RealityObject.FiasAddress.Letter;
            this.State = account.State.Name;
            this.DecisionChargeBalance = account.DecisionChargeBalance;
        }

        public string ChamberNum { get; set; }
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public string ExternalId { get; set; }
        public string RoomNum { get; set; }
        public bool IsRoomHasNoNumber { get; set; }
        public string AddressName { get; set; }
        public string PostCode { get; set; }
        public decimal Area { get; set; }
        public decimal AreaShare { get; set; }
        public decimal Tariff { get; set; }
        public decimal BaseTariff { get; set; }

        public long MunicipalityId { get; set; }
        public string Municipality { get; set; }
        public string Settlement { get; set; }
        public string PersonalAccountNum { get; set; }
        public BasePersonalAccount Account { get; set; }
        public string PlaceGuidId { get; set; }
        public long? SettlementId { get; set; }
        public long RoId { get; set; }
        public string Notation { get; set; }
        public decimal? AreaLivingNotLivingMkd { get; set; }
        public string PlaceName { get; set; }
        public string StreetName { get; set; }
        public string House { get; set; }
        public string Housing { get; set; }
        public string Building { get; set; }
        public string Letter { get; set; }
        public string State { get; set; }

        public string FactPlaceName { get; set; }
        public string FactStreetName { get; set; }
        public string FactHouse { get; set; }
        public string FactHousing { get; set; }
        public string FactBuilding { get; set; }
        public string FactLetter { get; set; }
        public string FactFlat { get; set; }

        /// <summary>
        /// гуид кошелька соцподдержки
        /// </summary>
        public string SsWalletGuid { get; set; }
        public RoomType RoomType { get; set; }
        public string PenaltyWalletGuid { get; set; }
        public decimal? DecisionChargeBalance { get; set; }
    }
}