namespace Bars.GisIntegration.Base.Migrations.Version_2016080200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016080200")]
    [MigrationDependsOn(typeof(Version_2016072700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GI_PACKAGE", "SIGNED", DbType.Boolean, ColumnProperty.NotNull, (object)false);
            this.Database.AddColumn("GI_PACKAGE", "PACKAGE_DATA_ID", DbType.Int64, ColumnProperty.NotNull, (object)0);

            //this.Database.RemoveColumn("GI_PACKAGE", "NOT_SIGNED_DATA");
            //this.Database.RemoveColumn("GI_PACKAGE", "SIGNED_DATA");
            //this.Database.RemoveColumn("GI_PACKAGE", "TRANSPORT_GUID_DICTIONARY");

            this.Database.AddPersistentObjectTable(
                "GI_PACKAGE_DATA",
                new RefColumn("DATA_ID", ColumnProperty.Null, "GI_PAC_DAT_DAT", "B4_FILE_INFO", "ID"),
                new RefColumn("SIG_DATA_ID", ColumnProperty.Null, "GI_PAC_DAT_SIGDAT", "B4_FILE_INFO", "ID"),
                new Column("TRANSPORT_GUID_DICTIONARY", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GI_PACKAGE_DATA");

            this.Database.RemoveColumn("GI_PACKAGE", "SIGNED");
            this.Database.RemoveColumn("GI_PACKAGE", "PACKAGE_DATA_ID");
        }
    }
}