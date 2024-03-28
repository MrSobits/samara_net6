namespace Bars.Gkh.RegOperator.Map.Dto
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Dto;

    /// <summary>
    /// Маппинг для <see cref="BasePersonalAccountDto"/>
    /// </summary>
    public class BasePersonalAccountDtoMap : PersistentObjectMap<BasePersonalAccountDto>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BasePersonalAccountDtoMap() : base("Bars.Gkh.RegOperator.Entities.Dto.BasePersonalAccountDto", "REGOP_PERS_ACC_DTO")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.PersonalAccountNum, "Номер лицевого счета").Column("ACC_NUM").NotNull();
            this.Property(x => x.UnifiedAccountNumber, "Номер лицевого счета").Column("GIS_ACC_NUM");
            this.Property(x => x.PersAccNumExternalSystems, "Номер ЛС во внешней системе").Column("REGOP_PERS_ACC_EXTSYST");
            this.Property(x => x.OpenDate, "Дата открытия").Column("OPEN_DATE").NotNull();
            this.Property(x => x.CloseDate, "Дата закрытия").Column("CLOSE_DATE");
            this.Property(x => x.AreaShare, "Доля собственности").Column("AREA_SHARE");
            this.Property(x => x.OwnerId, "Идентификатор абонента").Column("ACC_OWNER_ID").NotNull();
            this.Property(x => x.RoomId, "Идентификатор помещения").Column("ROOM_ID").NotNull();

            this.Property(x => x.AccountOwner, "Наименование абонента").Column("NAME").NotNull();
            this.Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE").NotNull();
            this.Property(x => x.PrivilegedCategoryId, "Категория льгот").Column("PRIVILEGED_CATEGORY_ID");
            this.Property(x => x.HasOnlyOneRoomWithOpenState, "Имеет только одно помещение со статусом Открыто").Column("HAS_ONLY_ROOM_WITH_OPEN_STATE").NotNull();

            this.Property(x => x.RoId, "Идентификатор дома").Column("RO_ID").NotNull();
            this.Property(x => x.Area, "Площадь помещения").Column("CAREA").NotNull();
            this.Property(x => x.RoomNum, "Номер помещения").Column("CROOM_NUM").NotNull();
            this.Property(x => x.ChamberNum, "Номер комнаты").Column("CHAMBER_NUM");
            this.Property(x => x.RoomAddress, "Адрес помещения").Column("ROOM_ADRESS").NotNull();

            this.Property(x => x.Address, "Адрес МКД").Column("ADDRESS").NotNull();
            this.Property(x => x.AreaMkd, "Площадь МКД").Column("AREA_MKD");
            this.Property(x => x.AccountFormationVariant, "Способ формирования фонда").Column("ACC_FORM_VARIANT");

            this.Property(x => x.Municipality, "Муниципальный район (наименование)").Column("MO_NAME").NotNull();
            this.Property(x => x.MuId, "Муниципальный район (идентификатор)").Column("MO_ID").NotNull();
            this.Property(x => x.Settlement, "Муниципальное образование (идентификатор)").Column("STL_NAME");
            this.Property(x => x.SettleId, "Муниципальное образование (идентификатор)").Column("STL_ID");
            this.Property(x => x.DigitalReceipt, "Признак электронная квитанция").Column("DIGITAL_RECEIPT").NotNull();
            this.Property(x => x.HaveEmail, "Наличие эл.почты").Column("HAVE_EMAIL").NotNull();
            this.Property(x => x.IsNotDebtor, "Не считать должником").Column("IS_NOT_DEBTOR").NotNull();

            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull();
        }
    }
}