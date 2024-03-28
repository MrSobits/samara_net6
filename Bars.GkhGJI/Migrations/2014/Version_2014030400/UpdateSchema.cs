namespace Bars.GkhGji.Migrations.Version_2014030400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014022500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("DATE_OF_PROCEEDINGS", DbType.Date));
            Database.AddColumn("GJI_PROTOCOL", new Column("HOUR_OF_PROCEEDINGS", DbType.Int32));
            Database.AddColumn("GJI_PROTOCOL", new Column("MINUTE_OF_PROCEEDINGS", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "DATE_OF_PROCEEDINGS");
            Database.RemoveColumn("GJI_PROTOCOL", "HOUR_OF_PROCEEDINGS");
            Database.RemoveColumn("GJI_PROTOCOL", "MINUTE_OF_PROCEEDINGS");
        }
    }
}