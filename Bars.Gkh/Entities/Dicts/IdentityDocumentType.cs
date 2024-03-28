namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Тип документа удостоверяющего личность
    /// </summary>
    public class IdentityDocumentType : BaseGkhDict
    {
        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Регулярное выражение для проверки корректности введенных значений серии и номера документа
        /// </summary>
        public virtual string Regex { get; set; }

        /// <summary>
        /// Сообщение, выводимое пользователю при несоответствии введенных данных шаблону
        /// </summary>
        public virtual string RegexErrorMessage { get; set; }
    }
}
