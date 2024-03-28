namespace Bars.GkhGji.Map.Dict
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities.Dict;

    public class InspectionBaseTypeMap : GkhBaseEntityMap<InspectionBaseType>
    {
        public InspectionBaseTypeMap()
            : base("GJI_DICT_INSPECTION_BASE_TYPE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            this.Property(x => x.InspectionKindId, "Вид проверки").Column("INSPECTION_KIND_ID").NotNull();
            this.Property(x => x.SendErp, "Значение передается в ЕРП").Column("VAL_SEND_ERP").NotNull();
            //this.Property(x => x.TorId, "Идентификатор в ТОР КНД").Column("TOR_ID");
            this.Property(x => x.ErknmCode, "Код в ЕРКНМ").Column("CODE_ERKNM");
            this.Property(x => x.HasTextField, "Наличие текстового поля").Column("HAS_TEXT_FIELD");
            this.Property(x => x.HasDate, "Наличие даты").Column("HAS_DATE");
            this.Property(x => x.HasRiskIndicator, "Наличие индикатора риска").Column("HAS_RISK_INDICATOR");
        }
    }
}