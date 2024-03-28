namespace Bars.Gkh.FormatDataExport.ExportableEntities.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Строковые данные части сущности для экспорта
    /// </summary>
    public class ExportablePartRow : ExportableRow
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id">Идентификатор экспортируемой сущности</param>
        /// <param name="partData">Словарь ячеек с данными</param>
        public ExportablePartRow(long id, Dictionary<int, string> partData)
        {
            this.Id = id;
            this.Cells = partData;
        }

        /// <summary>
        /// Оператор преобразования списка ячеек в объект <see cref="ExportablePartRow"/>
        /// </summary>
        /// <param name="partData">Словарь ячеек с данными</param>
        public static implicit operator ExportablePartRow(Dictionary<int, string> partData)
        {
            return new ExportablePartRow(long.Parse(partData[0]), partData);
        }
    }
}
