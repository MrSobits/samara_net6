namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class ViewWarningInspectionMap : PersistentObjectMap<ViewWarningInspection>
    {
        
        public ViewWarningInspectionMap() : 
                base("Реестр предостережений", "VIEW_WARNING_INSPECTION")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.Municipality, "Мунниципальный район").Column("MUNICIPALITY");
            this.Property(x => x.ContragentName, "Юридическое лицо").Column("CONTRAGENT_NAME");
            this.Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION");
            this.Property(x => x.TypeJurPerson, "Тип контрагента").Column("TYPE_JUR_PERSON");
            this.Property(x => x.InspectionDate, "Дата проверки").Column("INSPECTION_DATE");
            this.Property(x => x.CheckDate, "Дата документа").Column("CHECK_DATE");
            this.Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            this.Property(x => x.DocumentNumber, "Номер").Column("DOCUMENT_NUMBER");
            this.Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            this.Property(x => x.RegistrationNumber, "Учетный номер проверки в едином реестре").Column("REGISTR_NUMBER");
            this.Property(x => x.Inspectors, "Инспекторы").Column("INSPECTORS");
            this.Property(x => x.AppealCitsNumberDate, "Номер обращения гражданина").Column("APPEALCITS_NUMBER_DATE");
        }
    }
}
