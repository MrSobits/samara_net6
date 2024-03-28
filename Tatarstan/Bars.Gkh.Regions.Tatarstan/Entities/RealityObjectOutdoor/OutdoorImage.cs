namespace Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    /// <summary>
    /// Фото-архив двора
    /// </summary>
    public class OutdoorImage: BaseEntity
    {
        /// <summary>
        /// Двор
        /// </summary>
        public virtual RealityObjectOutdoor Outdoor { get; set; }

        /// <summary>
        /// Дата изображения
        /// </summary>
        public virtual DateTime? DateImage { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual ImagesGroup ImagesGroup { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual WorkRealityObjectOutdoor WorkCr { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
