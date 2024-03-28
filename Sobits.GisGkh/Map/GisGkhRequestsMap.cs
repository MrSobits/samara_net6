namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhRequests"</summary>
    public class GisGkhRequestsMap : BaseEntityMap<GisGkhRequests>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhRequestsMap()
            : base("Sobits.GisGkh.Entities", GisGkhRequestsMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_REQUESTS";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.MessageGUID, "GUID сообщения ГИС ЖКХ").Column(nameof(GisGkhRequests.MessageGUID).ToLower());
            this.Property(x => x.RequesterMessageGUID, "GUID сообщения отправителя").Column(nameof(GisGkhRequests.RequesterMessageGUID).ToLower());
            this.Property(x => x.TypeRequest, "Вид запроса").Column(nameof(GisGkhRequests.TypeRequest).ToLower()).NotNull();
            this.Property(x => x.RequestState, "Текущее состояние запроса").Column(nameof(GisGkhRequests.RequestState).ToLower()).NotNull();
            this.Property(x => x.Answer, "Ответ от сервера").Column(nameof(GisGkhRequests.Answer).ToLower());
            this.Reference(x => x.Operator, "Инициатор запроса").Column(nameof(GisGkhRequests.Operator).ToLower());
            this.Reference(x => x.LogFile, "Лог файл").Column(nameof(GisGkhRequests.LogFile).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class GisGkhRequestsNhMapping : ClassMapping<GisGkhRequests>
    {
        public GisGkhRequestsNhMapping()
        {
            this.Schema(GisGkhRequestsMap.SchemaName);
        }
    }
}