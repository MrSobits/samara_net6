namespace Bars.Gkh.Migrations.Version_2014112400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014112000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("GKH_TRANSITION",
                    new Column("EMAIL_SUBJECT", DbType.String, 100, ColumnProperty.NotNull),
                    new Column("EMAIL_TEMPLATE", DbType.String, 3000, ColumnProperty.NotNull),
                    new Column("EXEC_DEADLINE", DbType.Int32, ColumnProperty.NotNull),
                    new Column("IS_FIRST", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("NAME", DbType.String, 200, ColumnProperty.NotNull),
                    new RefColumn("INITIAL_RUBRIC", ColumnProperty.Null, "TRANSIT_INIT_RUBR", "GKH_SUG_RUBRIC", "ID"),
                    new RefColumn("TARGET_RUBRIC", ColumnProperty.Null, "TRANSIT_TARGET_RUBR", "GKH_SUG_RUBRIC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_TRANSITION");
        }
    }
}