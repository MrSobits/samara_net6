namespace Bars.Gkh.Migrations._2017.Version_2017030200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция Gkh.2017030200
    /// </summary>
    [Migration("2017030200")]
    [MigrationDependsOn(typeof(Version_2017022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GKH_MORG_CONTRACT", "PAYMENT_SERV_DATE", true);
            this.Database.ChangeDefaultValue("GKH_MORG_CONTRACT", "PAYMENT_SERV_DATE", null);
            this.Database.ExecuteNonQuery("UPDATE GKH_MORG_CONTRACT SET PAYMENT_SERV_DATE = NULL WHERE PAYMENT_SERV_DATE = 0");
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.ExecuteNonQuery("UPDATE GKH_MORG_CONTRACT SET PAYMENT_SERV_DATE = 0 WHERE PAYMENT_SERV_DATE IS NULL");
            this.Database.ChangeDefaultValue("GKH_MORG_CONTRACT", "PAYMENT_SERV_DATE", 0);
            this.Database.AlterColumnSetNullable("GKH_MORG_CONTRACT", "PAYMENT_SERV_DATE", false);
        }
    }
}