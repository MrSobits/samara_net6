namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016121600
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016121600
    /// </summary>
    [Migration("2016121600")]
    [MigrationDependsOn(typeof(Version_2016110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "Regop");
            //ViewManager.Create(this.Database, "Regop");
        }
    }
}
