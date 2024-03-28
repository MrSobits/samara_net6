namespace Bars.GkhEdoInteg.Migrations.Version_2013090400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhEdoInteg.Migrations.Version_2013082800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("INTGEDO_LOGR", new Column("TIME_EXECUTION", DbType.Decimal));
        }

        public override void Down()
        {
        }
    }
}