namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021111500
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021111500")]
    [MigrationDependsOn(typeof(Version_2021081000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_TASK_CALENDAR",
                new Column("DAY_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("DAY_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));
            Database.AddUniqueConstraint("UNQ_GJI_TASK_CALENDAR_DAY", "GJI_TASK_CALENDAR", "DAY_DATE");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TASK_CALENDAR");
        }

    }
}