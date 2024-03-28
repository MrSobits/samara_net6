/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "контрагенты, проводящие проверку юр.лица"
///     /// </summary>
///     public class BaseJurPersonContragentMap : BaseEntityMap<BaseJurPersonContragent>
///     {
///         public BaseJurPersonContragentMap() : base("GJI_BASEJURPERSON_CONTRAGENT")
///         {
///             References(x => x.BaseJurPerson, "BASEJURPERSON_ID", ReferenceMapConfig.NotNull);
///             References(x => x.Contragent, "CONTRAGENT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.BaseJurPersonContragent"</summary>
    public class BaseJurPersonContragentMap : BaseEntityMap<BaseJurPersonContragent>
    {
        
        public BaseJurPersonContragentMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.BaseJurPersonContragent", "GJI_BASEJURPERSON_CONTRAGENT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.BaseJurPerson, "BaseJurPerson").Column("BASEJURPERSON_ID").NotNull();
            this.Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID");
        }
    }
}
