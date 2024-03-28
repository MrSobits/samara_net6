namespace Bars.GkhDi.Migrations.Version_2016082600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    /// <summary>
    /// Миграция номер [2016082600]
    /// </summary>
    [Migration("2016082600")]
    [MigrationDependsOn(typeof(Version_2016033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "INN", DbType.String, 50);
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "INN");
        }
    }
}
