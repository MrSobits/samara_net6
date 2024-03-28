namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112301
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Удаляем ненужные таблицы
            Database.RemoveTable("OVRHL_SHORT_PROG_REC");
            Database.RemoveTable("OVHL_DPKR_CORRECT_ST2");

            Database.AddEntityTable(
                "OVRHL_DPKR_CORRECT_ST2",
                new Column("TYPE_RESULT", DbType.Int16, 4, ColumnProperty.NotNull, 10),
                new Column("PLAN_YEAR", DbType.Int16, 4, ColumnProperty.NotNull, 0),
                new RefColumn("REALITYOBJECT_ID", ColumnProperty.NotNull, "OVRHL_DPKR_CORRECT_ST2_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("ST2_VERSION_ID", ColumnProperty.NotNull, "OVRHL_DPKR_CORRECT_ST2_V", "OVRHL_STAGE2_VERSION", "ID"));
        }

        public override void Down()
        {
            // таблицу OVHL_DPKR_CORRECT_ST2 неоткатывем поскольку старая таблица втом виде ненужна
            // И удаленные таблицы невосстанавливаем
        }
    }
}