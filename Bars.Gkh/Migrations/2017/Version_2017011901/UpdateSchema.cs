namespace Bars.Gkh.Migrations._2017.Version_2017011901
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017011901
    /// </summary>
    [Migration("2017011901")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017011900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_LOG_IMPORT", "LOGIN", DbType.String);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_LOG_IMPORT", "LOGIN");
        }
    }
}
