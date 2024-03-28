namespace Bars.Gkh.Migrations._2016.Version_2016092600
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016092600
    /// </summary>
    [Migration("2016092600")]
    [MigrationDependsOn(typeof(Version_2016092200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_INVOLVED_CR_TO_2", DbType.Boolean));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_INVOLVED_CR_TO_2");
        }
    }
}
