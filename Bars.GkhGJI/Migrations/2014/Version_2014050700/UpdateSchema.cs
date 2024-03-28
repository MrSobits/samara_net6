namespace Bars.GkhGji.Migrations.Version_2014050700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014043000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_DEFINITION", new Column("DEF_TIME", DbType.DateTime,25));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_DEFINITION", "DEF_TIME");
        }
    }
}