namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Связь операции по счету оплат дома и оплаты акта выполненных работ
    /// </summary>
    public class RealObjPaymentAccOperPerfAct : BaseImportableEntity
    {
        /// <summary>
        /// Операция по счету оплат дома
        /// </summary>
        public virtual RealityObjectPaymentAccountOperation PayAccOperation { get; set; }

        /// <summary>
        /// Оплата в актах выполненных работ
        /// </summary>
        public virtual PerformedWorkActPayment PerformedWorkActPayment { get; set; }
    }
}