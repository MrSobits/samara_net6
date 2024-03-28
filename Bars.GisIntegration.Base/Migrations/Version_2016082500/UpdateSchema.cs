namespace Bars.GisIntegration.Base.Migrations.Version_2016082500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GisIntegration.Base.Enums;

    [Migration("2016082500")]
    [MigrationDependsOn(typeof(Version_2016080200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GI_ATTACHMENT", new Column("SRC_FILE_INFO_ID", DbType.Int64, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("GI_ATTACHMENT", new Column("FILE_STORAGE_NAME", DbType.Int16, ColumnProperty.NotNull, default(FileStorageName)));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GI_ATTACHMENT", "FILE_STORAGE_NAME");
            this.Database.RemoveColumn("GI_ATTACHMENT", "SRC_FILE_INFO_ID");
        }
    }
}