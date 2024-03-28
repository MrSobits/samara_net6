namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Опубликованная программа
    /// </summary>
    public class PublishedProgram : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Ссылка на версию
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Подписан ЭЦП
        /// </summary>
        public virtual bool EcpSigned { get; set; }

        /// <summary>
        /// Файл подписи
        /// </summary>
        public virtual FileInfo FileSign { get; set; }

        /// <summary>
        /// Файл xml
        /// </summary>
        public virtual FileInfo FileXml { get; set; }

        /// <summary>
        /// Файл pdf
        /// </summary>
        public virtual FileInfo FilePdf { get; set; }

        /// <summary>
        /// Файл сертификата
        /// </summary>
        public virtual FileInfo FileCertificate { get; set; }

        /// <summary>
        /// Дата подписания
        /// </summary>
        public virtual DateTime? SignDate { get; set; }

        /// <summary>
        /// Дата опубликования
        /// </summary>
        public virtual DateTime? PublishDate { get; set; }
    }
}