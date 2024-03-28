namespace Bars.Gkh.Regions.Voronezh.Entities
{
    using System;

    using B4.Modules.FileStorage;
    using Gkh.Entities;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Долевое ИП
    /// </summary>
    public class LawSuitDebtWorkSSP : BaseGkhEntity
    {
        /// <summary>
        /// Дата окончания работы
        /// </summary>
        public virtual Lawsuit Lawsuit { get; set; }

        // поля Взыскания долга  

        /// <summary>
        /// Дольщик
        /// </summary>
        public virtual LawsuitOwnerInfo LawsuitOwnerInfo { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? CbDebtDecisionSum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? DutyPayedForcibly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? PenaltyPayedForcibly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? TRPayedForcibly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? BtPayedForcibly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? DutyPayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? PenaltyPayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? TRPayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? BtPayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal? CbDuty { get; set; }

        /// <summary>
        /// признак колличества заявлений
        /// </summary>
        public virtual LawSuitDebtWorkType DebtWorkType { get; set; }
        
        /// <summary>
        /// возвращен ли документ
        /// </summary>
        public virtual bool? CbDocReturned { get; set; }
        /// <summary>
        /// Акт об отсутстввии имущества получен
        /// </summary>
        public virtual bool? LackOfPropertyAct { get; set; }
        /// <summary>
        /// Дата акта об отсутствии имущества
        /// </summary>
        public virtual DateTime? LackOfPropertyActDate { get; set; }

    }
}