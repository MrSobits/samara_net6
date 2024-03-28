namespace Bars.Gkh.Map.Administration.FormatDataExport
{
    using Bars.B4;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    using NHibernate.Mapping.ByCode.Conformist;

    public class FormatDataExportRemoteResultMap : GkhBaseEntityMap<FormatDataExportRemoteResult>
    {
        /// <inheritdoc />
        public FormatDataExportRemoteResultMap() : base("GKH_FORMAT_DATA_EXPORT_REMOTE_RESULT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.FileId).Column("FILE_ID");
            this.Property(x => x.TaskId).Column("TASK_ID");
            this.Property(x => x.LogId).Column("LOG_ID");
            this.Property(x => x.Status).Column("STATUS");
            this.Property(x => x.UploadResult).Column("UPLOAD_RESULT");
            this.Reference(x => x.TaskResult).Column("TASK_RESULT_ID");
        }

        public class FormatDataExportRemoteResultNhMapping : ClassMapping<FormatDataExportRemoteResult>
        {
            public FormatDataExportRemoteResultNhMapping()
            {
                this.Property(x => x.UploadResult, m =>
                {
                    m.Type<ImprovedBinaryJsonType<BaseDataResult>>();
                });
            }
        }
    }
}