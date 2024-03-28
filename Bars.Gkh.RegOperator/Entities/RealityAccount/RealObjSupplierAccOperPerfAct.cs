namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using GkhCr.Entities;

    /// <summary>
    /// Связь операции по рассчету с поставщиками и  актом выполненных работ
    /// </summary>
    public class RealObjSupplierAccOperPerfAct : BaseImportableEntity
    {
        /// <summary>
        ///  Операция счета по рассчету с поставщиками
        /// </summary>
        public virtual RealityObjectSupplierAccountOperation SupplierAccOperation { get; set; }

        /// <summary>
        /// Акт выполненных работ
        /// </summary>
        public virtual PerformedWorkAct PerformedWorkAct { get; set; }
    }
}