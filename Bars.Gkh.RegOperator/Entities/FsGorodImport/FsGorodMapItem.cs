namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг для импорта платежных агентов 
    /// </summary>
    public class FsGorodMapItem : BaseImportableEntity
    {
        /// <summary>
        /// Информация по импорту
        /// </summary>
        public virtual FsGorodImportInfo ImportInfo { get; set; }

        /// <summary>
        /// Свойство 
        /// </summary>
        public virtual bool IsMeta { get; set; }

        public virtual int Index { get; set; }

        public virtual string PropertyName { get; set; }

        public virtual string Regex { get; set; }

        public virtual bool GetValueFromRegex { get; set; }

        public virtual string RegexSuccessValue { get; set; }

        public virtual string ErrorText { get; set; }

        public virtual bool UseFilename { get; set; }

        public virtual string Format { get; set; }

        public virtual bool Required { get; set; }

        /// <summary>
        /// Код платежного агента
        /// </summary>
        public virtual PaymentAgent PaymentAgent { get; set; }
    }
}