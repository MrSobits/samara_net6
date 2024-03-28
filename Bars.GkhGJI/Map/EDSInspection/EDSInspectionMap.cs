namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    public class EDSInspectionMap : BaseEntityMap<EDSInspection>
    {

        public EDSInspectionMap() :
                base("Проверка СЭД", "GJI_EDS_INSPECTION")
        {
        }

        protected override void Map()
        {
            Property(x => x.InspectionDate, "Дата проверки").Column("INSPECTION_DATE").NotNull();
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            Property(x => x.NotOpened, "Не прочитано").Column("IS_NOT_READ").NotNull();
            Property(x => x.TypeBase, "Основание").Column("TYPE_BASE").NotNull();
            Reference(x => x.InspectionGji, "Проверка").Column("INSPECTION_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
