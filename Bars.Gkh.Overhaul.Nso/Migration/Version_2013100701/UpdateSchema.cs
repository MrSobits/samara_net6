namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013100701
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013100700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_DPKR_GROUPYEAR",
                new Column("YEAR", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "OVRHL_DPKR_GROUPYEAR_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_2", "FOR_CORRECTION");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", "FOR_CORRECTION");

            Database.AddColumn("OVHL_DPKR_CORRECT_ST2", new Column("IS_OWNER_CALCULATED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddRefColumn(
                               "OVHL_DPKR_CORRECT_ST2",
                new RefColumn("STAGE2_ID", ColumnProperty.NotNull, "OVHL_DPKR_COR_ST2_ST2", "OVRHL_RO_STRUCT_EL_IN_PRG_2", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVHL_DPKR_CORRECT_ST2", "STAGE2_ID");
            Database.RemoveColumn("OVHL_DPKR_CORRECT_ST2", "IS_OWNER_CALCULATED");

            Database.RemoveTable("OVRHL_DPKR_GROUPYEAR");
        }
    }
}