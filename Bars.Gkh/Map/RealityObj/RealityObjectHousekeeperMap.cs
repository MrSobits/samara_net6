namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class RealityObjectHousekeeperMap : BaseEntityMap<RealityObjectHousekeeper>
    {
        public RealityObjectHousekeeperMap() : 
                base("Cистема коллективного приема телевидения", "GKH_REALITY_HOUSEKEEPER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FIO, "ФИО").Column("FIO").NotNull();
            this.Property(x => x.IsActive, "Активен").Column("IS_ACTIVE");
            this.Property(x => x.Login, "Логин").Column("LOGIN").NotNull();
            this.Property(x => x.Password, "Password").Column("PASSWORD");
            this.Property(x => x.PhoneNumber, "Номер телефона").Column("PHONE_NUMBER");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
        }
    }
}
