namespace Bars.GkhCr.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Gkh.Entities;
    using System;

    /// <summary>
    /// Стройконтроль выполнения работы
    /// </summary>
    public class BuildControlTypeWorkSmrFile : BaseGkhEntity
    {
        /// <summary>
        /// Отчет СК
        /// </summary>
        public virtual BuildControlTypeWorkSmr BuildControlTypeWorkSmr { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Ссылка на видео
        /// </summary>
        public virtual string VideoLink { get; set; }
    }
}
