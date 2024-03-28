namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    using Castle.Windsor;

    /// <summary>
    /// Для переопределения в других модулях
    /// </summary>
    public interface IPartialExportableEntity
    {
        /// <summary>
        /// IoC
        /// </summary>
        IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить часть данных
        /// </summary>
        /// <param name="baseParams">Параметы для фильтрации данных</param>
        IEnumerable<ExportablePartRow> GetPartialEntityData(DynamicDictionary baseParams);
    }

    /// <summary>
    /// Для переопределения в других модулях
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public interface IPartialExportableEntity<out T> : IPartialExportableEntity
        where T : IExportableEntity
    {
        /// <summary>
        /// Базовая сущность
        /// </summary>
        T Entity { get; }
    }
}