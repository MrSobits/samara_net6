namespace Bars.Gkh.Repair.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "RP_DICT_PROGRAM",
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "PR_PROG_PER", "GKH_DICT_PERIOD", "ID"),
                new Column("TYPE_VISIBILITY", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("NAME", DbType.String, 300),
                new Column("TYPE_PROGRAM_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            Database.AddEntityTable(
                "RP_DICT_PROGRAM_MU",
                new RefColumn("PROGRAM_ID", ColumnProperty.NotNull, "PR_PROGMU_PR", "RP_DICT_PROGRAM", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "PR_PROGMU_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("RP_DICT_PROGRAM", "PERIOD_ID");
            Database.RemoveColumn("RP_DICT_PROGRAM_MU", "PROGRAM_ID");
            Database.RemoveColumn("RP_DICT_PROGRAM_MU", "MUNICIPALITY_ID");

            Database.RemoveTable("RP_DICT_PROGRAM");
            Database.RemoveTable("RP_DICT_PROGRAM_MU");
        }
    }
}