namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVMVDMap : BaseEntityMap<SMEVMVD>
    {
        
        public SMEVMVDMap() : 
                base("Запрос к ВС МВД", "GJI_CH_SMEV_MVD")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Reference(x => x.RegionCodePrimary, "Код региона основной").Column("REG_CODE_PRIMARY_ID").Fetch();
            Reference(x => x.RegionCodeAdditional, "Код региона дополнительный").Column("REG_CODE_ADDITIONAL_ID").Fetch();
            Property(x => x.MVDTypeAddressPrimary, "Тип адреса основной").Column("TYPE_ADDRESS_PRIMARY").NotNull();
            Property(x => x.MVDTypeAddressAdditional, "Тип адреса дополнительный").Column("TYPE_ADDRESS_ADDITIONAL");
            Property(x => x.AddressPrimary, "Адрес основной").Column("ADDRESS_PRIMARY").NotNull();
            Property(x => x.AddressAdditional, "Адрес дополнительный").Column("ADDRESS_ADDITIONAL");
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE").NotNull();
            Property(x => x.SNILS, "СНИЛС").Column("SNILS");
            Property(x => x.Surname, "Фамилия").Column("SURNAME");
            Property(x => x.Name, "Имя").Column("NAME");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.PatronymicName, "Отчество").Column("PATRONYMIC");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.AnswerInfo, "Результат").Column("ANSWER_INFO");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
