namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021110500
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021110500")]
    [MigrationDependsOn(typeof(Version_2021102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName infoTable = new SchemaQualifiedObjectName
        {
            Schema = "PUBLIC",
            Name = "FSSP_UPLOAD_DOWNLOAD_INFO"
        };

        private readonly SchemaQualifiedObjectName litigationTable = new SchemaQualifiedObjectName
        {
            Schema = "PUBLIC",
            Name = "FSSP_LITIGATION"
        };

        private readonly SchemaQualifiedObjectName fsspAddressMatchTable = new SchemaQualifiedObjectName
        {
            Schema = "PUBLIC",
            Name = "FSSP_ADDRESS_MATCH"
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(
                this.infoTable.Name,
                new RefColumn("DOWNLOAD_FILE_ID", ColumnProperty.NotNull, "FK_FSSP_UPLOAD_DOWNLOAD_INFO_DF", "B4_FILE_INFO", "ID"),
                new Column("DATE_DOWNLOAD_FILE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("USER_ID", ColumnProperty.NotNull, "FK_FSSP_UPLOAD_DOWNLOAD_INFO_USER", "B4_USER", "ID"),
                new Column("STATUS", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("LOG_FILE_ID", "FK_FSSP_UPLOAD_DOWNLOAD_INFO_LOGFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("UPLOAD_FILE_ID", "FK_FSSP_UPLOAD_DOWNLOAD_INFO_UF", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable(this.fsspAddressMatchTable.Name,
                new RefColumn("FSSP_UPLOAD_DOWNLOAD_INFO_ID", ColumnProperty.NotNull, "FK_FSSP_PADDRESS_MATCH_UPLOAD_DOWNLOAD_INFO", this.infoTable.Name, "ID"),
                new RefColumn("FSSP_ADDRESS_ID", ColumnProperty.NotNull, "FK_FSSP_PADDRESS_MATCH_FSSP_ADDRESS", "FSSP_ADDRESS", "ID"));

            this.Database.AddColumn(this.litigationTable, "OUTER_ID", DbType.Int64);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.fsspAddressMatchTable);
            this.Database.RemoveTable(this.infoTable);
            this.Database.RemoveColumn(this.litigationTable, "OUTER_ID");
        }
    }
}