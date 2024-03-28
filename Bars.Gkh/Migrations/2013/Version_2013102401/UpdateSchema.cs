namespace Bars.Gkh.Migrations.Version_2013102401
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_CONST_ELEMENT", new Column("VOLUME", DbType.Decimal));
            Database.AddColumn("GKH_DICT_CONST_ELEMENT", new Column("COST_REPAIR", DbType.Decimal));
            Database.AddColumn("GKH_DICT_METERING_DEVICE", new Column("REPLACEMENT_COST", DbType.Decimal));
            Database.AddColumn("GKH_DICT_METERING_DEVICE", new Column("LIFETIME", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_CONST_ELEMENT", "VOLUME");
            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "COST_REPAIR");
            Database.RemoveColumn("GKH_DICT_METERING_DEVICE", "REPLACEMENT_COST");
            Database.RemoveColumn("GKH_DICT_METERING_DEVICE", "LIFETIME");
        }
    }
}