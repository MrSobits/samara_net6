// ReSharper disable UnusedMember.Global
namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;

    using B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Архивный ПИР
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class FlattenedClaimWork : BaseEntity
    {
        /// <summary>
        /// Номер ЗВСП
        /// </summary>
        public virtual string Num { get; set; }

        /// <summary>
        /// Должник
        /// </summary>
        public virtual string DebtorFullname { get; set; }

        /// <summary>
        /// Адрес МКД
        /// </summary>
        public virtual string DebtorRoomAddress { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public virtual string DebtorRoomType { get; set; }

        /// <summary>
        /// Номер пом.
        /// </summary>
        public virtual string DebtorRoomNumber { get; set; }

        /// <summary>
        /// Период задолженности
        /// </summary>
        public virtual string DebtorDebtPeriod { get; set; }

        /// <summary>
        /// Сумма долга
        /// </summary>
        public virtual decimal? DebtorDebtAmount { get; set; }

        /// <summary>
        /// Размер (руб.)
        /// </summary>
        public virtual decimal? DebtorDutyAmount { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? DebtorDebtPaymentDate { get; set; }

        /// <summary>
        /// № платежного поручения
        /// </summary>
        public virtual string DebtorDutyPaymentAssignment { get; set; }

        /// <summary>
        /// Дата подачи заявления в суд
        /// </summary>
        public virtual DateTime? DebtorClaimDeliveryDate { get; set; }
        
        /// <summary>
        /// Оплаты после ЗВСП
        /// </summary>
        public virtual decimal? DebtorPaymentsAfterCourtOrder { get; set; }
        
        /// <summary>
        /// Тип суда
        /// </summary>
        public virtual string DebtorJurInstType { get; set; }

        /// <summary>
        /// Наименование суда
        /// </summary>
        public virtual string DebtorJurInstName { get; set; }

        /// <summary>
        /// № дела
        /// </summary>
        public virtual string CourtClaimNum { get; set; }

        /// <summary>
        /// Дата вынесения СП
        /// </summary>
        public virtual DateTime? CourtClaimDate { get; set; }

        /// <summary>
        /// Результат рассмотрения:
        /// </summary>
        public virtual string CourtClaimConsiderationResult { get; set; }

        /// <summary>
        /// Определение об отмене СП (дата)
        /// </summary>
        public virtual DateTime? CourtClaimCancellationDate { get; set; }

        /// <summary>
        /// Наименование РОСП
        /// </summary>
        public virtual string CourtClaimRospName { get; set; }

        /// <summary>
        /// Дата направления в РОСП
        /// </summary>
        public virtual DateTime? CourtClaimRospDate { get; set; }

        /// <summary>
        /// № Постановления о возбуждении  ИП
        /// </summary>
        public virtual string CourtClaimEnforcementProcNum { get; set; }

        /// <summary>
        /// Дата (ИП)
        /// </summary>
        public virtual DateTime? CourtClaimEnforcementProcDate { get; set; }

        /// <summary>
        /// № платежного поручения
        /// </summary>
        public virtual string CourtClaimPaymentAssignmentNum { get; set; }

        /// <summary>
        /// Дата (ПП)
        /// </summary>
        public virtual DateTime? CourtClaimPaymentAssignmentDate { get; set; }

        /// <summary>
        /// Сумма основного долга
        /// </summary>
        public virtual decimal? CourtClaimRospDebtExact { get; set; }

        /// <summary>
        /// Размер пошлины	
        /// </summary>
        public virtual decimal? CourtClaimRospDutyExact { get; set; }

        /// <summary>
        /// № Постановления об окончании ИП
        /// </summary>
        public virtual string CourtClaimEnforcementProcActEndNum { get; set; }

        /// <summary>
        /// Определение о повороте исполнения СП (дата)
        /// </summary>
        public virtual DateTime? CourtClaimDeterminationTurnDate { get; set; }

        /// <summary>
        /// Наименование РОСП
        /// </summary>
        public virtual string FkrRospName { get; set; }

        /// <summary>
        /// № Постановления о возбуждении  ИП
        /// </summary>
        public virtual string FkrEnforcementProcDecisionNum { get; set; }

        /// <summary>
        /// Дата (ИП)
        /// </summary>
        public virtual DateTime? FkrEnforcementProcDate { get; set; }

        /// <summary>
        /// № платежного поручения
        /// </summary>
        public virtual string FkrPaymentAssignementNum { get; set; }

        /// <summary>
        /// Дата (ПП)
        /// </summary>
        public virtual DateTime? FkrPaymentAssignmentDate { get; set; }

        /// <summary>
        /// Сумма основного долга
        /// </summary>
        public virtual decimal? FkrDebtExact { get; set; }

        /// <summary>
        /// Размер пошлины	
        /// </summary>
        public virtual decimal? FkrDutyExact { get; set; }

        /// <summary>
        /// № Постановления об окончании ИП
        /// </summary>
        public virtual string FkrEnforcementProcActEndNum { get; set; }

        /// <summary>
        /// Дата подачи иска в суд
        /// </summary>
        public virtual DateTime? LawsuitCourtDeliveryDate { get; set; }

        /// <summary>
        /// № дела
        /// </summary>
        public virtual string LawsuitDocNum { get; set; }

        /// <summary>
        /// Дата рассмотрения
        /// </summary>
        public virtual DateTime? LawsuitConsiderationDate { get; set; }

        ///// <summary>
        ///// Результат рассмотрения:
        ///// </summary>
        //public virtual string LawsuitConsiderationResult { get; set; }

        /// <summary>
        /// Сумма основного долга
        /// </summary>
        public virtual decimal? LawsuitDebtExact { get; set; }

        /// <summary>
        /// Размер пошлины
        /// </summary>
        public virtual decimal? LawsuitDutyExact { get; set; }

        /// <summary>
        /// Серия №
        /// </summary>
        public virtual string ListListNum { get; set; }

        /// <summary>
        /// Дата направления в РОСП									
        /// </summary>
        public virtual DateTime? ListListRopsDate { get; set; }

        /// <summary>
        /// Наименование РОСП
        /// </summary>
        public virtual string ListRospName { get; set; }

        /// <summary>
        /// Дата направления в РОСП
        /// </summary>
        public virtual DateTime? ListRospDate { get; set; }

        /// <summary>
        /// № Постановления о возбуждении  ИП
        /// </summary>
        public virtual string ListEnfProcDecisionNum { get; set; }

        /// <summary>
        /// Дата (ИП)
        /// </summary>
        public virtual DateTime? ListEnfProcDate { get; set; }

        /// <summary>
        /// № платежного поручения
        /// </summary>
        public virtual string ListPaymentAssignmentNum { get; set; }

        /// <summary>
        /// Дата (ПП)
        /// </summary>
        public virtual DateTime? ListPaymentAssignmentDate { get; set; }

        /// <summary>
        /// Сумма основного долга
        /// </summary>
        public virtual decimal? ListRospDebtExacted { get; set; }

        /// <summary>
        /// Размер пошлины
        /// </summary>
        public virtual decimal? ListRospDutyExacted { get; set; }

        /// <summary>
        /// № Постановления об окончании ИП
        /// </summary>
        public virtual string ListEnfProcActEndNum { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Regop_lawsuit_owner_info
        /// </summary>
        public virtual long? RloiId { get; set; }

        /// <summary>
        /// Архивная запись
        /// </summary>
        public virtual bool Archived { get; set; }

        /// <summary>
        /// Архивная запись
        /// </summary>
        public virtual ZVSPCourtDecision ZVSPCourtDecision { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Share { get; set; }

        /// <summary>
        /// Сумма пени
        /// </summary>
        public virtual decimal? PenaltyAmmount { get; set; }

        /// <summary>
        /// Сумма пени
        /// </summary>
        public virtual decimal? ZVSPPenaltyAmmount { get; set; }

        /// <summary>
        /// Определение об оставлении ИЗ без движения
        /// </summary>
        public virtual bool LawsuitDeterminationMotionless { get; set; }

        /// <summary>
        /// Дата определения об оставлении ИЗ без движения
        /// </summary>
        public virtual DateTime? LawsuitDeterminationMotionlessDate { get; set; }

        /// <summary>
        /// Определение о возврате ИЗ
        /// </summary>
        public virtual bool LawsuitDeterminationReturn { get; set; }

        /// <summary>
        /// Дата определения о возврате ИЗ
        /// </summary>
        public virtual DateTime? LawsuitDeterminationReturnDate { get; set; }

        /// <summary>
        /// Определение об отказе в приеме ИЗ
        /// </summary>
        public virtual bool LawsuitDeterminationDenail { get; set; }

        /// <summary>
        /// Дата определения об отказе в приеме ИЗ
        /// </summary>
        public virtual DateTime? LawsuitDeterminationDenailDate { get; set; }

        /// <summary>
        /// Определение о направлении ИЗ по подсудности
        /// </summary>
        public virtual bool LawsuitDeterminationJurDirected { get; set; }

        /// <summary>
        /// Дата определения о направлении ИЗ по подсудности
        /// </summary>
        public virtual DateTime? LawsuitDeterminationJurDirectedDate { get; set; }
        /// <summary>
        /// тип документа в решении иска
        /// </summary>
        public virtual LawsuitDocumentType LawsuitDocType { get; set; }
        /// <summary>
        /// Дата устранения недостатков иска
        /// </summary>
        public virtual DateTime LawsuitDeterminationMotionFix { get; set; }

        /// <summary>
        /// Отмена заочного решения
        /// </summary>
        public virtual bool LawsuitDistanceDecisionCancel { get; set; }
        /// <summary>
        /// Рассрочка
        /// </summary>
        public virtual string InstallmentPlan { get; set; }
        /// <summary>
        /// сумма пени
        /// </summary>
        public virtual decimal DebtorPenaltyAmount { get; set; }
        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual LawsuitResultConsideration LawsuitResultConsideration { get; set; } 
    }
}