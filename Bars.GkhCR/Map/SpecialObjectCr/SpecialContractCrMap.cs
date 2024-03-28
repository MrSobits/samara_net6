namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Договор КР"</summary>
    public class SpecialContractCrMap : BaseImportableEntityMap<SpecialContractCr>
    {

        public SpecialContractCrMap() :
                base("Договор КР", "CR_SPECIAL_OBJ_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.TypeContractObject, "Тип договора объекта КР").Column("TYPE_CONTRACT_ID").Fetch();
            this.Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            this.Reference(x => x.Customer, "Заказчик").Column("CUSTOMER_ID").Fetch();
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            this.Property(x => x.SumContract, "Сумма договора").Column("SUM");
            this.Property(x => x.BudgetMo, "Бюджет МО").Column("BUDGET_MO");
            this.Property(x => x.BudgetSubject, "Бюджет субъекта").Column("BUDGET_SUBJ");
            this.Property(x => x.OwnerMeans, "Средства собственников").Column("OWNER_MEANS");
            this.Property(x => x.FundMeans, "Средства фонда").Column("FUND_MEANS");
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }
}
