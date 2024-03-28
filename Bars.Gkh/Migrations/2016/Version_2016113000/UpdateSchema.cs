namespace Bars.Gkh.Migrations._2016.Version_2016113000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Миграция 2016113000
    /// </summary>
    [Migration("2016113000")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016110300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("FRGU_REG_NUMBER", DbType.String, 36));
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("FRGU_ORG_NUMBER", DbType.String, 36));
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("FRGU_SERVICE_NUMBER", DbType.String, 36));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONTRAGENT", "FRGU_REG_NUMBER");
            this.Database.RemoveColumn("GKH_CONTRAGENT", "FRGU_ORG_NUMBER");
            this.Database.RemoveColumn("GKH_CONTRAGENT", "FRGU_SERVICE_NUMBER");
        }
    }
}