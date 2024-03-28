namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectDocumentMap : BaseEntityMap<ConstructionObjectDocument>
    {
        public ConstructionObjectDocumentMap() :
            base("Документ для объекта строительства", "GKH_CONSTRUCT_OBJ_DOCUMENT")
        {      
        }

        protected override void Map()
        {
            this.Reference(x => x.ConstructionObject, "Объект строительства").Column("OBJECT_ID").NotNull().Fetch();
            this.Property(x => x.Type, "Тип документа").Column("TYPE").NotNull();
            this.Property(x => x.Name, "Наименование документа").Column("NAME");
            this.Property(x => x.Date, "Дата документа").Column("DATE");
            this.Property(x => x.Number, "Номер документа").Column("NUMBER");
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Reference(x => x.Contragent, "Участник процесса").Column("CONTRAGENT_ID").Fetch();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
        }
    }
}