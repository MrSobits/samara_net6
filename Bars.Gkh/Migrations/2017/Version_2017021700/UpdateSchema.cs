namespace Bars.Gkh.Migrations._2017.Version_2017021700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция Gkh.2017021700
    /// </summary>
    [Migration("2017021700")]
    [MigrationDependsOn(typeof(Version_2017021100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GKH_MORG_CONTRACT", "SET_PAYMENTS_FOUNDATION", true);
            this.Database.ChangeDefaultValue("GKH_MORG_CONTRACT", "SET_PAYMENTS_FOUNDATION", null);
            this.Database.ExecuteNonQuery("UPDATE GKH_MORG_CONTRACT SET SET_PAYMENTS_FOUNDATION = NULL WHERE SET_PAYMENTS_FOUNDATION = 0");
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.ChangeDefaultValue("GKH_MORG_CONTRACT", "SET_PAYMENTS_FOUNDATION", 0);
            this.Database.ExecuteNonQuery("UPDATE GKH_MORG_CONTRACT SET SET_PAYMENTS_FOUNDATION = 0 WHERE SET_PAYMENTS_FOUNDATION = NULL");
            this.Database.AlterColumnSetNullable("GKH_MORG_CONTRACT", "SET_PAYMENTS_FOUNDATION", false);
        }
    }
}