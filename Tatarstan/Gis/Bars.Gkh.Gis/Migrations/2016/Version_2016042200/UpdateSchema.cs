namespace Bars.Gkh.Gis.Migrations._2016.Version_2016042200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016042200")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("JSKTSJ_CONTRACT_COMM_SERVICE",
               new RefColumn("CONTRACT_ID", "JSKTSJ_COMM_SERV_CONTR", "GKH_MORG_JSKTSJ_CONTRACT", "ID"),
               new RefColumn("COMMUNAL_SERVICE_ID", "JSKTSJ_COMM_SERV_SERVICE", "MANORG_BIL_COMMUNAL_SERVICE", "ID"),
               new Column("START_DATE", DbType.DateTime),
               new Column("END_DATE", DbType.DateTime));

            this.Database.AddEntityTable("JSKTSJ_CONTRACT_ADD_SERVICE",
               new RefColumn("CONTRACT_ID", "JSKTSJ_ADD_SERV_CONTR", "GKH_MORG_JSKTSJ_CONTRACT", "ID"),
               new RefColumn("ADDITION_SERVICE_ID", "JSKTSJ_ADD_SERV_SERVICE", "MANORG_BIL_ADDITION_SERVICE", "ID"),
               new Column("START_DATE", DbType.DateTime),
               new Column("END_DATE", DbType.DateTime));

            this.Database.AddEntityTable("CONTRACT_OWNERS_COMM_SERVICE",
               new RefColumn("CONTRACT_ID", "CONTR_OWN_COMM_SERV_CONTR", "GKH_MORG_CONTRACT_OWNERS", "ID"),
               new RefColumn("COMMUNAL_SERVICE_ID", "CONTR_OWN_COMM_SERV_SERVICE", "MANORG_BIL_COMMUNAL_SERVICE", "ID"),
               new Column("START_DATE", DbType.DateTime),
               new Column("END_DATE", DbType.DateTime));

            this.Database.AddEntityTable("CONTRACT_OWNERS_ADD_SERVICE",
               new RefColumn("CONTRACT_ID", "CONTR_OWN_ADD_SERV_CONTR", "GKH_MORG_CONTRACT_OWNERS", "ID"),
               new RefColumn("ADDITION_SERVICE_ID", "CONTR_OWN_ADD_SERV_SERVICE", "MANORG_BIL_ADDITION_SERVICE", "ID"),
               new Column("START_DATE", DbType.DateTime),
               new Column("END_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveTable("JSKTSJ_CONTRACT_COMM_SERVICE");
            this.Database.RemoveTable("JSKTSJ_CONTRACT_ADD_SERVICE");
            this.Database.RemoveTable("CONTRACT_OWNERS_COMM_SERVICE");
            this.Database.RemoveTable("CONTRACT_OWNERS_ADD_SERVICE");
        }
    }
}
