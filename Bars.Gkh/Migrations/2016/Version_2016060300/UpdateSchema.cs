namespace Bars.Gkh.Migrations._2016.Version_2016060300
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция '2016.06.03.00'
    /// </summary>
    [Migration("2016060300")]
    [MigrationDependsOn(typeof(Version_2016052000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_RO_PUBRESORG_SERVICE", new Column("SERVICE_PERIOD", DbType.String, 255));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            // не нужна - fix миграция
        }
    }
}
