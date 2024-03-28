namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using System.Collections.Generic;

    /// <summary>
    /// Модель записи справочника "Группа нарушений"
    /// </summary>
    public class GroupViolations
    {
        /// <summary>
        /// Уникальный идентификатор группы нарушений
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Наименование группы нарушений
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        public IEnumerable<Violation> Violations { get; set; }
    }
}