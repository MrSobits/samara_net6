namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>Маппинг для "Исковое зявление"</summary>
    public class LawsuitMap : JoinedSubClassMap<Lawsuit>
    {

        public LawsuitMap() :
                base("Исковое зявление", "CLW_LAWSUIT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DateEnd, "Дата окончания работы").Column("DATE_END");
            this.Property(x => x.BidDate, "Дата заявки").Column("BID_DATE");
            this.Property(x => x.BidNumber, "Номер заявления").Column("BID_NUMBER").Length(100);
            this.Reference(x => x.PetitionType, "Тип заявления").Column("PETITION_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Property(x => x.DebtBaseTariffSum, "Сумма долга по базовому тарифу").Column("DEBT_BASE_TARIFF_SUM");
            this.Property(x => x.DebtDecisionTariffSum, "Сумма долга по тарифу решения").Column("DEBT_DECISION_TARIFF_SUM");
            this.Property(x => x.DebtSum, "Сумма долга").Column("DEBT_SUM");
            this.Property(x => x.PenaltyDebt, "Сумма пени").Column("PENALTY_DEBT");
            this.Property(x => x.Duty, "Гос. пошлина").Column("DUTY");
            this.Property(x => x.Costs, "Суд издержки").Column("COSTS");
            this.Property(x => x.JudgeName, "ФИО Судьи").Column("JUDGE_NAME");
            this.Property(x => x.NumberCourtBuisness, "Номер дела").Column("COURT_BUISNESS_NUMBER");
            this.Property(x => x.WhoConsidered, "Кем рассмотрено").Column("WHO_CONSIDERED").DefaultValue(LawsuitConsiderationType.NotSet).NotNull();
            this.Reference(x => x.JurInstitution, "Судебное учреждение").Column("JINST_ID").Fetch();
            this.Reference(x => x.JuridicalSectorMu, "Место нахожедния").Column("JURSECTOR_MU_ID").Fetch();
            this.Property(x => x.DateOfAdoption, "Дата принятия").Column("DATE_OF_ADOPTION");
            this.Property(x => x.DateOfRewiew, "Дата рассмотрения").Column("DATE_OF_REWIEW");
            this.Property(x => x.Suspended, "Приостановлено").Column("SUSPENDED");
            this.Property(x => x.DeterminationNumber, "Номер определения").Column("DETERMIN_NUMBER").Length(100);
            this.Property(x => x.DeterminationDate, "Дата определения").Column("DETERMIN_DATE");
            this.Property(x => x.ResultConsideration, "Результат рассмотрения").Column("RESULT_CONSIDERATION").DefaultValue(LawsuitResultConsideration.Denied).NotNull();
            this.Property(x => x.LawsuitDocType, "Тип документа").Column("LAW_TYPE_DOCUMENT").DefaultValue(LawsuitDocumentType.NotSet).NotNull();
            this.Property(x => x.ConsiderationDate, "Дата формирования").Column("DATE_CONSIDERAT");
            this.Property(x => x.ConsiderationNumber, "Номер формирования").Column("NUM_CONSIDERAT").Length(100);
            this.Property(x => x.DebtSumApproved, "Сумма признанной задолженности").Column("DEBT_SUM_APPROV");
            this.Property(x => x.PenaltyDebtApproved, "Сумма признанной пени").Column("PENALTY_DEBT_APPROV");
            this.Property(x => x.CbSize, "Взыскания долга - Размер погашения").Column("CB_SIZE");
            this.Property(x => x.CbDebtSum, "Взыскания долга - Сумма долга").Column("CB_DEBT_SUM");
            this.Property(x => x.CbPenaltyDebt, "Взыскания долга - Сумма пени").Column("CB_PENALTY_DEBT");
            this.Property(x => x.CbFactInitiated, "Взыскания долга - факт возбуждения дела").Column("CB_FACT_INITIATED").DefaultValue(LawsuitFactInitiationType.NotInitiated).NotNull();
            this.Property(x => x.FactInitiatedNote, "Взыскания долга - Факт возбуждения дела - Примечание").Column("FACT_INITIATED_NOTE").Length(255);
            this.Property(x => x.CbDateInitiated, "Взыскания долга - Дата возбуждения").Column("CB_DATE_INITIATED");
            this.Reference(x => x.CbStationSsp, "Взыскания долга - Участок ССП").Column("CB_SSP_JINST_ID").Fetch();
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
            this.Property(x => x.DutyPostponement, "Отсрочка оплаты госпошлины").Column("DUTY_POSTPONEMENT");
            // новые поля для воронежа
            this.Property(x => x.IsDeterminationReturn, "Определение о возвращении ЗВСП").Column("IS_DETERMINATION_RETURN");
            this.Property(x => x.DateDeterminationReturn, "Дата определения о возвращении ЗВСП").Column("DATE_DETERMINATION_RETURN");
            this.Property(x => x.IsDeterminationRenouncement, "Определение об отказе принятия ЗВСП").Column("IS_DETERMINATION_RENOUNCEMENT");
            this.Property(x => x.DateDeterminationRenouncement, "Дата определения об отказе принятия ЗВСП").Column("DATE_DETERMINATION_RENOUNCEMENT");
            this.Property(x => x.DateJudicalOrder, "Дата вынесения судебного приказа").Column("DATE_JUDICAL_ORDER");
            this.Property(x => x.IsDeterminationCancel, "Определение об отмене судебного приказа").Column("IS_DETERMINATION_CANCEL");
            this.Property(x => x.DateDeterminationCancel, "Дата определения об отмене судебного приказа").Column("DATE_DETERMINATION_CANCEL");
            this.Property(x => x.IsDeterminationOfTurning, "Определение о повороте СП").Column("IS_DETERMINATION_TURN");
            this.Property(x => x.DateDeterminationOfTurning, "Дата определения о повороте СП").Column("DATE_DETERMINATION_TURN");
            this.Property(x => x.FKRAmountCollected, "Взысканная сумма с ФКР").Column("FKR_AMMOUNT_COLLECTED");
            //еще несколько полей для Воронежа
            this.Property(x => x.ComentСonsideration, "Примечание").Column("COMENT_CONSIDERATION");
            this.Property(x => x.PayDocDate, "Дата платежного поручения").Column("PAY_DOC_DATE");
            this.Property(x => x.PayDocNumber, "Номер платежного поручения").Column("PAY_DOC_NUMBER");
            this.Property(x => x.MoneyLess, "Недостаточно денег для оплаты пошлины").Column("MONEY_LESS");

            this.Property(x => x.IsMotionless, "оставлено без движения").Column("IS_MOTIONLESS");
            this.Property(x => x.DateMotionless, "дата оставления без движения").Column("DATE_MOTIONLESS");
            this.Property(x => x.IsErrorFix, "Устранение недостатков").Column("IS_ERROR_FIX");
            this.Property(x => x.DateErrorFix, "Дата устранения недостатков").Column("DATE_ERROR_FIX");
            this.Property(x => x.IsLimitationOfActions, "применен срок исковой давности").Column("IS_LIMITATION_OF_ACTIONS");
            this.Property(x => x.DateLimitationOfActions, "дата применения срока исковой давности").Column("DATE_LIMITATION_OF_ACTIONS");
            this.Property(x => x.IsLawsuitDistanceDecisionCancel, "отмена заочного решения").Column("IS_DISTANCE_DECISION_CANCEL");
            this.Property(x => x.DateLawsuitDistanceDecisionCancel, "дата отмены заочного решения").Column("DATE_DISTANCE_DECISION_CANCEL");
            this.Property(x => x.RedirectDate, "Дата перенаправления").Column("REDIRECT_DATE");
            this.Property(x => x.LawsuitDistanceDecisionCancelComment, "Комментарий отмена заочного решения").Column("DISTANCE_DECISION_CANCEL_COMMENT");

            this.Property(x => x.IsDenied, "Отказано").Column("IS_DENIED");
            this.Property(x => x.DeniedDate, "Дата отказа").Column("DENIED_DATE");
            this.Property(x => x.IsDeniedAdmission, "Отказано в приеме").Column("IS_DENIED_ADMISSION");
            this.Property(x => x.DeniedAdmissionDate, "Дата отказа в приёме").Column("DENIED_ADMISSION_DATE");
            this.Property(x => x.IsDirectedByJuridiction, "Направлено по подсудности").Column("IS_DIRECTED_BY_JURIDICTION");
            this.Property(x => x.DirectedByJuridictionDate, "Дата направления по подсудности").Column("DIRECTED_BY_JURIDICTION_DATE");
            this.Property(x => x.DirectedToDebtor, "Дата направления должнику").Column("DIRECTED_TO_DEBTOR");
            this.Property(x => x.DutyDebtApproved, "Сумма признанной пошлины").Column("DUTY_DEBT_APPROV");

            //Поле для Смоленска
            this.Property(x => x.DutyPayed, "Оплачено Пени").Column("DUTY_PAYED");
            
            this.Property(x => x.DebtStartDate, "Расчетная дата начала задолженности").Column("DEBT_START_DATE");
            this.Property(x => x.DebtEndDate, "Расчетная дата окончания задолженности").Column("DEBT_END_DATE");
            this.Property(x => x.DebtCalcMethod, "Метод формирования эталонных начислений").Column("DEBT_CALC_METHOD").DefaultValue(DebtCalcMethod.NotSet);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
        }
    }
}
