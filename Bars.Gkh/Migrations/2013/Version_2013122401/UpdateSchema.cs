namespace Bars.Gkh.Migrations.Version_2013122401
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("OKTMO", DbType.Int64));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "OKTMO");
        }
    }
}