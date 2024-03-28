namespace Bars.GkhGji.Migrations.Version_2014040700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014031700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECK", new Column("ACT_TO_PRES", DbType.Boolean));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK", "ACT_TO_PRES");
        }
    }
}