namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class RealityObjectVidecamMap : BaseEntityMap<RealityObjectVidecam>
    {
        public RealityObjectVidecamMap() : 
                base("Камера видеонаблюдения", "GKH_REALITY_VIDECAM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.InstallPlace, "InstallPlace").Column("INSTALLPLACE");
            this.Property(x => x.Workability, "Работоспособность").Column("WORKABILITY");
            this.Property(x => x.TypeVidecam, "TypeVidecam").Column("DESCRIPTION");
            this.Property(x => x.UnicalNumber, "UnicalNumber").Column("UNIQ_NUMBER");
            this.Property(x => x.VidecamAddress, "VidecamAddress").Column("VIDECAM_URL");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
        }
    }
}
