namespace Bars.Gkh.Migrations._2016.Version_2016121501
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016121501
    /// </summary>
    [Migration("2016121501")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016121400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumnNotNullable("GKH_RO_PUB_SERVORG", "REAL_OBJ_ID", false);
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}