namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class OkiDocTypeMap : BaseEntityMap<OkiDocType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiDocTypeMap() :
            base("D_OKI_DOC_TYPE")
        {
            //Устанавливаем схему
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_DOC_TYPE_ID");
                m.Generator(Generators.Native);
            });

            this.Map(x => x.OkiDocTypeName, "OKI_DOC_TYPE");
        }
    }
}
