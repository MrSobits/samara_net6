namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.OkiTypeGroup
    /// </summary>
    public class OkiTypeGroupMap : BaseEntityMap<OkiTypeGroup>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiTypeGroupMap() :
            base("D_OKI_TYPE_GROUP")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_TYPE_GROUP_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Name, "OKI_TYPE_GROUP");
        }

    }
}
