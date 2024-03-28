namespace Bars.GkhCr.Regions.Tatarstan.Map.ObjectOutdoorCr
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TypeWorkRealityObjectOutdoorMap : BaseEntityMap<TypeWorkRealityObjectOutdoor>
    {
        /// <inheritdoc />
        public TypeWorkRealityObjectOutdoorMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.TypeWorkRealityObjectOutdoor", "CR_OBJ_OUTDOOR_TYPE_WORK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Volume, "Объем (плановый)").Column("VOLUME");
            this.Property(x => x.Sum, "Сумма (плановая)").Column("SUM");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(255);
            this.Property(x => x.IsActive, "Признак является ли запись Активной").Column("IS_ACTIVE");
            this.Reference(x => x.ObjectOutdoorCr, "Объект").Column("OBJECT_OUTDOOR_ID").NotNull().Fetch();
            this.Reference(x => x.WorkRealityObjectOutdoor, "Вид работы").Column("WORK_ID").NotNull().Fetch();
        }
    }
}
