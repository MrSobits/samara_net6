namespace Bars.Gkh.Migrations._2016.Version_2016102300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016102300
    /// </summary>
    [Migration("2016102300")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016092700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("PROVIDER_CODE", DbType.String));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONTRAGENT", "PROVIDER_CODE");
        }
    }
}