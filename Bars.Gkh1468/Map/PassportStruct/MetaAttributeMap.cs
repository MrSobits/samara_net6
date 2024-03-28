/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.Gkh1468.Entities;
/// 
///     public class MetaAttributeMap : BaseGkhEntityByCodeMap<MetaAttribute>
///     {
///         public MetaAttributeMap()
///             : base("GKH_PSTRUCT_META_ATTR")
///         {
///             Map(x => x.Code, "CODE", false);
///             Map(x => x.OrderNum, "ORDER_NUM", false, 0);
///             Map(x => x.Name, "NAME");
///             Map(x => x.Type, "TYPE", false);
///             Map(x => x.ValueType, "VALUE_TYPE", false);
///             Map(x => x.DictCode, "DICT_CODE", false);
///             Map(x => x.ValidateChilds, "VALIDATE_CHILDS", false);
///             Map(x => x.GroupText, "GROUP_TEXT", false);
///             Map(x => x.IntegrationCode, "INTEGRATION_CODE", false);
///             Map(x => x.MaxLength, "MAX_LENGTH", false);
///             Map(x => x.MinLength, "MIN_LENGTH", false);
///             Map(x => x.Pattern, "PATTERN", false);
///             Map(x => x.Exp, "Exp", false);
///             Map(x => x.AllowNegative, "ALLOW_NEGATIVE", false);
///             Map(x => x.Required, "REQUIRED", false);
///             Map(x => x.DataFillerCode, "FILLER_CODE", false);
///             Map(x => x.UseInPercentCalculation, "USE_PERC_CALC");
///             References(x => x.Parent, "PARENT_ID");
///             References(x => x.ParentPart, "PARENT_PART_ID");
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Атрибут структуры паспорта"</summary>
    public class MetaAttributeMap : BaseEntityMap<MetaAttribute>
    {
        
        public MetaAttributeMap() : 
                base("Атрибут структуры паспорта", "GKH_PSTRUCT_META_ATTR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Property(x => x.Code, "Код атрибута").Column("CODE").Length(250);
            Property(x => x.OrderNum, "Порядок атрибута").Column("ORDER_NUM");
            Property(x => x.Name, "Наименование").Column("NAME").Length(250);
            Property(x => x.Type, "Тип атрибута").Column("TYPE");
            Property(x => x.ValueType, "Тип хранимого значения").Column("VALUE_TYPE");
            Property(x => x.DictCode, "Тип хранимого значения").Column("DICT_CODE").Length(250);
            Reference(x => x.ParentPart, "Родительский раздел").Column("PARENT_PART_ID");
            Reference(x => x.Parent, "Родительский атрибут").Column("PARENT_ID");
            Property(x => x.ValidateChilds, "Проверять соответсвие суммы значений дочерних атрибутов (для групповых со значени" +
                    "ем)").Column("VALIDATE_CHILDS");
            Property(x => x.GroupText, "Текст используемый при группировке (для групповых)").Column("GROUP_TEXT").Length(250);
            Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID");
            Property(x => x.IntegrationCode, "Код интеграции").Column("INTEGRATION_CODE").Length(250);
            Property(x => x.DataFillerCode, "Код заполнителя значения по-умолчанию").Column("FILLER_CODE").Length(250);
            Property(x => x.UseInPercentCalculation, "Учитывать поле при расчете процента заполнения").Column("USE_PERC_CALC");
            Property(x => x.MaxLength, "Поля используемые при валидации").Column("MAX_LENGTH");
            Property(x => x.MinLength, "MinLength").Column("MIN_LENGTH");
            Property(x => x.Pattern, "Pattern").Column("PATTERN").Length(250);
            Property(x => x.Exp, "Exp").Column("EXP");
            Property(x => x.Required, "Required").Column("REQUIRED");
            Property(x => x.AllowNegative, "AllowNegative").Column("ALLOW_NEGATIVE");
        }
    }
}
