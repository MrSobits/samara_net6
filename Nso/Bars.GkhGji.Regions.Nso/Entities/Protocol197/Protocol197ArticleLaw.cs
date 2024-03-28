namespace Bars.GkhGji.Regions.Nso.Entities
{
	using Bars.Gkh.Entities;
	using Bars.GkhGji.Entities;

	/// <summary>
    /// Статьи закона в протоколе ГЖИ 19.7
    /// </summary>
	public class Protocol197ArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// Протокол 19.7
        /// </summary>
        public virtual Protocol197 Protocol197 { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}