namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018040700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    using Bars.Gkh.Utils;

    [Migration("2018040700")]
    [MigrationDependsOn(typeof(Version_2017071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_DPKR_ACTUAL_CRITERIAS",
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull),
                new Column("STATUSES", DbType.String),
                new Column("TYPE_HOUSE", DbType.Int32),
                new Column("CONDITION_HOUSE", DbType.Int32),
                new Column("IS_NUMBER_APARTMENTS", DbType.Boolean, ColumnProperty.NotNull),
                new Column("NUMBER_APARTMENTS_CONDITION", DbType.Int32),
                new Column("NUMBER_APARTMENTS", DbType.Int32),
                new Column("IS_YEAR_REPAIR", DbType.Boolean, ColumnProperty.NotNull),
                new Column("YEAR_REPAIR_CONDITION", DbType.Int32),
                new Column("YEAR_REPAIR", DbType.Int16),
                new Column("CHECK_REPAIR_ADVISABLE", DbType.Boolean, ColumnProperty.NotNull),
                new Column("CHECK_INVOLVED_CR", DbType.Boolean, ColumnProperty.NotNull),
                new Column("IS_STRUCT_EL_COUNT", DbType.Boolean, ColumnProperty.NotNull),
                new Column("STRUCT_EL_COUNT_CONDITION", DbType.Int32),
                new Column("STRUCT_EL_COUNT", DbType.Int32),
                new RefColumn("ORERATOR_ID", ColumnProperty.NotNull, "OVRHL_DPKR_ACTUAL_CRITERIAS_ORERATOR_ID_GKH_OPERATOR_ID", "GKH_OPERATOR", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.Null, "OVRHL_DPKR_ACTUAL_CRITERIAS_STATE_ID_B4_STATE_ID", "B4_STATE", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_DPKR_ACTUAL_CRITERIAS");
        }
    }
}