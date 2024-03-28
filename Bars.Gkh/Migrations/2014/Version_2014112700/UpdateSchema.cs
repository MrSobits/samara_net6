namespace Bars.Gkh.Migrations.Version_2014112700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("GKH_SUGG_HISTORY",
                    new Column("RECORD_DATE", DbType.DateTime),
                    new Column("EXEC_DEADLINE", DbType.DateTime),
                    new Column("TARGET_EXEC_TYPE", DbType.Int32, ColumnProperty.Null),
                    new Column("EXEC_EMAIL", DbType.String, 200, ColumnProperty.Null),

                    new RefColumn("SUGG_ID", ColumnProperty.Null, "HIST_SUGG", "GKH_CIT_SUG", "ID"),
                    new RefColumn("MANORG_ID", ColumnProperty.Null, "HISTORY_EXEC_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                    new RefColumn("MUNICIPALITY_ID", ColumnProperty.Null, "HISTORY_EXEC_MANORG", "GKH_DICT_MUNICIPALITY", "ID"),
                    new RefColumn("INSPECTION_ID", ColumnProperty.Null, "HISTORY_EXEC_INSPECT", "GKH_DICT_ZONAINSP", "ID"));

            Database.RemoveColumn("GKH_TRANSITION", "INITIAL_RUBRIC");
            Database.RemoveColumn("GKH_TRANSITION", "TARGET_RUBRIC");
            Database.AddColumn("GKH_TRANSITION", new Column("INIT_EXEC_TYPE", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddColumn("GKH_TRANSITION", new Column("TARGET_EXEC_TYPE", DbType.Int32, ColumnProperty.Null));
            Database.AddColumn("GKH_TRANSITION", new Column("EXEC_EMAIL", DbType.String, 200, ColumnProperty.Null));
            Database.AddRefColumn("GKH_TRANSITION", new RefColumn("RUBRIC_ID", ColumnProperty.Null, "TRANSIT_RUBR", "GKH_SUG_RUBRIC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_TRANSITION", "RUBRIC_ID");
            Database.RemoveColumn("GKH_TRANSITION", "TARGET_EXEC_TYPE");
            Database.RemoveColumn("GKH_TRANSITION", "INIT_EXEC_TYPE");
            // Не стал добавлять RefColumns TARGET_RUBRIC, INITIAL_RUBRIC - так в этом нет необходимости
            Database.RemoveTable("GKH_SUGG_HISTORY");
        }
    }
}