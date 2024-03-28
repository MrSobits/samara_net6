/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Представление"
///     /// </summary>
///     public class PresentationMap : SubclassMap<Presentation>
///     {
///         public PresentationMap()
///         {
///             Table("GJI_PRESENTATION");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(500);
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.TypeInitiativeOrg, "TYPE_INITIATIVE_ORG").Not.Nullable().CustomType<TypeInitiativeOrgGji>();
///             
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///             References(x => x.Official, "OFFICIAL_ID").Fetch.Join();
///             Map(x => x.DescriptionSet, "REQUIR_TEXT").Length(2000);
///             Map(x => x.ExecutantPost, "EXEC_POST").Length(200);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Представление"</summary>
    public class PresentationMap : JoinedSubClassMap<Presentation>
    {
        
        public PresentationMap() : 
                base("Представление", "GJI_PRESENTATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(500);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.TypeInitiativeOrg, "Тип инициативного органа").Column("TYPE_INITIATIVE_ORG").NotNull();
            Property(x => x.DescriptionSet, "Текст требования").Column("REQUIR_TEXT").Length(2000);
            Property(x => x.ExecutantPost, "Должность").Column("EXEC_POST").Length(200);
            Reference(x => x.Executant, "тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.Official, "Должностное лицо").Column("OFFICIAL_ID").Fetch();
        }
    }
}
