namespace Bars.Gkh.RegOperator.Map.Import
{
    using System.Collections.Generic;
    
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.RegOperator.Imports.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    using ChesImport = Bars.Gkh.RegOperator.Entities.Import.Ches.ChesImport;

    public class ChesImportMap : BaseEntityMap<ChesImport>
    {
        /// <inheritdoc />
        public ChesImportMap()
            : base("Сущность для хранения данных об импорте ЧЭС", "REGOP_CHES_IMPORT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.State, "Статус загрузки").Column("STATE").NotNull();
            this.Property(x => x.AnalysisState, "Статус разбора").Column("ANALYSIS_STATE").NotNull();
            this.Property(x => x.LoadedFiles, "Загруженные типы файлов").Column("LOADED_FILES");
            this.Property(x => x.ImportedFiles, "Импортированные типы файлов").Column("IMPORTED_FILES");

            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull();
            this.Reference(x => x.Task, "Задача, в рамках которой происходит импорт").Column("TASK_ID");
            this.Reference(x => x.User, "Пользователь").Column("USER_ID");
        }
    }

    public class ChesImportNhMapping : ClassMapping<ChesImport>
    {
        public ChesImportNhMapping()
        {
            this.Property(x => x.LoadedFiles, m => m.Type<ImprovedJsonSerializedType<SortedSet<FileType>>>());
            this.Property(x => x.ImportedFiles, m => m.Type<ImprovedJsonSerializedType<SortedSet<FileType>>>());
        }
    }
}