namespace Bars.GkhCr.Regions.Tatarstan.Map.ObjectOutdoorCr
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TypeWorkRealityObjectOutdoorHistoryMap : BaseEntityMap<TypeWorkRealityObjectOutdoorHistory>
    {
        /// <inheritdoc />
        public TypeWorkRealityObjectOutdoorHistoryMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr.TypeWorkRealityObjectOutdoorHistory", "CR_OBJ_OUTDOOR_TYPE_WORK_HIST")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Volume, "Объем выполнения").Column("VOLUME");
            this.Property(x => x.Sum, "Сумма расходов").Column("SUM");
            this.Property(x => x.UserName, "Имя пользователя").Column("USER_NAME").Length(255);
            this.Property(x => x.TypeAction, "Тип действия для истории вида работ объекта").Column("TYPE_ACTION").NotNull();
            this.Reference(x => x.TypeWorkRealityObjectOutdoor, "Вид работы объекта").Column("TYPE_WORK_ID").NotNull().Fetch();
        }
    }
}
