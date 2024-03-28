namespace Bars.GkhGji.Migrations.Version_2014042200
{
    using System.Data;
    using Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014041800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("DATE_WRITE_OUT", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "DATE_WRITE_OUT");
        }
    }
}