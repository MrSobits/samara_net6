namespace Bars.GkhGji.Regions.Tyumen.Migrations.Version_2018120700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018120700")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Regions.Tyumen.Migrations.Version_2018120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_LICENSE_NOTIFICATION",
                new Column("COMMENT", DbType.String),
                new Column("LOCAL_GOVERNMENT_ID", DbType.Int64),
                new Column("CONTRAGENT_ID", DbType.Int64),
                new Column("MORG_RO_ID", DbType.Int64),
                new Column("MO_DATE_START", DbType.DateTime),
                new Column("NOTICE_OMS_SEND_DATE", DbType.DateTime),
                new Column("NOTICE_RECIVED_DATE", DbType.DateTime),
                new Column("OMS_NOTISE_RESULT", DbType.Int64),
                new Column("OMS_NOTISE_RESULT_DATE", DbType.DateTime),
                new Column("OMS_NOTISE_RESULT_NUMBER", DbType.String, 30),
                new Column("REGISTRED_NUMBER", DbType.String, 30),
                new Column("LICENSE_NOTIFICATION_NUMBER", DbType.String, 30));

            Database.AddForeignKey("FK_GJI_LIC_NOT_LOC_GOV", "GJI_LICENSE_NOTIFICATION", "LOCAL_GOVERNMENT_ID", "GKH_LOCAL_GOVERNMENT", "ID");
            Database.AddForeignKey("FK_GJI_LIC_NOT_CONTR", "GJI_LICENSE_NOTIFICATION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_LIC_NOT_MORG_RO", "GJI_LICENSE_NOTIFICATION", "MORG_RO_ID", "GKH_MORG_CONTRACT_REALOBJ", "ID");

        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {           
            Database.RemoveTable("GJI_LICENSE_NOTIFICATION");
        }
    }
}