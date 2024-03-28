namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Фото-архив жилого дома
    /// </summary>
    public class RealityObjectImage : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual ImagesGroup ImagesGroup { get; set; }

        /// <summary>
        /// Выводить на печать
        /// </summary>
        public virtual bool ToPrint { get; set; }

        /// <summary>
        /// Дата изображения
        /// </summary>
        public virtual DateTime? DateImage { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work WorkCr { get; set; }
    }
}
