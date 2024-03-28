namespace Bars.Gkh.Migrations.Version_2014121700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_INSPECTOR", new Column("EMAIL", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "EMAIL");
        }
    }
}