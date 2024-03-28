/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Управляющая организация"
///     /// </summary>
///     public class ManagingOrganizationMap : BaseGkhEntityMap<ManagingOrganization>
///     {
///         public ManagingOrganizationMap() : base("GKH_MANAGING_ORGANIZATION")
///         {
///             Map(x => x.OrgStateRole, "ORG_STATE_ROLE").Not.Nullable().CustomType<OrgStateRole>();
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.TypeManagement, "TYPE_MANAGEMENT").Not.Nullable().CustomType<TypeManagementManOrg>();
/// 
///             // дополнительные реквизиты
///             Map(x => x.MemberRanking, "MEMBER_RANKING").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.CountMo, "COUNT_MO");
///             Map(x => x.CountOffices, "COUNT_OFFICES");
///             Map(x => x.CountSrf, "COUNT_SRF");
///             Map(x => x.IsTransferredManagementTsj, "IS_TRANSFER_MANAGE_TSJ").Not.Nullable();
///             Map(x => x.NumberEmployees, "NUMBER_EMPLOYEES");
///             Map(x => x.OfficialSite, "OFFICIAL_SITE").Length(100);
///             Map(x => x.OfficialSite731, "OFFICIAL_SITE_731").Not.Nullable();
///             Map(x => x.ShareMo, "SHARE_MO");
///             Map(x => x.ShareSf, "SHARE_SF");
///             
///             // деятельность
///             Map(x => x.ActivityDateEnd, "ACTIVITY_DATE_END");
///             Map(x => x.ActivityDescription, "ACTIVITY_DESCRIPTION");
///             Map(x => x.ActivityGroundsTermination, "ACTIVITY_TERMINATION").Not.Nullable().CustomType<GroundsTermination>();
/// 
///             //nso
///             Map(x => x.CaseNumber, "CASE_NUMBER").Nullable().Length(100);
///             References(x => x.TsjHead, "TSJ_HEAD_CONTACT_ID").Nullable().LazyLoad();
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Управляющая организация"</summary>
    public class ManagingOrganizationMap : BaseImportableEntityMap<ManagingOrganization>
    {
        
        public ManagingOrganizationMap() : 
                base("Управляющая организация", "GKH_MANAGING_ORGANIZATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.OrgStateRole, "Статус").Column("ORG_STATE_ROLE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.TypeManagement, "Тип управления").Column("TYPE_MANAGEMENT").NotNull();
            Property(x => x.MemberRanking, "Участвует в рейтинге УК").Column("MEMBER_RANKING").NotNull();
            Property(x => x.CountMo, "Количество МО").Column("COUNT_MO");
            Property(x => x.CountOffices, "Количество офисов").Column("COUNT_OFFICES");
            Property(x => x.CountSrf, "Количество СРФ").Column("COUNT_SRF");
            Property(x => x.IsTransferredManagementTsj, "Передано управление (Только для ТСЖ)").Column("IS_TRANSFER_MANAGE_TSJ").NotNull();
            Property(x => x.NumberEmployees, "Общая численность сотрудников").Column("NUMBER_EMPLOYEES");
            Property(x => x.OfficialSite, "Официальный сайт").Column("OFFICIAL_SITE").Length(100);
            Property(x => x.OfficialSite731, "Официальный сайт для раскрытия информации по 731").Column("OFFICIAL_SITE_731").NotNull();
            Property(x => x.ShareMo, "Доля участия МО (%)").Column("SHARE_MO");
            Property(x => x.ShareSf, "Доля участия СФ (%)").Column("SHARE_SF");
            Property(x => x.ActivityDateEnd, "Дата окончания деятельности").Column("ACTIVITY_DATE_END");
            Property(x => x.ActivityDescription, "Описание для деятельности").Column("ACTIVITY_DESCRIPTION");
            Property(x => x.ActivityGroundsTermination, "Основание прекращения деятельности").Column("ACTIVITY_TERMINATION").NotNull();
            Property(x => x.CaseNumber, "№ дела").Column("CASE_NUMBER").Length(100);
            Property(x => x.IsDispatchCrrespondedFact, "Устав товарищества собственников жилья или кооператива").Column("IS_DISPATCH_CORR_FACT").NotNull();
            Property(x => x.DispatchPhone, "Контактные номера телефонов").Column("DISPATCH_PHONE");

            Reference(x => x.TsjHead, "Председатель ТСЖ").Column("TSJ_HEAD_CONTACT_ID");
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull();
            Reference(x => x.DispatchAddress, "Адрес диспетчерской службы").Column("FIAS_DISPATCH_ID");
            Reference(x => x.DispatchFile, "Устав товарищества собственников жилья или кооператива").Column("FILE_STATUTE_ID");
        }
    }
}
