namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.LogImport"</summary>
    public class LogImportMap : BaseEntityMap<LogImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public LogImportMap() : base("Bars.Gkh.Entities.LogImport", "GKH_LOG_IMPORT")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.UploadDate, "Дата загрузки").Column("UPLOAD_DATE");
            this.Property(x => x.FileName, "FileName").Column("FILE_NAME");
            this.Property(x => x.ImportKey, "Ключ импорта").Column("IMPORT_KEY").Length(256);
            this.Property(x => x.CountWarning, "Количество предупреждений").Column("COUNT_WARNING");
            this.Property(x => x.CountError, "Количество ошибок").Column("COUNT_ERROR");
            this.Property(x => x.CountImportedRows, "Количество импортированных строк").Column("COUNT_IMPORTED_ROWS");
            this.Property(x => x.CountChangedRows, "Количество изменнных строк").Column("COUNT_CHANGED_ROWS");
            this.Property(x => x.CountImportedFile, "Количество импортированных файлов").Column("COUNT_IMPORTED_FILE");
            this.Property(x => x.Login, "Логин").Column("LOGIN");
            this.Reference(x => x.Operator, "Оператор").Column("OPERATOR_ID").Fetch();
            this.Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            this.Reference(x => x.LogFile, "LogFile").Column("LOG_FILE_ID").Fetch();
            this.Reference(x => x.Task, "Задача, которая разбирала импорт, на сервере вычислений").Column("TASK_ID").Fetch();
        }
    }
}