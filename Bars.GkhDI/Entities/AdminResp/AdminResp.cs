namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Административная ответственность
    /// </summary>
    public class AdminResp : BaseDocument
    {
        /// <summary>
        /// Контролирующий орган
        /// </summary>
        public virtual SupervisoryOrg SupervisoryOrg { get; set; }

        /// <summary>
        /// Количество выявленных нарушений
        /// </summary>
        public virtual int? AmountViolation { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public virtual decimal? SumPenalty { get; set; }

        /// <summary>
        /// Дата оплаты штрафа
        /// </summary>
        public virtual DateTime? DatePaymentPenalty { get; set; }

        /// <summary>
        /// Дата наложения штрафа
        /// </summary>
        public virtual DateTime? DateImpositionPenalty { get; set; }

        /// <summary>
        /// Вид нарушения
        /// </summary>
        public virtual string TypeViolation { get; set; }

        /// <summary>
        /// Меры
        /// </summary>
        public virtual string Actions { get; set; }

         /// <summary>
        /// Деятельность управляющей организации в периоде раскрытия информации
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        #region Постановление_988
        /// <summary>
        /// Деятельность управляющей организации в периоде раскрытия информации
        /// </summary>
        public virtual TypePersonAdminResp TypePerson { get; set; }

        /// <summary>
        /// ФИО должностного лица
        /// </summary>
        public virtual string Fio { get; set; }
        
        /// <summary>
        /// Должность физ лица 
        /// </summary>
        public virtual string Position { get; set; }
        #endregion
    }
}
