namespace Bars.Gkh.Migrations._2016.Version_2016121400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016121400
    /// </summary>
    [Migration("2016121400")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016113000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DATE_LIC_REG", DbType.DateTime));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DATE_LIC_DEL", DbType.DateTime));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("REG_REASON", DbType.String));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DEL_REASON", DbType.String));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DATE_LIC_REG");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DATE_LIC_DEL");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "REG_REASON");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DEL_REASON");
        }
    }
}