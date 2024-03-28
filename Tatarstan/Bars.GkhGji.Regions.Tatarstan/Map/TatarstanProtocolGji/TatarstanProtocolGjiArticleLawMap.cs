namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiArticleLawMap : BaseEntityMap<TatarstanProtocolGjiArticleLaw>
    {
        /// <inheritdoc />
        public TatarstanProtocolGjiArticleLawMap()
            : base(typeof(TatarstanProtocolGjiArticleLaw).FullName, "GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LAW")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.TatarstanProtocolGji, "TatarstanProtocolGji").Column("TATARSTAN_PROTOCOL_GJI_ID").Fetch();
            this.Reference(x => x.ArticleLaw, "ArticleLaw").Column("ARTICLE_LAW_ID").Fetch();
        }
    }
}
