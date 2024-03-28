namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Файлы проверки
    /// </summary>
    public class AuditFilesProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор файла
        /// </summary>
        public long Id => this.File.Id;

        /// <summary>
        /// 1. Уникальный идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор проверки
        /// </summary>
        [ProxyId(typeof(AuditProxy))]
        public long AuditId { get; set; }

        /// <summary>
        /// 3. Тип файла
        /// </summary>
        public int Type { get; set; }
    }
}