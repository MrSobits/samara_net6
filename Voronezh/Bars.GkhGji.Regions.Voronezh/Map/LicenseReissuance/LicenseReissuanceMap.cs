namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities;
    
    
    /// <summary>Маппинг для "Определение акта проверки ГЖИ"</summary>
    public class LicenseReissuanceMap : BaseEntityMap<LicenseReissuance>
    {
        
        public LicenseReissuanceMap() : 
                base("Обращение за переоформлением лицензии", "GJI_CH_LICENSE_REISSUANCE")
        {
        }

        protected override void Map()
        {
            Property(x => x.ReissuanceDate, "Дата обращения").Column("REISSUANCE_DATE");
            Property(x => x.RegisterNumber, "как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый нас" +
                    "лучай если заходят изенить номер на маску").Column("REG_NUMBER").Length(100);
            Property(x => x.RegisterNum, "как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый нас" +
                    "лучай если заходят изенить номер на маску").Column("REG_NUM");
            Property(x => x.ConfirmationOfDuty, "подтверждение гос пошлины").Column("CONF_DUTY").Length(1000);
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент - Выбирается из УО но сохраняется Контрагент").Column("CONTRAGENT_ID").NotNull();
            Reference(x => x.ManOrgLicense, "Лицензия").Column("LICENSE_ID");
            Reference(x => x.File, "Лицензия").Column("FILE_ID");
            this.Property(x => x.ReplyTo, "Получатель ответа").Column("REPLY_TO");
            this.Property(x => x.DeclineReason, "Причина отклонения").Column("DECLINE_REASON");
            this.Property(x => x.RPGUNumber, "Тип обращения").Column("RPGU_NUMBER");
            this.Property(x => x.MessageId, "Тип обращения").Column("MESSAGE_ID");
        }
    }
}
