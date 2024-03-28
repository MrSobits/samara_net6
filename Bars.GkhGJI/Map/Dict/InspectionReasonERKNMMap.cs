
namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;


    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.InspectionReason"</summary>
    public class InspectionReasonERKNMMap : BaseEntityMap<InspectionReasonERKNM>
    {

        public InspectionReasonERKNMMap() :
                base("Bars.GkhGji.Entities.Dict.InspectionReason", "GJI_DICT_DECISION_REASON_ERKNM")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(1500).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(50).NotNull();
            Property(x => x.ERKNMDocumentType, "ERKNMDocumentType").Column("DOC_TYPE").NotNull();
            Property(x => x.ERKNMId, "ERKNMId").Column("ERKNMID").Length(5).NotNull();
        }
    }
}
