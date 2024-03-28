namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020042300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020042300")]
    [MigrationDependsOn(typeof(Version_2020041700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ProtocolTableName = "GJI_TATARSTAN_PROTOCOL_GJI";
        private const string ResolutionTableName = "GJI_TATARSTAN_RESOLUTION_GJI";
        private const string ColumnName = "IDENTITY_DOCUMENT_ID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.ProtocolTableName,
                new RefColumn(UpdateSchema.ColumnName, "GJI_TATARSTAN_PROTOCOL_IDENTITY_DOCUMENT",
                    "GKH_DICT_IDENTITY_DOC_TYPE", "ID"));

            this.Database.AddColumn(UpdateSchema.ResolutionTableName,
                new RefColumn(UpdateSchema.ColumnName, "GJI_TATARSTAN_RESOLUTION_IDENTITY_DOCUMENT",
                    "GKH_DICT_IDENTITY_DOC_TYPE", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.ProtocolTableName, UpdateSchema.ColumnName);
            this.Database.RemoveColumn(UpdateSchema.ResolutionTableName, UpdateSchema.ColumnName);
        }
    }
}
