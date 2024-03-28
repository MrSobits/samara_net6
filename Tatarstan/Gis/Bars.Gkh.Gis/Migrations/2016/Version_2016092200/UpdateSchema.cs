namespace Bars.Gkh.Gis.Migrations._2016.Version_2016092200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016092200")]
    [MigrationDependsOn(typeof(Version_2016082500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CONTRACT_OWNERS_WORK_SERVICE",
               new RefColumn("CONTRACT_ID", "CONTR_OWN_WORK_SERV_CONTR_ID", "GKH_MORG_CONTRACT_OWNERS", "ID"),
               new RefColumn("WORK_SERVICE_ID", "CONTR_OWN_WORK_SERV_SERV_ID", "MANORG_BIL_WORK_SERVICE", "ID"),
               new Column("PAYMENT_AMOUNT", DbType.Decimal));

            this.Database.AddEntityTable("JSKTSJ_CONTRACT_WORK_SERVICE",
                new RefColumn("CONTRACT_ID", "JSKTSJ_CONTR_WORK_SERV_CONTR_ID", "GKH_MORG_JSKTSJ_CONTRACT", "ID"),
                new RefColumn("WORK_SERVICE_ID", "JSKTSJ_CONTR_WORK_SERV_SERV_ID", "MANORG_BIL_WORK_SERVICE", "ID"),
                new Column("PAYMENT_AMOUNT", DbType.Decimal));

            this.Database.AddEntityTable("TRANSFER_CONTRACT_WORK_SERVICE",
               new RefColumn("CONTRACT_ID", "TRANSF_CONTR_WORK_SERV_CONTR_ID", "GKH_MORG_CONTRACT_JSKTSJ", "ID"),
               new RefColumn("WORK_SERVICE_ID", "TRANSF_CONTR_WORK_SERV_SERV_ID", "MANORG_BIL_WORK_SERVICE", "ID"),
               new Column("PAYMENT_AMOUNT", DbType.Decimal));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CONTRACT_OWNERS_WORK_SERVICE");
            this.Database.RemoveTable("JSKTSJ_CONTRACT_WORK_SERVICE");
            this.Database.RemoveTable("TRANSFER_CONTRACT_WORK_SERVICE");
        }
    }
}
