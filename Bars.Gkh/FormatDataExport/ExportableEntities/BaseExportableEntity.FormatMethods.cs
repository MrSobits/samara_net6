namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System.Runtime.CompilerServices;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.FormatProvider.Converter;

    /// <summary>
    /// Базовый класс не хранимой экспортируемой сущности
    /// </summary>
    /// <remarks>
    /// Методы приведения данных к требуемому формату
    /// </remarks>
    public abstract partial class BaseExportableEntity : ExportFormatConverter
    {
        /// <summary>
        /// Прокси-класс вложений договоров
        /// </summary>
        protected class FileProxy
        {
            /// <summary>
            /// Вложение
            /// </summary>
            public FileInfo File { get; set; }

            /// <summary>
            /// Идентификатор договора
            /// </summary>
            public long ContractId { get; set; }


            /// <summary>
            /// Тип вложения
            /// </summary>
            public int Type { get; set; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long? GetId(IHaveId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.Id : (long?)null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long? GetId(IHaveExportId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.ExportId : (long?)null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long Id(IHaveId entity)
        {
            return entity.Id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long Id(IHaveExportId entity)
        {
            return entity.ExportId;
        }
    }
}