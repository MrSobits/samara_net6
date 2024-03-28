namespace Bars.Gkh.Migrations._2017.Version_2017061500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017061500")]
    [MigrationDependsOn(typeof(Version_2017061300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("NOTICE_ACCEPTANCE_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("NOTICE_VIOLATION_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("REVIEW_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("NOTICE_RETURN_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("REVIEW_DATE_LK", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("PREPARATION_OFFER_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("SEND_RESULT_DATE", DbType.DateTime));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("SEND_METHOD", DbType.Int16));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "NOTICE_ACCEPTANCE_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "NOTICE_VIOLATION_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "REVIEW_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "NOTICE_RETURN_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "REVIEW_DATE_LK");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "PREPARATION_OFFER_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "SEND_RESULT_DATE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "SEND_METHOD");
        }
    }
}