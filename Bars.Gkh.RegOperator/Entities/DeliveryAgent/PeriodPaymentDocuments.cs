namespace Bars.Gkh.RegOperator.Entities
{
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Документы платежного агента за период по насПункту_улице_типЛС
    /// </summary>
    public class PeriodPaymentDocuments : BaseImportableEntity
    {
        /// <summary>
        /// Период начисления
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Ссылка на файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Код документа насПункту_улице_типЛС
        /// </summary>
        public virtual string DocumentCode { get; set; }
    }
}