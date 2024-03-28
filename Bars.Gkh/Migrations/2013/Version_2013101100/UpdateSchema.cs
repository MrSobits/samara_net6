namespace Bars.Gkh.Migrations.Version_2013101100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013100801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_SUG_RUBRIC",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("EXECUTOR_TYPE", DbType.Int16, ColumnProperty.NotNull));

            Database.AddEntityTable(
                "GKH_CIT_SUG",
                 new Column("SUG_NUM", DbType.String, ColumnProperty.NotNull),
                new Column("CREATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FIO", DbType.String, ColumnProperty.NotNull),
                new Column("ADDRESS", DbType.String, ColumnProperty.NotNull),
                new Column("PHONE", DbType.String, ColumnProperty.NotNull),
                new Column("EMAIL", DbType.String, ColumnProperty.NotNull),
                new Column("PROBLEM_PLACE", DbType.String, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 1000, ColumnProperty.NotNull),
                new RefColumn("RO_ID", ColumnProperty.Null, "CIT_SUG_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("RUBRIC_ID", ColumnProperty.NotNull, "CIT_SUG_RUBRIC", "GKH_SUG_RUBRIC", "ID"));

            Database.AddEntityTable(
                "GKH_CIT_SUG_COMMENT",
                new RefColumn("SUG_ID", ColumnProperty.Null, "CIT_SUG_COMM_SUG", "GKH_CIT_SUG", "ID"),
                new Column("TEXT", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("CREATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("IS_ANSWER", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CIT_SUG_COMMENT");
            Database.RemoveTable("GKH_CIT_SUG");
            Database.RemoveTable("GKH_SUG_RUBRIC");
        }
    }
}