namespace Bars.Gkh.RegOperator.Imports.FsGorod
{
    using Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Прокси-класс для маппинга импорта платежных агентов
    /// </summary>
    public class FsGorodMapItemProxy
    {
        /// <summary>
        /// Мета-структура
        /// </summary>
        public bool IsMeta { get; set; }

        /// <summary>
        /// Индекс, с которого считается файл
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Регулярное выражение
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// Использовать значение из регулярного выражения
        /// </summary>
        public bool GetValueFromRegex { get; set; }

        /// <summary>
        /// Значение поля при совпадении
        /// </summary>
        public string RegexSuccessValue { get; set; }
        
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        /// Использовать наименование файла
        /// </summary>
        public bool UseFilename { get; set; }

        /// <summary>
        /// Формат преобразования строки
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Платежный агент
        /// </summary>
        public PaymentAgent PaymentAgent { get; set; }

        /// <summary>
        /// Обязательное
        /// </summary>
        public virtual bool Required { get; set; }

        /// <summary>
        /// Преобразование в сущность
        /// </summary>
        /// <param name="importInfo">строка импорта</param>
        /// <returns>маппинг для импорта</returns>
        public FsGorodMapItem AsEntity(FsGorodImportInfo importInfo)
        {
            return new FsGorodMapItem
            {
                ErrorText = this.ErrorText,
                Format = this.Format,
                GetValueFromRegex = this.GetValueFromRegex,
                ImportInfo = importInfo,
                Index = this.Index,
                IsMeta = this.IsMeta,
                PropertyName = this.PropertyName,
                Regex = this.Regex,
                RegexSuccessValue = this.RegexSuccessValue,
                UseFilename = this.UseFilename,
                PaymentAgent = this.PaymentAgent,
                Required= this.Required
            };
        }

        /// <summary>
        /// Преобразование из сущности
        /// </summary>
        /// <param name="entity">маппинг импорта</param>
        /// <returns>прокси класс импорта</returns>
        public static FsGorodMapItemProxy FromEntity(FsGorodMapItem entity)
        {
            return new FsGorodMapItemProxy
            {
                ErrorText = entity.ErrorText,
                Format = entity.Format,
                GetValueFromRegex = entity.GetValueFromRegex,
                Index = entity.Index,
                IsMeta = entity.IsMeta,
                PropertyName = entity.PropertyName,
                Regex = entity.Regex,
                RegexSuccessValue = entity.RegexSuccessValue,
                UseFilename = entity.UseFilename,
                PaymentAgent = entity.PaymentAgent,
                Required = entity.Required
            };
        }
    }
}
