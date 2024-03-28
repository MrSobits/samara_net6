namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class TatarstanProtocolGjiArticleLaw : BaseEntity
    {
        /// <summary>
        /// Протокол ГЖИ РТ
        /// </summary>
        public virtual TatarstanProtocolGji TatarstanProtocolGji { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }
    }
}
