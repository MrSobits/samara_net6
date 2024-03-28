namespace Bars.GkhGji.Migrations._2024.Version_2024030110
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030110")]
    [MigrationDependsOn(typeof(Version_2024030109.UpdateSchema))]
    /// Является Version_2019090300 из ядра
    public class UpdateSchema : Migration
    {
        private const string RisTaskTableName = "GI_TASK";
        private const string DisposalTableName = "GJI_DISPOSAL";
        private const string FileInfoTableName = "B4_FILE_INFO";

        private const string DisposalColumnName = "DISPOSAL_ID";
        private const string RequestFileInfoColumnName = "REQUEST_XML_FILE_ID";
        private const string ResponseFileInfoColumnName = "RESPONSE_XML_FILE_ID";

        private const string DisposalConstraintName = "GKH_RIS_TASK_DISPOSAL";
        private const string RequestFileInfoConstraintName = "GKH_RIS_TASK_REQUEST_XML";
        private const string ResponseFileInfoConstraintName = "GKH_RIS_TASK_RESPONSE_XML";

        /// <inheritdoc />
        public override void Up()
        {
            if (!this.Database.TableExists(UpdateSchema.RisTaskTableName))
            {
                return;
            }

            this.Database.AddColumn(UpdateSchema.RisTaskTableName, new RefColumn(UpdateSchema.DisposalColumnName,
                ColumnProperty.Null, UpdateSchema.DisposalConstraintName, UpdateSchema.DisposalTableName, "id"));
            this.Database.AddColumn(UpdateSchema.RisTaskTableName, new RefColumn(UpdateSchema.RequestFileInfoColumnName,
                ColumnProperty.Null, UpdateSchema.RequestFileInfoConstraintName, UpdateSchema.FileInfoTableName, "id"));
            this.Database.AddColumn(UpdateSchema.RisTaskTableName, new RefColumn(UpdateSchema.ResponseFileInfoColumnName,
                ColumnProperty.Null, UpdateSchema.ResponseFileInfoConstraintName, UpdateSchema.FileInfoTableName, "id"));

            this.Database.AddForeignKey(UpdateSchema.DisposalConstraintName, UpdateSchema.RisTaskTableName, UpdateSchema.DisposalColumnName, UpdateSchema.DisposalTableName, "id");
            this.Database.AddForeignKey(UpdateSchema.RequestFileInfoConstraintName, UpdateSchema.RisTaskTableName, UpdateSchema.RequestFileInfoColumnName, UpdateSchema.FileInfoTableName, "id");
            this.Database.AddForeignKey(UpdateSchema.ResponseFileInfoConstraintName, UpdateSchema.RisTaskTableName, UpdateSchema.ResponseFileInfoColumnName, UpdateSchema.FileInfoTableName, "id");
        }

        /// <inheritdoc />
        public override void Down()
        {
            if (!this.Database.TableExists(UpdateSchema.RisTaskTableName))
            {
                return;
            }

            this.Database.RemoveConstraint(UpdateSchema.RisTaskTableName, UpdateSchema.DisposalConstraintName);
            this.Database.RemoveConstraint(UpdateSchema.RisTaskTableName, UpdateSchema.RequestFileInfoConstraintName);
            this.Database.RemoveConstraint(UpdateSchema.RisTaskTableName, UpdateSchema.ResponseFileInfoConstraintName);
            this.Database.RemoveColumn(UpdateSchema.RisTaskTableName, UpdateSchema.DisposalColumnName);
            this.Database.RemoveColumn(UpdateSchema.RisTaskTableName, UpdateSchema.RequestFileInfoColumnName);
            this.Database.RemoveColumn(UpdateSchema.RisTaskTableName, UpdateSchema.ResponseFileInfoColumnName);
        }
    }
}