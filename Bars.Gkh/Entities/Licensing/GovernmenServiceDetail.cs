namespace Bars.Gkh.Entities.Licensing
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Показатель Формы 1-ГУ
    /// </summary>
    public class GovernmenServiceDetail : BaseImportableEntity
    {
        /// <summary>
        /// Тип показателя
        /// </summary>
        public virtual GovernmenServiceDetailGroup DetailGroup { get; set; }

        /// <summary>
        /// Форма 1-ГУ
        /// </summary>
        public virtual FormGovernmentService FormGovernmentService { get; set; }

        /// <summary>
        /// Значение показателя
        /// </summary>
        public virtual decimal? Value { get; set; }
    }
}