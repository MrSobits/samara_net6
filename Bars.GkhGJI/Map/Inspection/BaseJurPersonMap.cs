namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание плановой проверки юр. лиц ГЖИ"</summary>
    public class BaseJurPersonMap : JoinedSubClassMap<BaseJurPerson>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseJurPersonMap() : 
                base("Основание плановой проверки юр. лиц ГЖИ", "GJI_INSPECTION_JURPERSON")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.DateStart, "Дата начала проверки").Column("DATE_START");
            this.Property(x => x.CountDays, "Срок проверки (Количество дней)").Column("COUNT_DAYS");
            this.Property(x => x.CountHours, "Срок проверки (Количество часов)").Column("COUNT_HOURS");
            this.Property(x => x.Reason, "Причина").Column("REASON").Length(500);
            this.Property(x => x.AnotherReasons, "Иные основания проверки").Column("ANOTHER_REASONS").Length(500);
            this.Property(x => x.TypeBaseJuralPerson, "Тип основания проверки ЮЛ").Column("TYPE_BASE_JURAL").NotNull();
            this.Property(x => x.TypeFact, "Факт проверки ЮЛ").Column("TYPE_FACT").NotNull();
            this.Property(x => x.TypeForm, "Форма проверки ЮЛ").Column("TYPE_FORM").NotNull();
            this.Reference(x => x.Plan, "План проверки юр. лиц").Column("PLAN_ID");
            this.Property(x => x.UriRegistrationNumber, "Дата начала проверки").Column("URI_REGISTRATION_NUMBER");
            this.Property(x => x.UriRegistrationDate, "Дата присвоения учетного номера").Column("URI_REGISTRATION_DATE");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Property(x => x.PlanNumber, "Порядковый номер в плане").Column("PLAN_NUMBER");
        }
    }
}
