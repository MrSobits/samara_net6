namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013090501
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013090500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "OVRHL_RO_STRUCT_EL_IN_PRG_3",
               new RefColumn("RO_ID", ColumnProperty.NotNull, "PRG_ST3_RO", "GKH_REALITY_OBJECT", "ID"),
               new Column("YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
               new Column("CEO_STRING", DbType.String, 4000, ColumnProperty.Null),
               new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddEntityTable(
                "OVRHL_RO_STRUCT_EL_IN_PRG_2",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "PRG_ST2_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("CEO_ID", ColumnProperty.NotNull, "PRG_ST2_CEO", "OVRHL_COMMON_ESTATE_OBJECT", "ID"),
                new RefColumn("STAGE3_ID", ColumnProperty.Null, "PRG_ST2_ST3", "OVRHL_RO_STRUCT_EL_IN_PRG_3", "ID"),
                new Column("YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("SE_STRING", DbType.String, 4000, ColumnProperty.Null),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG", new RefColumn("STAGE2_ID", ColumnProperty.Null, "PRG_ST2", "OVRHL_RO_STRUCT_EL_IN_PRG_2", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG", "STAGE2_ID");

            Database.RemoveTable("OVRHL_RO_STRUCT_EL_IN_PRG_2");
            Database.RemoveTable("OVRHL_RO_STRUCT_EL_IN_PRG_3");
        }
    }
}