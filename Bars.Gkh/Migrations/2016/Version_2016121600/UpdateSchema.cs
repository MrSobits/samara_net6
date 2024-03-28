namespace Bars.Gkh.Migrations._2016.Version_2016121600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016121600
    /// </summary>
    [Migration("2016121600")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016061500.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016080500.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016090300.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016092600.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016101400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_OPERATOR", "RIS_TOKEN", DbType.String);
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_OPERATOR", "RIS_TOKEN");
        }
    }
}