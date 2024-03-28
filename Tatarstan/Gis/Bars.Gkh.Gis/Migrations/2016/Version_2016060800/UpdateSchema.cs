namespace Bars.Gkh.Gis.Migrations._2016.Version_2016060800
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция для конечного выпиливания модуля Gkh.Billing
    /// </summary>
    [Migration("2016060800")]
    [MigrationDependsOn(typeof(Version_2016042200.UpdateSchema))]
    public class UUpdateSchema : Migration
    {
        public override void Up()
        {
            if (this.Database.TableExists("BIL_ADDRESS_MATCH"))
            {
                this.Database.RemoveTable("BIL_ADDRESS_MATCH");
            }
        }

        public override void Down()
        {
            // ignored
        }
    }
}
