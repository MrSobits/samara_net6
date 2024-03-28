namespace Bars.Gkh.Migrations.Version_2014021000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("LOCALITY", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "LOCALITY");
        }
    }
}