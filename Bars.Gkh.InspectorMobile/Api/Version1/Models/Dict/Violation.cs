namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    /// <summary>
    /// Модель записи справочниа "Нарушения"
    /// </summary>
    public class Violation
    {
        /// <summary>
        /// Уникальный идентификатор нарушения
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование нарушения
        /// </summary>
        public string Name { get; set; }
    }
}