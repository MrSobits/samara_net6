namespace Bars.Gkh.Regions.Tatarstan.Map.RealityObject.Intercom
{
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObject;

    public class IntercomMap : B4.Modules.Mapping.Mappers.BaseEntityMap<Intercom>
    {

        public IntercomMap() : base("Сведения о домофонах", "GKH_OBJ_INTERCOM") { }

        protected override void Map()
        {
            this.Property(x => x.IntercomCount, "IntercomCount").Column("INTERCOM_COUNT");
            this.Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID");
            this.Property(x => x.Recording, "Recording").Column("RECORDING");
            this.Property(x => x.ArchiveDepth, "ArchiveDepth").Column("ARCHIVE_DEPTH");
            this.Property(x => x.ArchiveAccess, "ArchiveAccess").Column("ARCHIVE_ACCESS");
            this.Property(x => x.Tariff, "Tariff").Column("TARIFF");
            this.Property(x => x.InstallationDate, "InstallationDate").Column("INSTALLATION_DATE");
            this.Property(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE");
            this.Property(x => x.HasNotTariff, "HasNotTariff").Column("HAS_NOT_TARIFF");
            this.Property(x => x.AnalogIntercomCount, "AnalogIntercomCount").Column("ANALOG_INTERCOM_COUNT");
            this.Property(x => x.IpIntercomCount, "IpIntercomCount").Column("IP_INTERCOM_COUNT");
            this.Property(x => x.EntranceCount, "EntranceCount").Column("ENTRANCE_COUNT");
            this.Property(x => x.IntercomInstallationDate, "IntercomInstallationDate").Column("INTERCOM_INSTALLATION_DATE");
        }
         
    }
}
