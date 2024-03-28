namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014021000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014020400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("OVRHL_TYPE_WORK_CR_ST1",
               new RefColumn("ST1_ID", "OV_TW_CR_ST1_VERS", "OVRHL_STAGE1_VERSION", "ID"),
               new RefColumn("TYPE_WORK_CR_ID", ColumnProperty.NotNull, "OV_TW_CR_ST1_TW", "CR_OBJ_TYPE_WORK", "ID"),
               new RefColumn("UNIT_MEASURE_ID", ColumnProperty.NotNull, "OV_TW_CR_ST1_UM", "GKH_DICT_UNITMEASURE", "ID"),
               new Column("CALCULATE_BY", DbType.Int32, ColumnProperty.NotNull, 0),
               new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0),
               new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_TYPE_WORK_CR_ST1");
        }
    }
}