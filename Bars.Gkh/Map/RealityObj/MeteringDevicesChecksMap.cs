namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    using Entities.RealityObj;

    public class MeteringDevicesChecksMap : BaseImportableEntityMap<MeteringDevicesChecks>
    {
        public MeteringDevicesChecksMap()
            : base("Проверки приборов учёта", "GKH_METERING_DEVICES_CHECKS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ControlReading, "Контрольное показание").Column("CONTROL_READING");
            this.Property(x => x.RemovalControlReadingDate, "Дата снятия контрольного показания").Column("REMOVAL_CONTROL_READING_DATE");
            this.Property(x => x.StartDateCheck, "Дата начала проверки").Column("START_DATE_CHECK");
            this.Property(x => x.StartValue, "Значение показаний прибора учета на момент начала проверки").Column("START_VALUE");
            this.Property(x => x.EndDateCheck, "Дата окончания проверки").Column("END_DATE_CHECK");
            this.Property(x => x.EndValue, "Значение показаний на момент окончания проверки").Column("END_VALUE");
            this.Property(x => x.MarkMeteringDevice, "Марка прибора учёта").Column("MARK_METERING_DEVICE");
            this.Property(x => x.IntervalVerification, "Межпроверочный интервал (лет)").Column("INTERVAL_VERIFICATION");
            this.Property(x => x.NextDateCheck, "Плановая дата следующей проверки").Column("NEXT_DATE_CHECK");
            this.Reference(x => x.MeteringDevice, "Приборы учета жилого дома").Column("METERING_DEVICE_ID");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID");

        }
    }
}