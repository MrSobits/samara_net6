/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.WasteCollection
/// {
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities.WasteCollection;
/// 
///     public class WasteCollectionPlaceMap : BaseEntityMap<WasteCollectionPlace>
///     {
///         public WasteCollectionPlaceMap()
///             : base("GIS_WASTE_COLL_PLACE")
///         {
///             References(x => x.RealityObject, "REAL_OBJ");
///             References(x => x.Customer, "CUSTOMER");
///             Map(x => x.TypeWaste, "TYPE_WASTE");
///             Map(x => x.TypeWasteCollectionPlace, "TYPE_WASTE_PLACE");
///             Map(x => x.PeopleCount, "PEOPLE_COUNT");
///             Map(x => x.ContainersCount, "CONTAINERS_COUNT");
///             Map(x => x.WasteAccumulationDaily, "ACCUM_DAILY");
///             Map(x => x.LandfillDistance, "LANDFILL_DIST");
///             Map(x => x.Comment, "COMMENT", false, 1000);
///             Property(x => x.ExportDaysWinter,
///                 m =>
///                 {
///                     m.Type<ImprovedJsonSerializedType<ExportWasteDays>>();
///                     m.Column("EXP_DAYS_WINTER");
///                 });
///             Property(x => x.ExportDaysSummer,
///                 m =>
///                 {
///                     m.Type<ImprovedJsonSerializedType<ExportWasteDays>>();
///                     m.Column("EXP_DAYS_SUMMER");
///                 });
///             References(x => x.Contractor, "CONTRACTOR");
///             Map(x => x.NumberContract, "NUMBER_CONTRACT", false, 200);
///             Map(x => x.DateContract, "DATE_CONTRACT");
///             References(x => x.FileContract, "FILE_CONTRACT", ReferenceMapConfig.Fetch);
///             Map(x => x.LandfillAddress, "LANDFILL_ADDRESS", false, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.WasteCollection
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Gis.Entities.WasteCollection;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.WasteCollection.WasteCollectionPlace"</summary>
    public class WasteCollectionPlaceMap : BaseEntityMap<WasteCollectionPlace>
    {
        
        public WasteCollectionPlaceMap() : 
                base("Bars.Gkh.Gis.Entities.WasteCollection.WasteCollectionPlace", "GIS_WASTE_COLL_PLACE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REAL_OBJ");
            Reference(x => x.Customer, "Customer").Column("CUSTOMER");
            Property(x => x.TypeWaste, "TypeWaste").Column("TYPE_WASTE");
            Property(x => x.TypeWasteCollectionPlace, "TypeWasteCollectionPlace").Column("TYPE_WASTE_PLACE");
            Property(x => x.PeopleCount, "PeopleCount").Column("PEOPLE_COUNT");
            Property(x => x.ContainersCount, "ContainersCount").Column("CONTAINERS_COUNT");
            Property(x => x.WasteAccumulationDaily, "WasteAccumulationDaily").Column("ACCUM_DAILY");
            Property(x => x.LandfillDistance, "LandfillDistance").Column("LANDFILL_DIST");
            Property(x => x.Comment, "Comment").Column("COMMENT").Length(1000);
            Property(x => x.ExportDaysWinter, "ExportDaysWinter").Column("EXP_DAYS_WINTER");
            Property(x => x.ExportDaysSummer, "ExportDaysSummer").Column("EXP_DAYS_SUMMER");
            Reference(x => x.Contractor, "Contractor").Column("CONTRACTOR");
            Property(x => x.NumberContract, "NumberContract").Column("NUMBER_CONTRACT").Length(200);
            Property(x => x.DateContract, "DateContract").Column("DATE_CONTRACT");
            Reference(x => x.FileContract, "FileContract").Column("FILE_CONTRACT").Fetch();
            Property(x => x.LandfillAddress, "LandfillAddress").Column("LANDFILL_ADDRESS").Length(2000);
        }
    }

    public class WasteCollectionPlaceNHibernateMapping : ClassMapping<WasteCollectionPlace>
    {
        public WasteCollectionPlaceNHibernateMapping()
        {
            Property(x => x.ExportDaysWinter,
                m =>
                {
                    m.Type<ImprovedJsonSerializedType<ExportWasteDays>>();
                });
            Property(x => x.ExportDaysSummer,
                m =>
                {
                    m.Type<ImprovedJsonSerializedType<ExportWasteDays>>();
                });
        }
    }
}
