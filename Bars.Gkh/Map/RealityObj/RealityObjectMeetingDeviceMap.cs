namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
       
    /// <summary>Маппинг для "Приборы учета жилого дома"</summary>
    public class RealityObjectMeteringDeviceMap : BaseImportableEntityMap<RealityObjectMeteringDevice>
    {
        /// <summary>Маппинг для "Приборы учета жилого дома"</summary>
        public RealityObjectMeteringDeviceMap() : 
                base("Приборы учета жилого дома", "GKH_OBJ_METERING_DEVICE")
        {
        }

        /// <summary>Маппинг для "Приборы учета жилого дома"</summary>
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            this.Property(x => x.DateRegistration, "Дата постановки на учет").Column("DATE_REGISTRATION");
            this.Property(x => x.DateInstallation, "Дата установки(год)").Column("DATE_INSTALLATION");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.MeteringDevice, "Прибор учета").Column("METERING_DEVICE_ID").NotNull().Fetch();
            this.Property(x => x.SerialNumber, "Заводской номер прибора учёта").Column("Serial_Number");
            this.Property(x => x.AddingReadingsManually, "Внесение показаний в ручном режиме").Column("READINGS_MANUALLY");
            this.Property(x => x.NecessityOfVerificationWhileExpluatation, "Обязательности проверки в рамках эксплуатации прибора учета").Column("NECESSITY_VERIFICATION");
            this.Property(x => x.PersonalAccountNum, "Номер лицевого счёта").Column("ACCOUNT_NUMBER");
            this.Property(x => x.DateFirstVerification, "Дата первичной поверки").Column("DATE_FIRST_VERIFICATION");
        }
    }
}
