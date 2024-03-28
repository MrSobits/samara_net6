namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    ///     Пункт нормативного документа
    /// </summary>
    public class NormativeDocItem : BaseImportableEntity
    {
        /// <summary>
        ///     Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        ///     Текст (описание)
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        ///     Нормативный документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }
    }
}