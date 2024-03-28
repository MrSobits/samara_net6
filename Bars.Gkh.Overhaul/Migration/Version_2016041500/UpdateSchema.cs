namespace Bars.Gkh.Overhaul.Migration.Version_2016041500
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016041500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2015020300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("OVRHL_DICT_WORK_PRICE", new RefColumn("SETTLEMENT_ID","MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_DICT_WORK_PRICE", "SETTLEMENT_ID");
        }
    }
}