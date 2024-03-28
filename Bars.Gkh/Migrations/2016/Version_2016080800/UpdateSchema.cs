namespace Bars.Gkh.Migrations._2016.Version_2016080800
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016080800")]
    [MigrationDependsOn(typeof(Version_2016062200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("CADASTRAL_HOUSE_NUMBER", DbType.String));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "CADASTRAL_HOUSE_NUMBER");
        }
    }
}