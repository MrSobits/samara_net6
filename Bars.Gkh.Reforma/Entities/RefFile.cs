namespace Bars.Gkh.Reforma.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Reforma.Entities.Dict;

    /// <summary>
    /// Связь: Файл в нашей системе - Файл в Реформе ЖКХ
    /// </summary>
    public class RefFile : PersistentObject
    {
        /// <summary>
        /// Описание файла
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Идентификатор файла в реформе
        /// </summary>
        public virtual int ExternalId { get; set; }

        /// <summary>
        /// Период синхронизации
        /// </summary>
        public virtual ReportingPeriodDict ReportingPeriod { get; set; }
    }
}