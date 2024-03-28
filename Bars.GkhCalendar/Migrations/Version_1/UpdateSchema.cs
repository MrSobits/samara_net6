namespace Bars.GkhCalendar.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CALENDAR_DAY",
                new Column("DAY_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("DAY_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CALENDAR_DAY");
        }
    }
}