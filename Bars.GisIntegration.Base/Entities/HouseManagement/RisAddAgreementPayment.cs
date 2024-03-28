namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;
    using Bars.B4.DataAccess;
    
    /// <summary>
    /// Сведения о внесении платы и задолженности по такой плате
    /// </summary>
    public class RisAddAgreementPayment : BaseEntity
    {
        /// <summary>
        /// Идентификатор версии сведений о внесении платы в ГИС ЖКХ
        /// </summary>
        public virtual Guid AgreementPaymentVersion { get; set; }

        /// <summary>
        /// Период с
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// Период до
        /// </summary>
        public virtual DateTime DateTo { get; set; }

        /// <summary>
        /// Начислено за период
        /// </summary>
        public virtual decimal Bill { get; set; }

        /// <summary>
        /// Размер задолженности (-)/переплаты (+) за период
        /// </summary>
        public virtual decimal Debt { get; set; }

        /// <summary>
        /// Оплачено за период
        /// </summary>
        public virtual decimal Paid { get; set; }
    }
}
