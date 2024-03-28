namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021040801
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021040801")]
    [MigrationDependsOn(typeof(Version_2021040800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_LICENSE_ACTION", new Column("REPLY_TO", DbType.String, 500));
            Database.AddColumn("GJI_LICENSE_ACTION", new Column("RPGU_NUMBER", DbType.String, 50));
            Database.AddColumn("GJI_LICENSE_ACTION", new Column("MESSAGE_ID", DbType.String, 50));
            Database.AddRefColumn("GJI_LICENSE_ACTION", new RefColumn("FILE_ID", ColumnProperty.Null, "GJI_LICENSE_ACTION_F_I", "B4_FILE_INFO", "ID"));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_LICENSE_ACTION", "REPLY_TO");
            Database.RemoveColumn("GJI_LICENSE_ACTION", "MESSAGE_ID");
            Database.RemoveColumn("GJI_LICENSE_ACTION", "RPGU_NUMBER");
            Database.RemoveColumn("GJI_LICENSE_ACTION", "FILE_ID");
        }
    }
}