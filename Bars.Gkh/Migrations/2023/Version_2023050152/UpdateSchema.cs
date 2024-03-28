namespace Bars.Gkh.Migrations._2023.Version_2023050152
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050152")]

    [MigrationDependsOn(typeof(Version_2023050151.UpdateSchema))]

    /// Является Version_2022111900 из ядра
    public class UpdateSchema : Migration
    {
        private const string TableName = "EMAIL_MESSAGE";

        public override void Up()
        {
            this.Database.AddEntityTable(TableName,
                new Column("EMAIL_MESSAGE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("EMAIL_ADDRESS", DbType.String),
                new Column("ADDITIONAL_INFO", DbType.String),
                new Column("SENDING_TIME", DbType.DateTime),
                new Column("SENDING_STATUS", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("RECIPIENT_CONTRAGENT_ID", "RECIPIENT_CONTRAGENT_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                new RefColumn("LOG_FILE_ID", "EMAIL_MESSAGE_FILE_LOG", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable(TableName);
        }
    }
}