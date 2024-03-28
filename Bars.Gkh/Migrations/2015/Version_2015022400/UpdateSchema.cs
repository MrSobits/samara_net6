namespace Bars.Gkh.Migration.Version_2015022400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015021701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_CLEANING", DbType.Decimal, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_CLEANING");
        }
    }
}