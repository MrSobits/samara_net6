namespace Bars.GkhGji.Migrations.Version_2014021902
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014021901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECK_ROBJECT", new Column("NOT_REVEALED_VIOLATIONS", DbType.String, 1000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK_ROBJECT", "NOT_REVEALED_VIOLATIONS");
        }
    }
}