namespace Bars.Gkh.Entities.Licensing
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Licensing;

    /// <summary>
    /// Форма 1-ГУ
    /// </summary>
    public class FormGovernmentService : BaseImportableEntity
    {
        /// <summary>
        /// Государственная услуга
        /// </summary>
        public virtual FormGovernmentServiceType GovernmentServiceType { get;set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Квартал
        /// </summary>
        public virtual Quarter Quarter { get; set; }
    }
}