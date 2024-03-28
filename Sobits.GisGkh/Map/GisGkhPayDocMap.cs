namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhPayDoc"</summary>
    public class GisGkhPayDocMap : BaseEntityMap<GisGkhPayDoc>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhPayDocMap()
            : base("Sobits.GisGkh.Entities", GisGkhPayDocMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_PAY_DOC";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Account, "Лицевой счёт").Column("ACCOUNT").NotNull().Fetch();
            this.Reference(x => x.Period, "Период").Column("PERIOD").NotNull().Fetch();
            this.Property(x => x.PaymentDocumentID, "Идентификатор платёжного документа").Column("PAY_DOC_ID");
            this.Property(x => x.PaymentDocumentTransportGUID, "Транспортный идентификатор платёжного документа").Column("PAY_DOC_TRANSPORT_GUID");
            this.Property(x => x.GisGkhGuid, "GUID платёжного документа").Column("GIS_GKH_GUID");
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class GisGkhPayDocNhMapping : ClassMapping<GisGkhPayDoc>
    {
        public GisGkhPayDocNhMapping()
        {
            this.Schema(GisGkhPayDocMap.SchemaName);
        }
    }
}