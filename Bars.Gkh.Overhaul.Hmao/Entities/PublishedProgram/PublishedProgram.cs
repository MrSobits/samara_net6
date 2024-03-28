﻿namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Опубликованная программа
    /// </summary>
    public class PublishedProgram : BaseImportableEntity, IStatefulEntity
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

        /// <summary>
        /// Количество домов в программе
        /// </summary>
        public virtual int? TotalRoCount { get; set; }

        /// <summary>
        /// Количество домов включеных в программу
        /// </summary>
        public virtual int? IncludedRoCount { get; set; }

        /// <summary>
        /// Количество домов исключенных из программы
        /// </summary>
        public virtual int? ExcludedRoCount { get; set; }
    }
}