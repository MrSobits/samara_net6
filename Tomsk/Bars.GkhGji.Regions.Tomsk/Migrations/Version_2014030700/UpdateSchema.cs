namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014030700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOL_PROS_DEFINITION", new Column("EXECUTION_TIME", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOL_PROS_DEFINITION", "EXECUTION_TIME");
        }
    }
}