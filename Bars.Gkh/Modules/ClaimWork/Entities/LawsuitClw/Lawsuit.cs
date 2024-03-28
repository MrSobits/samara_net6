// ReSharper disable All
namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Gkh.Entities;
    using Enums;

    using NHibernate.Engine;

    /// <summary>
    /// Исковое зявление
    /// </summary>
    public class Lawsuit : DocumentClw
    {
        /// <summary>
        /// Дата окончания работы
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public virtual DateTime? BidDate { get; set; }

        /// <summary>
        /// Расчетная дата начала задолженности
        /// </summary>
        public virtual DateTime? DebtStartDate { get; set; }

        /// <summary>
        /// Расчетная дата начала задолженности
        /// </summary>
        public virtual DateTime? DebtEndDate { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string BidNumber { get; set; }

        /// <summary>
        /// Тип заявления
        /// </summary>
        public virtual PetitionToCourtType PetitionType { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Сумма долга по базовому тарифу
        /// </summary>
        public virtual decimal? DebtBaseTariffSum { get; set; }

        /// <summary>
        /// Сумма долга по тарифу решения
        /// </summary>
        public virtual decimal? DebtDecisionTariffSum { get; set; }

        /// <summary>
        /// Сумма долга
        /// </summary>
        public virtual decimal? DebtSum { get; set; }

        /// <summary>
        /// Сумма пени
        /// </summary>
        public virtual decimal? PenaltyDebt { get; set; }

        /// <summary>
        /// Гос. пошлина
        /// </summary>
        public virtual decimal? Duty { get; set; }

        /// <summary>
        /// Суд издержки
        /// </summary>
        public virtual decimal? Costs { get; set; }

        /// <summary>
        /// Отсрочка оплаты госпошлины
        /// </summary>
        public virtual bool DutyPostponement { get; set; }

        /// <summary>
        /// Кем рассмотрено
        /// </summary>
        public virtual LawsuitConsiderationType WhoConsidered { get; set; }

        /// <summary>
        /// Судебное учреждение
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }

        /// <summary>
        /// Место нахожедния
        /// </summary>
        public virtual Municipality JuridicalSectorMu { get; set; }

        /// <summary>
        /// Дата принятия 
        /// </summary>
        public virtual DateTime? DateOfAdoption { get; set; }

        /// <summary>
        /// Дата рассмотрения  
        /// </summary>
        public virtual DateTime? DateOfRewiew { get; set; }

        /// <summary>
        /// Приостановлено 
        /// </summary>
        public virtual bool Suspended { get; set; }

        /// <summary>
        /// Номер определения 
        /// </summary>
        public virtual string DeterminationNumber { get; set; }

        /// <summary>
        /// Дата определения 
        /// </summary>
        public virtual DateTime? DeterminationDate { get; set; }

        /// <summary>
        /// Результат рассмотрения 
        /// </summary>
        public virtual LawsuitResultConsideration ResultConsideration { get; set; }

        /// <summary>
        /// Тип документа 
        /// </summary>
        public virtual LawsuitDocumentType LawsuitDocType { get; set; }

        /// <summary>
        /// Дата формирования
        /// </summary>
        public virtual DateTime? ConsiderationDate { get; set; }

        /// <summary>
        /// Номер формирования
        /// </summary>
        public virtual string ConsiderationNumber { get; set; }

        /// <summary>
        /// ФИО Судьи
        /// </summary>
        public virtual string JudgeName { get; set; }

        /// <summary>
        /// Номер дела
        /// </summary>
        public virtual string NumberCourtBuisness { get; set; }

        /// <summary>
        /// Сумма признанной задолженности
        /// </summary>
        public virtual decimal? DebtSumApproved { get; set; }

        /// <summary>
        /// Сумма признанной пени
        /// </summary>
        public virtual decimal? PenaltyDebtApproved { get; set; }

        /// <summary>
        /// Сумма признанной пошлины
        /// </summary>
        public virtual decimal? DutyDebtApproved { get; set; }

        // поля Взыскания долга

        /// <summary>
        /// Взыскания долга  - Размер погашения
        /// </summary>
        public virtual LawsuitCollectionDebtType CbSize { get; set; }

        /// <summary>
        /// Взыскания долга  - Сумма долга
        /// </summary>
        public virtual decimal? CbDebtSum { get; set; }

        /// <summary>
        /// Взыскания долга - Сумма пени
        /// </summary>
        public virtual decimal? CbPenaltyDebt { get; set; }

        /// <summary>
        /// Взыскания долга  - факт возбуждения дела
        /// </summary>
        public virtual LawsuitFactInitiationType CbFactInitiated { get; set; }

        /// <summary>
        /// Факт возбуждения дела - Примечание
        /// </summary>
        public virtual string FactInitiatedNote { get; set; }

        /// <summary>
        /// Взыскания долга  - Дата возбуждения
        /// </summary>
        public virtual DateTime? CbDateInitiated { get; set; }

        /// <summary>
        /// Взыскания долга - Участок ССП
        /// </summary>
        public virtual JurInstitution CbStationSsp { get; set; }

        /// <summary>
        /// Взыскания долга - Дата ССП
        /// </summary>
        public virtual DateTime? CbDateSsp { get; set; }

        /// <summary>
        /// Взыскания долга - факт возбуждения дела
        /// </summary>
        public virtual LawsuitCollectionDebtDocumentType CbDocumentType { get; set; }

        /// <summary>
        /// Взыскания долга - сумма для погашения
        /// </summary>
        public virtual decimal? CbSumRepayment { get; set; }

        /// <summary>
        /// Взыскания долга - Дата документа
        /// </summary>
        public virtual DateTime? CbDateDocument { get; set; }

        /// <summary>
        /// Взыскания долга - Номер документа
        /// </summary>
        public virtual string CbNumberDocument { get; set; }

        /// <summary>
        /// Взыскания долга - Файл
        /// </summary>
        public virtual FileInfo CbFile { get; set; }

        /// <summary>
        /// Взыскания долга - сумма, пошаговая в рамках производства
        /// </summary>
        public virtual decimal? CbSumStep { get; set; }

        /// <summary>
        /// Взыскания долга - Производство остановлено
        /// </summary>
        public virtual bool CbIsStopped { get; set; }

        /// <summary>
        /// Взыскания долга - Дата остановки
        /// </summary>
        public virtual DateTime? CbDateStopped { get; set; }

        /// <summary>
        /// Взыскания долга - Причина
        /// </summary>
        public virtual LawsuitCollectionDebtReasonStoppedType CbReasonStoppedType { get; set; }

        /// <summary>
        /// Взыскания долга - Описание причины 
        /// </summary>
        public virtual string CbReasonStoppedDescription { get; set; }

        /// <summary>
        /// Взыскания долга - Дата направления на исполнение в ССП
        /// </summary>
        public virtual DateTime? DateDirectionForSsp { get; set; }
        
        //------------------------------- НОВЫЕ ПОЛЯ ДЛЯ ВОРОНЕЖА------------------------------------------
        /// <summary>
        /// Определение о возвращении заявления о вынесении судебного приказа
        /// </summary>
        public virtual bool IsDeterminationReturn { get; set; }

        /// <summary>
        ///Дата определения о возвращении заявления о вынесении судебного приказа
        /// </summary>
        public virtual DateTime? DateDeterminationReturn { get; set; }

        /// <summary>
        /// Определение об отказе принятья заявления о вынесении судебного приказа
        /// </summary>
        public virtual bool IsDeterminationRenouncement { get; set; }

        /// <summary>
        ///Дата определения об отказе принятья заявления о вынесении судебного приказа
        /// </summary>
        public virtual DateTime? DateDeterminationRenouncement { get; set; }

        /// <summary>
        ///Дата вынесения судебного приказа
        /// </summary>
        public virtual DateTime? DateJudicalOrder { get; set; }

        /// <summary>
        /// Определение об отмене судебного приказа
        /// </summary>
        public virtual bool IsDeterminationCancel { get; set; }

        /// <summary>
        ///Дата определения об отмене судебного приказа
        /// </summary>
        public virtual DateTime? DateDeterminationCancel { get; set; }

        /// <summary>
        /// Определение о повороте судебного приказа
        /// </summary>
        public virtual bool IsDeterminationOfTurning { get; set; }

        /// <summary>
        ///Дата определения об отмене судебного приказа
        /// </summary>
        public virtual DateTime? DateDeterminationOfTurning { get; set; }

        /// <summary>
        /// Взысканная сумма с ФКР
        /// </summary>
        public virtual decimal? FKRAmountCollected { get; set; }
        
        /// <summary>
        /// Метод формирования эталонных начислений
        /// </summary>
        public virtual DebtCalcMethod DebtCalcMethod { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
        
        /// <summary>
        /// Примечание рассмотрения
        /// </summary>
        public virtual string ComentСonsideration { get; set; }


        /// <summary>
        /// Дата платежного поручения
        /// </summary>
        public virtual DateTime PayDocDate { get; set; }

        /// <summary>
        /// Номер платежного поручения
        /// </summary>
        public virtual decimal PayDocNumber { get; set; }

        /// <summary>
        /// оплачено Пени. Для Смоленска
        /// </summary>
        public virtual decimal DutyPayed { get; set; }

        /// <summary>
        /// Недостаточно денег для оплвты гос пошлины
        /// </summary>
        public virtual bool MoneyLess { get; set; }

        /// <summary>
        /// оставлено без движения
        /// </summary>
        public virtual bool IsMotionless { get; set; }

        /// <summary>
        /// дата оставления без движения
        /// </summary>
        public virtual DateTime DateMotionless { get; set; }

        /// <summary>
        /// Устранение недостатков
        /// </summary>
        public virtual bool IsErrorFix { get; set; }

        /// <summary>
        /// дата устранения недостатков
        /// </summary>
        public virtual DateTime DateErrorFix { get; set; }

        /// <summary>
        /// применен срок исковой давности
        /// </summary>
        public virtual bool IsLimitationOfActions { get; set; }

        /// <summary>
        /// дата применения срока исковой давности
        /// </summary>
        public virtual DateTime? DateLimitationOfActions { get; set; }

        /// <summary>
        /// отмена заочного решения
        /// </summary>
        public virtual bool IsLawsuitDistanceDecisionCancel { get; set; }

        /// <summary>
        /// дата отмены заочного решения
        /// </summary>
        public virtual DateTime DateLawsuitDistanceDecisionCancel { get; set; }
        /// <summary>
        /// Коментарий отмены заочного решения
        /// </summary>
        public virtual string LawsuitDistanceDecisionCancelComment { get; set; }

        /// <summary>
        /// дата перенаправления
        /// </summary>
        public virtual DateTime RedirectDate { get; set; }
        /// <summary>
        /// Отказано в приеме
        /// </summary>
        public virtual bool IsDeniedAdmission { get; set; }
        /// <summary>
        /// Дата отказа в приёме
        /// </summary>
        public virtual DateTime DeniedAdmissionDate { get; set; }
        /// <summary>
        /// Отказано
        /// </summary>
        public virtual bool IsDenied { get; set; }
        /// <summary>
        /// Дата отказа
        /// </summary>
        public virtual DateTime DeniedDate { get; set; }
        /// <summary>
        /// Направлено по подсудности
        /// </summary>
        public virtual bool IsDirectedByJuridiction { get; set; }
        /// <summary>
        /// Дата направления по подсудности
        /// </summary>
        public virtual DateTime DirectedByJuridictionDate { get; set; }
        /// <summary>
        /// Дата направления должнику
        /// </summary>
        public virtual DateTime DirectedToDebtor { get; set; }
    }
}