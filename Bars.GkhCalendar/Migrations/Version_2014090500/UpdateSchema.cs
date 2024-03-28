namespace Bars.GkhCalendar.Migrations.Version_2014090500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCalendar.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Так как дни были не уникальны, удаляем дубли, оставляя день с найибольшим ID
            Database.ExecuteNonQuery(@"
delete from gkh_calendar_day 
where id in (
    select id from gkh_calendar_day 
    where day_date in (
        select day_date from gkh_calendar_day
        group by day_date having count(*) > 1) and
    id not in (
        select max(id) from gkh_calendar_day
        group by day_date having count(*) > 1))");

            Database.AddUniqueConstraint("UNQ_CALENDAR_DAY", "GKH_CALENDAR_DAY", "DAY_DATE");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_CALENDAR_DAY", "UNQ_CALENDAR_DAY");
        }
    }
}