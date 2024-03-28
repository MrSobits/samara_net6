namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.Ches;

    /// <summary>
    /// Сущность для хранения данных об импорте ЧЭС
    /// </summary>
    public class ChesImport : BaseEntity
    {
        private SortedSet<FileType> loadedFiles;
        private SortedSet<FileType> importedFiles;

        /// <summary>
        /// .ctor
        /// </summary>
        public ChesImport()
        {
            this.loadedFiles = new SortedSet<FileType>();
            this.importedFiles = new SortedSet<FileType>();

            this.AnalysisState = ChesAnalysisState.LoadingCorrection;
            this.State = ChesImportState.Loading;
        }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Задача, в рамках которой происходит импорт
        /// </summary>
        public virtual TaskEntry Task { get; set; }

        /// <summary>
        /// Статус загрузки
        /// </summary>
        public virtual ChesImportState State { get; set; }

        /// <summary>
        /// Пользователь.
        /// <para>Пользователь, который последним вносил изменения в загруженные файлы.</para>
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Статус разбора
        /// </summary>
        public virtual ChesAnalysisState AnalysisState { get; set; }

        /// <summary>
        /// Загруженные типы файлов
        /// </summary>
        public virtual SortedSet<FileType> LoadedFiles
        {
            get
            {
                return this.loadedFiles ?? (this.loadedFiles = new SortedSet<FileType>()); 
                
            }
            set
            {
                this.loadedFiles = value;
                
            }
        }

        /// <summary>
        /// Импортированные типы файлов
        /// </summary>
        public virtual SortedSet<FileType> ImportedFiles
        {
            get
            {
                return this.importedFiles ?? (this.importedFiles = new SortedSet<FileType>());
                
            }
            set
            {
                this.importedFiles = value;
                
            }
        }
    }
}