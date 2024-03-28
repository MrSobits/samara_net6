namespace Bars.Gkh.Regions.Msk.Migration.Version_2015012100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Msk.Migration.Version_2015011300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("MSK_DPKR_INFO", new Column("CEO_STATES", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("MSK_DPKR_INFO", "CEO_STATES");
        }
    }
}