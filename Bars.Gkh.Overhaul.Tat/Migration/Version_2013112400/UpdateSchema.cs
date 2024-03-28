namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_SHORT_PROG_OBJ",
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_OBJ_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_OBJ_V", "OVRHL_PRG_VERSION", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.Null, "OVRHL_SHORT_PROG_OBJ_ST", "B4_STATE", "ID"));

            Database.AddEntityTable("OVRHL_SHORT_PROG_REC",
                new RefColumn("SHORT_PROG_OBJ_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_REC_RO", "OVRHL_SHORT_PROG_OBJ", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_REC_W", "GKH_DICT_WORK", "ID"),
                new RefColumn("STAGE2_ID", "OVRHL_SHORT_PROG_REC_ST2", "OVRHL_STAGE2_VERSION", "ID"),
                new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("COST", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SHORT_PROG_REC");
            Database.RemoveTable("OVRHL_SHORT_PROG_OBJ");
        }
    }
}