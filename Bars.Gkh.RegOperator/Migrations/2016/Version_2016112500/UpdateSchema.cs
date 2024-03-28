namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016112500
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016112500
    /// </summary>
    [Migration("2016112500")]
    [MigrationDependsOn(typeof(Version_2016111500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "Regop");
            //ViewManager.Create(this.Database, "Regop");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
