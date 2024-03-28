namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectContractMap : BaseEntityMap<ConstructionObjectContract>
    {
        public ConstructionObjectContractMap() :
            base("Договор для объекта строительства", "GKH_CONSTRUCT_OBJ_CONTRACT")
        {      
        }

        protected override void Map()
        {
            this.Reference(x => x.ConstructionObject, "Объект строительства").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.Type, "Тип договора").Column("TYPE").NotNull();
            this.Property(x => x.Name, "Наименование документа").Column("NAME");
            this.Property(x => x.Date, "Дата документа").Column("DATE");
            this.Property(x => x.Number, "Номер документа").Column("NUMBER");
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Property(x => x.Sum, "Сумма договора").Column("SUM");
            this.Property(x => x.DateStart, "Дата начала действия договора").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания действия договора").Column("DATE_END");
            this.Reference(x => x.Contragent, "Участник процесса").Column("CONTRAGENT_ID").Fetch();
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
        }
    }
}