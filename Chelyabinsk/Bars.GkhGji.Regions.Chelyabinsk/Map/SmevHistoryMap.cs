namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4;
    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    public class SmevHistoryMap : BaseEntityMap<SmevHistory>
    {
        /// <inheritdoc />
        public SmevHistoryMap() : 
                base("Сущность для работы с сервисом истории запросов", "GJI_SMEV_HISTORY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.RequestId, "Идентификатор запроса лицензии").Column("REQUEST_ID").NotNull();
            this.Property(x => x.ActionCode, "Тип Action").Column("ACTION_CODE").NotNull();
            this.Property(x => x.LicenseRequestType, "Тип запроса лицензии").Column("REQUEST_TYPE").NotNull();
            this.Property(x => x.Status, "Статус").Column("STATUS").NotNull();
            this.Property(x => x.Guid, "Guid").Column("GUID");
            this.Property(x => x.UniqId, "UniqId").Column("UNIQ_ID").NotNull();
            this.Property(x => x.InnerId, "InnerId").Column("INNER_ID");
            this.Property(x => x.ExtActionId, "ExtActionId").Column("EXT_ACTION_ID");
            this.Property(x => x.SocId, "SocId").Column("SOC_ID");
        }

        public class AppealCitsTransferResultMapNHibernateMapping : ClassMapping<AppealCitsTransferResult>
        {
            public AppealCitsTransferResultMapNHibernateMapping()
            {
                this.Property(x => x.ExportParams, m =>
                {
                    m.Type<BinaryJsonType<BaseParams>>();
                });
            }
        }
    }
}
