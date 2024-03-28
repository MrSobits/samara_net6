namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2023082300
{
    using System.Data;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2023082300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022101700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_KPKR_COST_LIMITS", new RefColumn("CAPITALGROUP_ID", "COST_LIMITS_CAPITALGROUP", "GKH_DICT_CAPITAL_GROUP", "ID"));
            Database.AddEntityTable("COST_LIMIT_TYPE_WORK_CR",
               new RefColumn("COST_LIMIT_ID", "COST_LIMIT_TYPE_WORK_CR_COST_LIMIT", "OVRHL_KPKR_COST_LIMITS", "ID"),
               new RefColumn("TYPE_WORK_CR_ID", "COST_LIMIT_TYPE_WORK_CR_TYPE_WORK_CR", "CR_OBJ_TYPE_WORK", "ID"),
               new Column("YEAR", DbType.String, 10),
               new Column("COST", DbType.Decimal),
               new Column("VOLUME", DbType.Decimal),
               new RefColumn("UNIT_MEASURE_ID", "COST_LIMIT_TYPE_WORK_CR_UM", "GKH_DICT_UNITMEASURE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_KPKR_COST_LIMITS", "CAPITALGROUP_ID");
            Database.RemoveTable("COST_LIMIT_TYPE_WORK_CR");
        }
    }
}