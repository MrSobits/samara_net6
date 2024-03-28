namespace Bars.GkhDi.Migrations.Version_2013110700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013092500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_BASE_SERVICE", new Column("SCHEDULE_PREVENT_JOB", DbType.Int16, ColumnProperty.NotNull, 30));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_BASE_SERVICE", "SCHEDULE_PREVENT_JOB");
        }
    }
}