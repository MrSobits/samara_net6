namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// статья закона ГЖИ
    /// </summary>
    public class ArticleLawGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Часть
        /// </summary>
        public virtual string Part { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        public virtual string Article { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public virtual string KBK { get; set; }

        /// <summary>
        /// Код ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhCode { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
    }
}