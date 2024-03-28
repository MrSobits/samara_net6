namespace Bars.GkhCr.Regions.Tatarstan.Map.ObjectOutdoorCr
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrMap : BaseEntityMap<ObjectOutdoorCr>
    {
        /// <inheritdoc />
        public ObjectOutdoorCrMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr", "CR_OBJECT_OUTDOOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.DateEndBuilder, "Дата завершения работ подрядчиком").Column("DATE_END_BUILDER");
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            this.Property(x => x.СommissioningDate, "Дата принятия на регистрацию").Column("COMISSIONING_DATE");
            this.Property(x => x.SumDevolopmentPsd, "Сумма на разработку экспертизы ПСД").Column("SUM_DEV_PSD");
            this.Property(x => x.SumSmr, "Сумма на СМР").Column("SUM_SMR");
            this.Property(x => x.SumSmrApproved, "Утвержденная сумма").Column("SUM_SMR_APPROVED");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(255);
            this.Property(x => x.MaxAmount, "Предельная сумма из КПКР").Column("MAX_AMOUNT");
            this.Property(x => x.FactAmountSpent, "Фактически освоенная сумма").Column("FACT_AMOUNT_SPENT");
            this.Property(x => x.FactStartDate, "Фактическая дата начала работ").Column("FACT_START_DATE");
            this.Property(x => x.FactEndDate, "Фактическая дата окончания работ").Column("FACT_END_DATE");
            this.Property(x => x.WarrantyEndDate, "Дата окончания гарантийных обязательств").Column("WARRANTY_END_DATE");
            this.Property(x => x.GjiNum, "Номер ГЖИ").Column("GJI_NUM").Length(255);
            this.Property(x => x.DateAcceptGji, "Дата принятия ГЖИ").Column("DATE_ACCEPT_GJI");
            this.Property(x => x.DateGjiReg, "Дата регистрации ГЖИ").Column("DATE_GJI_REG");
            this.Property(x => x.DateStopWorkGji, "Дата остановки работ ГЖИ").Column("DATE_STOP_WORK_GJI");
            this.Reference(x => x.RealityObjectOutdoorProgram, "Программа").Column("PROGRAM_OUTDOOR_ID").Fetch();
            this.Reference(x => x.BeforeDeleteRealityObjectOutdoorProgram, "Программа, на которую ссылался объект до удаления").Column("BEFORE_DELETE_PROGRAM_OUTDOOR_ID").Fetch();
            this.Reference(x => x.RealityObjectOutdoor, "Двор").Column("REALITY_OBJECT_OUTDOOR_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
