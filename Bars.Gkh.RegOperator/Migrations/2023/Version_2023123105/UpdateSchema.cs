namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023123105
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123105")]

    [MigrationDependsOn(typeof(Version_2023123104.UpdateSchema))]
    // Является Version_2020091100 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "Regop");
            //ViewManager.Create(this.Database, "Regop");
        }
        public override void Down()
        {
        }
    }
}
