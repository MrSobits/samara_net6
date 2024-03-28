namespace Bars.Gkh.Map.Administration.FormatDataExport
{
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    using NHibernate.Mapping.ByCode.Conformist;

    public class FormatDataExportResultMap : GkhBaseEntityMap<FormatDataExportResult>
    {
        /// <inheritdoc />
        public FormatDataExportResultMap() : base("GKH_FORMAT_DATA_EXPORT_RESULT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.StartDate).Column("START_DATE");
            this.Property(x => x.EndDate).Column("END_DATE");
            this.Property(x => x.Progress).Column("PROGRESS");
            this.Property(x => x.Status).Column("STATUS");

            this.Property(x => x.EntityCodeList).Column("ENTITY_CODE_LIST");

            this.Reference(x => x.LogOperation).Column("LOG_OPERATION_ID");
            this.Reference(x => x.Task).Column("TASK_ID");
        }

        public class FormatDataExportResultMapNHibernateMapping : ClassMapping<FormatDataExportResult>
        {
            public FormatDataExportResultMapNHibernateMapping()
            {
                this.Property(x => x.EntityCodeList, m =>
                {
                    m.Type<ImprovedBinaryJsonType<List<string>>>();
                });
            }
        }
    }
}