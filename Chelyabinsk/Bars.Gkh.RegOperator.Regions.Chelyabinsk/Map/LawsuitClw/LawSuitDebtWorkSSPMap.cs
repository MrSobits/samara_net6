namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Entities;

    /// <summary>Маппинг для "Исковое зявление"</summary>
    public class LawSuitDebtWorkSSPMap : BaseEntityMap<LawSuitDebtWorkSSP>
    {

        public LawSuitDebtWorkSSPMap() :
                base("Исковое зявление", "CLW_LAWSUIT_DEBTWORK")
        {
        }

        protected override void Map()
        {
         
            this.Property(x => x.CbSize, "Взыскания долга - Размер погашения").Column("CB_SIZE");
            this.Property(x => x.CbDebtSum, "Взыскания долга - Сумма долга").Column("CB_DEBT_SUM");
            this.Property(x => x.CbPenaltyDebt, "Взыскания долга - Сумма пени").Column("CB_PENALTY_DEBT");
            this.Property(x => x.CbFactInitiated, "Взыскания долга - факт возбуждения дела").Column("CB_FACT_INITIATED").DefaultValue(LawsuitFactInitiationType.NotInitiated).NotNull();
            this.Property(x => x.FactInitiatedNote, "Взыскания долга - Факт возбуждения дела - Примечание").Column("FACT_INITIATED_NOTE").Length(255);
            this.Property(x => x.CbDateInitiated, "Взыскания долга - Дата возбуждения").Column("CB_DATE_INITIATED");
            this.Reference(x => x.CbStationSsp, "Взыскания долга - Участок ССП").Column("CB_SSP_JINST_ID").Fetch();
            this.Reference(x => x.LawsuitOwnerInfo, "Дольщик").Column("OWNER_INFO").Fetch();
            this.Property(x => x.CbDateSsp, "Взыскания долга - Дата направления ССП").Column("CB_SSP_DATE");
            this.Property(x => x.CbDocumentType, "Взыскания долга - факт возбуждения дела").Column("CB_DOCUMENT_TYPE").DefaultValue(LawsuitCollectionDebtDocumentType.NotSet).NotNull();
            this.Property(x => x.CbSumRepayment, "Взыскания долга - сумма для погашения").Column("CB_SUM_REPAYMENT");
            this.Property(x => x.CbDateDocument, "Взыскания долга - Дата документа").Column("CB_DATE_DOC");
            this.Property(x => x.CbNumberDocument, "Взыскания долга - Номер документа").Column("CB_NUMBER_DOC").Length(100);
            this.Reference(x => x.CbFile, "Взыскания долга - Файл").Column("CB_FILE_ID").Fetch();
            this.Property(x => x.CbSumStep, "Взыскания долга - сумма, пошаговая в рамках производства").Column("CB_SUM_STEP");
            this.Property(x => x.CbIsStopped, "Взыскания долга - Производство остановлено").Column("CB_IS_STOPP").DefaultValue(false).NotNull();
            this.Property(x => x.CbDateStopped, "Взыскания долга - Дата остановки").Column("CB_DATE_STOPP");
            this.Property(x => x.CbReasonStoppedType, "Взыскания долга - Причина").Column("CB_REASON_STOPP");
            this.Property(x => x.CbReasonStoppedDescription, "Взыскания долга - Описание причины").Column("CB_REASON_STOPPDESC").Length(2000);
            this.Property(x => x.DateDirectionForSsp, "Взыскания долга - Дата направления на исполнение в ССП").Column("CB_DATE_DIRECTION_SSP");
            this.Reference(x => x.Lawsuit, "Заявление").Column("LAWSUIT_ID").NotNull().Fetch();
            this.Property(x => x.DebtWorkType, "Признак количества дел").Column("DEBT_WORK_TYPE");
            this.Property(x => x.CbDocReturned, "Документ возвращен").Column("CB_DOC_RETURNED");
            this.Property(x => x.LackOfPropertyAct, "Получен акт об отсутствии имущества").Column("LACK_OF_PPROPERTY_ACT");
            this.Property(x => x.LackOfPropertyActDate, "Дата акта об отсутствии имущества").Column("LACK_OF_PPROPERTY_ACT_DATE");
        }
    }
}
