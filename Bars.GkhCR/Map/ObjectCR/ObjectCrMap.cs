namespace Bars.GkhCr.Map.ObjectCr
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Объект капитального ремонта"</summary>
    public class ObjectCrMap : BaseImportableEntityMap<ObjectCr>
    {
        
        public ObjectCrMap() : 
                base("Объект капитального ремонта", "CR_OBJECT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.GjiNum, "Номер ГЖИ").Column("GJI_NUM").Length(300);
            this.Property(x => x.ProgramNum, "Номер по программе").Column("PROGRAM_NUM").Length(300);
            this.Property(x => x.DateEndBuilder, "Дата завершения работ подрядчиком").Column("DATE_END_BUILDER");
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            this.Property(x => x.DateStopWorkGji, "Дата остановки работ ГЖИ").Column("DATE_STOP_WORK_GJI");
            this.Property(x => x.DateCancelReg, "Дата отклонения от регистрации").Column("DATE_CANCEL_REG");
            this.Property(x => x.DateAcceptCrGji, "Дата принятия КР ГЖИ").Column("DATE_ACCEPT_GJI");
            this.Property(x => x.DateAcceptReg, "Дата принятия на регистрацию").Column("DATE_ACCEPT_REG");
            this.Property(x => x.DateGjiReg, "Дата регистрации ГЖИ").Column("DATE_GJI_REG");
            this.Property(x => x.SumDevolopmentPsd, "Сумма на разработку экспертизы ПСД").Column("SUM_DEV_PSD");
            this.Property(x => x.SumSmr, "Сумма на СМР").Column("SUM_SMR");
            this.Property(x => x.SumSmrApproved, "Утвержденная сумма").Column("SUM_SMR_APPROVED");
            this.Property(x => x.SumTehInspection, "Сумма на технадзор").Column("SUM_TECH_INSP");
            this.Property(x => x.FederalNumber, "Федеральный номер").Column("FEDERAL_NUM").Length(300);
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.AllowReneg, "Разрешение на повторное согласование").Column("ALLOW_RENEG");
            this.Property(x => x.MaxKpkrAmount, "Предельная сумма из КПКР").Column("MAX_KPKR_AMOUNT");
            this.Property(x => x.FactAmountSpent, "Фактически освоенная сумма").Column("FACT_AMOUNT_SPENT");
            this.Property(x => x.FactStartDate, "Фактическая дата начала работ").Column("FACT_START_DATE");
            this.Property(x => x.FactEndDate, "Фактическая дата окончания работ").Column("FACT_END_DATE");
            this.Property(x => x.WarrantyEndDate, "Дата окончания гарантийных обязательств").Column("WARRANTY_END_DATE");
            this.Reference(x => x.ProgramCr, "Программа").Column("PROGRAM_ID").Fetch();
            this.Reference(x => x.BeforeDeleteProgramCr, "Программа, на которую ссылался объект до удаления").Column("BEFORE_DELETE_PROGRAM_ID").Fetch();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}