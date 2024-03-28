namespace Bars.Gkh.FormatDataExport.ExportableEntities.Model
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataModels;

    /// <summary>
    /// Строковые данные сущности для экспорта
    /// </summary>
    public class ExportableRow : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; protected set; }

        /// <inheritdoc />
        public Dictionary<int, string> Cells { get; protected set; }

        /// <inheritdoc />
        public int CellCount => this.Cells.Count;

        /// <inheritdoc />
        public void Merge(ExportableRow partialData)
        {
            foreach (var value in partialData.Cells)
            {
                if (value.Key != 0 && this.Cells.ContainsKey(value.Key))
                {
                    this.Cells[value.Key] = value.Value;
                }
            }
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public ExportableRow()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public ExportableRow(long id, IList<string> stringCollection)
        {
            this.Id = id;
            var i = 0;

            this.Cells = new Dictionary<int, string>(stringCollection.Count);

            foreach (var s in stringCollection)
            {
                this.Cells.Add(i++, s);
            }
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public ExportableRow(IHaveId entity, IList<string> stringCollection)
            : this(entity.Id, stringCollection)
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public ExportableRow(IHaveExportId entity, IList<string> stringCollection)
            :this(entity.ExportId, stringCollection)
        {
        }

        /// <summary>
        /// Оператор преобразования списка ячеек в объект <see cref="ExportableRow"/>
        /// </summary>
        /// <param name="cellList">Список ячеек</param>
        public static implicit operator ExportableRow(List<string> cellList)
        {
            var i = 0;
            return new ExportableRow
            {
                Id = long.Parse(cellList[0]),
                Cells = cellList.ToDictionary(x => i++)
            };
        }
    }
}