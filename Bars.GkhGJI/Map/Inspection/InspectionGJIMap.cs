namespace Bars.GkhGji.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Проверка ГЖИ"</summary>
    public class InspectionGjiMap : GkhBaseEntityMap<InspectionGji>
    {

        public InspectionGjiMap() : base("GJI_INSPECTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE").NotNull();
            this.Property(x => x.TypeJurPerson, "Тип юридического лица").Column("TYPE_JUR_PERSON").NotNull();
            this.Property(x => x.InspectionNumber, "Номер документа").Column("INSPECTION_NUMBER").Length(20);
            this.Property(x => x.InspectionNum, "Номер документа (Целая часть)").Column("INSPECTION_NUM");
            this.Property(x => x.InspectionYear, "Год документа").Column("INSPECTION_YEAR");
            this.Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            this.Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION").NotNull();
            this.Reference(x => x.Contragent, "Контрагент проверки").Column("CONTRAGENT_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.InspectionBaseType, "Основание проверки").Column("INSPECTION_TYPE_BASE_ID").Fetch();
            this.Property(x => x.RegistrationNumber, "Учетный номер проверки в едином реестре проверок").Column("REGISTR_NUMBER");
            this.Property(x => x.RegistrationNumberDate, "Дата присвоения учетного номера").Column("REGISTR_NUMBER_DATE");
            this.Property(x => x.CheckDayCount, "Срок проверки (количество дней)").Column("CHECK_DAY_COUNT");
            this.Property(x => x.CheckDate, "Дата проверки").Column("CHECK_DATE");
            this.Property(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE");
            this.Property(x => x.ReasonErpChecking, "Основание проверки ЕРП").Column("REASON_ERP_CHECKING");
        }
    }
}
