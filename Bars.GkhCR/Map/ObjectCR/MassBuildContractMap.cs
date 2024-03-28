namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Договор подряда КР"</summary>
    public class MassBuildContractMap : BaseImportableEntityMap<MassBuildContract>
    {

        public MassBuildContractMap() :
                base("Договор подряда КР", "CR_OBJ_MASS_BUILD_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
            this.Reference(x => x.ProgramCr, "Программа капитального ремонта").Column("PROGRAM_ID").NotNull().Fetch();
            this.Reference(x => x.Inspector, "Инспекторы").Column("INSPECTOR_ID").Fetch();
            this.Reference(x => x.Builder, "Подрядчики").Column("BUILDER_ID").Fetch();
            this.Property(x => x.TypeContractBuild, "Тип договора КР").Column("TYPE_CONTRACT_BUILD").NotNull();
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            this.Property(x => x.DateInGjiRegister, "Договор внесен в реестр ГЖИ").Column("DATE_GJI");
            this.Property(x => x.DocumentDateFrom, "Дата от (документ)").Column("DOCUMENT_DATE_FROM");
            this.Property(x => x.BudgetMo, "Бюджет МО").Column("BUDGET_MO");
            this.Property(x => x.BudgetSubject, "Бюджет субъекта").Column("BUDGET_SUBJ");
            this.Property(x => x.OwnerMeans, "Средства собственников").Column("OWNER_MEANS");
            this.Property(x => x.FundMeans, "Средства фонда").Column("FUND_MEANS");
            this.Property(x => x.DateCancelReg, "Дата отклонения от регистрации").Column("DATE_CANCEL");
            this.Property(x => x.DateAcceptOnReg, "Дата принятия на регистрацию, но еще не зарегистрированно").Column("DATE_ACCEPT");
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.Sum, "Сумма договора подряда").Column("SUM");
            this.Property(x => x.StartSum, "Сумма договора подряда").Column("START_SUM");
            this.Reference(x => x.DocumentFile, "Файл (документ)").Column("DOCUMENT_FILE_ID");
            this.Property(x => x.ProtocolName, "Протокол").Column("PROTOCOL_NAME").Length(300);
            this.Property(x => x.ProtocolNum, "Номер протокола").Column("PROTOCOL_NUM").Length(50);
            this.Property(x => x.ProtocolDateFrom, "Дата от (протокол)").Column("PROTOCOL_DATE_FROM");
            this.Reference(x => x.ProtocolFile, "Файл (протокол)").Column("PROTOCOL_FILE_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            this.Reference(x => x.Contragent, "Заказчик").Column("CONTRAGENT_ID");
            this.Property(x => x.TerminationDate, "Дата расторжения").Column("TERMINATION_DATE");
            this.Reference(x => x.TerminationDocumentFile, "Документ-основание").Column("TERMINATION_DOCUMENT_FILE_ID");
            this.Property(x => x.TerminationReason, "Основание расторжения").Column("TERMINATION_REASON");
            this.Property(x => x.GuaranteePeriod, "Гарантийный срок (лет)").Column("GUARANTEE_PERIOD");
            this.Property(x => x.UrlResultTrading, "Ссылка на результаты проведения торгов").Column("URL_RESULT_TRADING");
            this.Property(x => x.TerminationDocumentNumber, "Номер документа").Column("TERMINATION_DOCUMENT_NUMBER");
            this.Reference(x => x.TerminationDictReason, "Причина расторжения").Column("TERMINATION_REASON_ID");
        }
    }

}
