namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>Маппинг для "План проверок юр. лиц ГЖИ"</summary>
    public class PlanJurPersonGjiMap : BaseEntityMap<PlanJurPersonGji>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PlanJurPersonGjiMap() : 
                base("План проверок юр. лиц ГЖИ", "GJI_DICT_PLANJURPERSON")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.DateApproval, "Дата утверждения плана").Column("DATE_APPROVAL");
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            this.Property(x => x.DateDisposal, "Дата приказа").Column("DATE_DISPOSAL");
            this.Property(x => x.NumberDisposal, "Номер приказа").Column("NUMBER_DISPOSAL").Length(50);
            this.Property(x => x.UriRegistrationNumber, "Регистрационный номер плана в едином реестре проверок").Column("URI_REGISTRATION_NUMBER");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Property(x => x.ErknmGuid, "Идентификатор плана в ЕРКНМ").Column("ERKNM_GUID").Length(36);;
        }
    }
}