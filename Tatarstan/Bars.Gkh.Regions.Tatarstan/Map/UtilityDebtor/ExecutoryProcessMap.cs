namespace Bars.Gkh.Regions.Tatarstan.Map.UtilityDebtor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Маппинг для "Исполнительное производство"
    /// </summary>
    public class ExecutoryProcessMap : JoinedSubClassMap<ExecutoryProcess>
    {
        public ExecutoryProcessMap() : 
                base("Исполнительное производство", "CLW_EXECUTORY_PROCESS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.AccountOwner, "Абонент").Column("ACCOUNT_OWNER").Length(150);
            this.Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE").DefaultValue(OwnerType.Individual).NotNull();
            this.Reference(x => x.RealityObject, "Адрес абонента").Column("RO_ID").NotNull();

            this.Reference(x => x.JurInstitution, "Подразделение ОСП").Column("JUR_INSTITUTION_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Property(x => x.RegistrationNumber, "Регистрационный номер").Column("REG_NUMBER").Length(50);
            this.Property(x => x.Document, "Документ").Column("DOCUMENT").Length(50);
            this.Property(x => x.DebtSum, "Сумма для погашения").Column("DEBT_SUM");
            this.Property(x => x.PaidSum, "Сумма погашения в рамках производствa").Column("PAID_SUM");
            this.Property(x => x.DateStart, "Дата возбуждения").Column("DATE_START");
            this.Property(x => x.IsTerminated, "Производство прекращено").Column("IS_TERMINATED").DefaultValue(false).NotNull();
            this.Property(x => x.DateEnd, "Дата прекращения").Column("DATE_END");
            this.Property(x => x.TerminationReasonType, "Причина прекращения").Column("TERM_REASON_TYPE").DefaultValue(TerminationReasonType.NotSet).NotNull();
            this.Property(x => x.Notation, "Примечание").Column("NOTATION").Length(255);
            this.Property(x => x.Creditor, "Взыскатель").Column("CREDITOR").Length(255);
            this.Reference(x => x.LegalOwnerRealityObject, "Адрес юр.лица").Column("LEGAL_OWNER_RO_ID");
            this.Property(x => x.Inn, "ИНН").Column("INN").Length(50);
            this.Property(x => x.Clause, "Статья").Column("CLAUSE").Length(50);
            this.Property(x => x.Paragraph, "Пункт").Column("PARAGRAPH").Length(50);
            this.Property(x => x.Subparagraph, "Подпункт").Column("SUBPARAGRAPH").Length(50);
        }
    }
}