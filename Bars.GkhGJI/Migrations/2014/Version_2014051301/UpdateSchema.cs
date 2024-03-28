namespace Bars.GkhGji.Migrations.Version_2014051301
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014051200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_DEFINITION", new Column("DATE_PROC", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_DEFINITION", "DATE_PROC");
        }
    }
}