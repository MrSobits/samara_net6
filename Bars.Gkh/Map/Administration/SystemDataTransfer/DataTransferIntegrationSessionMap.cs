namespace Bars.Gkh.Map.SystemDataTransfer
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities.Administration.SystemDataTransfer;

    using NHibernate.Mapping.ByCode.Conformist;

    public class DataTransferIntegrationSessionMap : GkhBaseEntityMap<DataTransferIntegrationSession>
    {
        /// <inheritdoc />
        public DataTransferIntegrationSessionMap()
            : base("GKH_DATA_INTEGRATION_SESSION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.DateStart).Column("DATE_START").NotNull();
            this.Property(x => x.DateEnd).Column("DATE_END");
            this.Property(x => x.Description).Column("DESCRIPTION");
            this.Property(x => x.TypeIntegration).Column("TYPE");
            this.Property(x => x.Guid).Column("GUID").NotNull();
            this.Property(x => x.Success).Column("SUCCESS");
            this.Property(x => x.TransferingState).Column("STATE");
            this.Property(x => x.ErrorCode).Column("ERROR_CODE");
            this.Property(x => x.ErrorMessage).Column("ERROR_MESSAGE");
            this.Property(x => x.TypesNames).Column("TYPE_NAMES");
        }
    }

    public class DataTransferIntegrationSessionNhMapping : ClassMapping<DataTransferIntegrationSession>
    {
        public DataTransferIntegrationSessionNhMapping()
        {
            this.Property(x => x.TypesNames, m =>
            {
                m.Type<ImprovedJsonSerializedType<IDictionary<string, bool?>>>();
            });
        }
    }
}