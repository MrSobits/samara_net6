/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Должности"
///     /// </summary>
///     public class PositionMap : BaseGkhEntityMap<Position>
///     {
///         public PositionMap() : base("GKH_DICT_POSITION")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
/// 
///             Map(x => x.NameGenitive, "NAME_GENETIVE").Length(300);
///             Map(x => x.NameDative, "NAME_DATIVE").Length(300);
///             Map(x => x.NameAccusative, "NAME_ACCUSATIVE").Length(300);
///             Map(x => x.NameAblative, "NAME_ABLATIVE").Length(300);
///             Map(x => x.NamePrepositional, "NAME_PREPOSITIONAL").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Должность"</summary>
    public class PositionMap : BaseImportableEntityMap<Position>
    {
        
        public PositionMap() : 
                base("Должность", "GKH_DICT_POSITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.NameGenitive, "Наименование, родительский падеж").Column("NAME_GENETIVE").Length(300);
            Property(x => x.NameDative, "Наименование, Дательный падеж").Column("NAME_DATIVE").Length(300);
            Property(x => x.NameAccusative, "Наименование, Винительный падеж").Column("NAME_ACCUSATIVE").Length(300);
            Property(x => x.NameAblative, "Наименование, Творительный падеж").Column("NAME_ABLATIVE").Length(300);
            Property(x => x.NamePrepositional, "Наименование, Предложный падеж").Column("NAME_PREPOSITIONAL").Length(300);
        }
    }
}
