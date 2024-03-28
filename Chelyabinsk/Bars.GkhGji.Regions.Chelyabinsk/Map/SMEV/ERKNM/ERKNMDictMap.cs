namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class ERKNMDictMap : BaseEntityMap<ERKNMDict>
    {
        
        public ERKNMDictMap() : 
                base("Запрос справочника ЕРКНМ", "GJI_CH_GIS_ERKNM_DICT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGE_ID");
            Property(x => x.DictGuid, "ГУИД справочника").Column("DICT_GUID");
            Property(x => x.ControlTypeGuid, "ГУИД КНМ").Column("KNM_GUID");
            Property(x => x.KNOGuid, "ГУИД КНО").Column("KNO_GUID");
            Property(x => x.CompareDate, "Дата сопомтавления").Column("COMPARE_DATE");
            Property(x => x.Answer, "Результат").Column("ANSWER");
        }
    }
}
