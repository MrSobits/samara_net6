namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    public class ChesImportFileMap : BaseEntityMap<ChesImportFile>
    {
        /// <inheritdoc />
        public ChesImportFileMap()
            : base("Сущность для хранения данных об импорте файла ЧЭС", "REGOP_CHES_IMPORT_FILE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ChesImport, "Сущность для хранения данных об импорте ЧЭС").Column("CHES_IMPORT_ID").NotNull();
            this.Reference(x => x.File, "Файл лога").Column("FILE_ID");
            this.Property(x => x.FileType, "Тип файла").Column("FILE_TYPE").NotNull();
            this.Property(x => x.CheckState, "Состояние проверки").Column("CHECK_STATE").NotNull();
            this.Property(x => x.IsImported, "Файл импортирован").Column("IS_IMPORTED").NotNull();
        }
    }
}