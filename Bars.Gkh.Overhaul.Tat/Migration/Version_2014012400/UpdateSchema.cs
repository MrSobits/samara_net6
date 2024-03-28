namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014012400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014012100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_SHORT_PROG_DEFECT",
                new RefColumn("SHORT_OBJ_ID", ColumnProperty.NotNull, "OV_SHORT_PROG_DEFECT_OBJ", "OVRHL_SHORT_PROG_OBJ", "ID"),
                new RefColumn("STATE_ID", "OV_SHORT_PROG_DEFECT_ST", "B4_STATE", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OV_SHORT_PROG_DEFECT_W", "GKH_DICT_WORK", "ID"),
                new RefColumn("FILE_ID", "OV_SHORT_PROG_DEFECT_F", "B4_FILE_INFO", "ID"),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddEntityTable(
                "OVRHL_SHORT_PROG_PROTOCOL",
                new RefColumn("SHORT_OBJ_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_PROT_OBJ", "OVRHL_SHORT_PROG_OBJ", "ID"),
                new RefColumn("CONTRAGENT_ID", "OVRHL_SHORT_PROG_PROT_CTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("FILE_ID", "OVRHL_SHORT_PROG_PROT_F", "B4_FILE_INFO", "ID"),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("COUNT_ACCEPT", DbType.Decimal),
                new Column("COUNT_VOTE", DbType.Decimal),
                new Column("COUNT_VOTE_GENERAL", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 2000),
                new Column("DATE_FROM", DbType.Date),
                new Column("GRADE_OCCUPANT", DbType.Int32),
                new Column("GRADE_CLIENT", DbType.Int32),
                new Column("SUM_ACT_VER_OF_COSTS", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SHORT_PROG_DEFECT");
            Database.RemoveTable("OVRHL_SHORT_PROG_PROTOCOL");
        }
    }
}