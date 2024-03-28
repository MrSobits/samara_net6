namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Акт выполненных работ"</summary>
    public class SpecialPerformedWorkActMap : BaseImportableEntityMap<SpecialPerformedWorkAct>
    {
        public SpecialPerformedWorkActMap() : 
                base("Акт выполненных работ", "CR_SPECIAL_OBJ_PERFOMED_WORK_ACT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
            this.Property(x => x.DocumentNum, "Номер акта").Column("DOCUMENT_NUM").Length(300);
            this.Property(x => x.Volume, "Объем").Column("VOLUME");
            this.Property(x => x.Sum, "Сумма").Column("SUM");
            this.Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            this.Property(x => x.RepresentativeSigned, "Акт подписан представителем собственников").Column("REPRESENTATIVE_SIGNED");
            this.Property(x => x.RepresentativeSurname, "Фамилия представителя").Column("REPRESENTATIVE_SURNAME");
            this.Property(x => x.RepresentativeName, "Имя представителя").Column("REPRESENTATIVE_NAME");
            this.Property(x => x.RepresentativePatronymic, "Отчество представителя").Column("REPRESENTATIVE_PATRONYMIC");
            this.Property(x => x.ExploitationAccepted, "Принято в эксплуатацию").Column("EXPLOITATION_ACCEPTED");
            this.Property(x => x.WarrantyStartDate, "Дата начала гарантийного срока").Column("WARRANTY_START_DATE");
            this.Property(x => x.WarrantyEndDate, "Дата окончания гарантийного срока").Column("WARRANTY_END_DATE");

            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").Fetch();
            this.Reference(x => x.TypeWorkCr, "Работа").Column("TYPE_WORK_CR_ID").NotNull();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.CostFile, "Справка о стоимости выполненных работ и затрат").Column("COST_FILE_ID").Fetch();
            this.Reference(x => x.DocumentFile, "Документ акта").Column("DOC_FILE_ID").Fetch();
            this.Reference(x => x.AdditionFile, "Приложение к акту").Column("ADDIT_FILE_ID").Fetch();
        }
    }
}
