namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectMap : BaseEntityMap<ConstructionObject>
    {
        public ConstructionObjectMap()
            :
                base("Объекты строительства", "GKH_CONSTRUCTION_OBJECT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Address, "Адрес").Column("ADDRESS").Length(1000);
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.SumSmr, "Сумма на СМР").Column("SUM_SMR");
            this.Property(x => x.SumDevolopmentPsd, "Сумма на разработку экспертизы ПСД").Column("SUM_DEV_PSD");
            this.Property(x => x.DateEndBuilder, "Дата завершения работ подрядчиком").Column("DATE_END_BUILDER");
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateStopWork, "Дата остановки работ").Column("DATE_STOP_WORK");
            this.Property(x => x.DateResumeWork, "Дата возобновления работ").Column("DATE_RESUME_WORK");
            this.Property(x => x.ReasonStopWork, "Причина остановки работ").Column("REASON_STOP_WORK");
            this.Property(x => x.DateCommissioning, "Дата сдачи в эксплуатацию").Column("DATE_COMMISSIONING");
            this.Property(x => x.LimitOnHouse, "Лимит на дом").Column("LIMIT_ON_HOUSE");
            this.Property(x => x.TotalArea, "Общая площадь дома").Column("TOTAL_AREA");
            this.Property(x => x.NumberApartments, "Количество квартир").Column("NUM_APARTMENTS");
            this.Property(x => x.ResettleProgNumberApartments, "Количество квартир по программе переселения").Column("RESETTLE_PROG_NUM_APARTS");
            this.Property(x => x.NumberFloors, "Количетсво этажей").Column("NUM_FLOORS");
            this.Property(x => x.NumberEntrances, "Количество подъездов").Column("NUM_ENTRANCES");
            this.Property(x => x.NumberLifts, "Количество лифтов").Column("NUM_LIFTS");
            this.Property(x => x.TypeRoof, "Тип кровли").Column("TYPE_ROOF").NotNull();

            this.Reference(x => x.FiasAddress, "Адрес ФИАС").Column("FIAS_ADDRESS_ID");
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
            this.Reference(x => x.MoSettlement, "МО - поселение").Column("STL_MUNICIPALITY_ID").Fetch();
            this.Reference(x => x.RoofingMaterial, "Материал кровли").Column("ROOFING_MATERIAL_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.WallMaterial, "Материал стен").Column("WALL_MATERIAL_ID");
            this.Reference(x => x.ResettlementProgram, "Программа переселения").Column("RESETTLEMENT_PROGRAM_ID").Fetch();
        }
    }
}