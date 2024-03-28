namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для "Основание плановой проверки юр. лиц ГЖИ"</summary>
    public class BaseOMSUMap : JoinedSubClassMap<BaseOMSU>
    {
        /// <summary>
        /// Конструктор
        ///// </summary>
        public BaseOMSUMap() : 
                base("Основание плановой проверки ОМСУ ГЖИ", "GJI_CH_INSPECTION_OMSU")
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
            this.Property(x => x.TypeBaseOMSU, "Тип основания проверки ОМСУ").Column("TYPE_BASE_JURAL").NotNull();
            this.Property(x => x.TypeFact, "Факт проверки ОМСУ").Column("TYPE_FACT").NotNull();
            this.Property(x => x.TypeForm, "Форма проверки ОМСУ").Column("TYPE_FORM").NotNull();
            this.Property(x => x.OmsuPerson, "Должностное лицо ОМСУ").Column("OMSU_PERSON");
            this.Reference(x => x.Plan, "План проверки ОМСУ").Column("PLAN_ID");
            this.Property(x => x.UriRegistrationNumber, "Дата начала проверки").Column("URI_REGISTRATION_NUMBER");
            this.Property(x => x.UriRegistrationDate, "Дата присвоения учетного номера").Column("URI_REGISTRATION_DATE");
        }
    }
}
