namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class RealityObjectAntennaMap : BaseEntityMap<RealityObjectAntenna>
    {
        public RealityObjectAntennaMap() : 
                base("Cистема коллективного приема телевидения", "GKH_REALITY_ANTENNA")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Availability, "Наличие").Column("AVAILABILITY").NotNull();
            this.Property(x => x.Workability, "Работоспособность").Column("WORKABILITY");
            this.Property(x => x.FrequencyFrom, "Частота от").Column("FREQUENCY_FROM");
            this.Property(x => x.FrequencyTo, "Частота до").Column("FREQUENCY_TO");
            this.Property(x => x.Range, "Диапазон").Column("RANGE");
            this.Property(x => x.NumberApartments, "Количество подключенных квартир").Column("NUMBER_APARTMENTS");
            this.Property(x => x.Reason, "Причина отсутствия").Column("REASON");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.FileInfo, "Файл").Column("FILE_ID");
        }
    }
}
