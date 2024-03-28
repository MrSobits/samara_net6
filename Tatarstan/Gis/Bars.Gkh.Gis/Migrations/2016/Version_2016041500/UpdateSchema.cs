namespace Bars.Gkh.Gis.Migrations._2016.Version_2016041500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016041500")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("MANORG_BIL_ADDITION_SERVICE",
                new RefColumn("MANORG_ID", "MO_BIL_ADD_SERV_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("BILSERVICE_ID", "MO_BIL_ADD_SERV_BILSERV", "BIL_DICT_SERVICE", "ID"));

            this.Database.AddEntityTable("MANORG_BIL_COMMUNAL_SERVICE",
                new RefColumn("MANORG_ID", "MO_BIL_COMM_SERV_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("BILSERVICE_ID", "MO_BIL_COMM_SERV_BILSERV", "BIL_DICT_SERVICE", "ID"),
                new Column("NAME", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("RESOURCE", DbType.Int32, ColumnProperty.NotNull, 0));

            this.Database.AddEntityTable("MANORG_BIL_WORK_SERVICE",
                new RefColumn("MANORG_ID", "MO_BIL_WORK_SERV_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("BILSERVICE_ID", "MO_BIL_WORK_SERV_BILSERV", "BIL_DICT_SERVICE", "ID"),
                new Column("PURPOSE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("TYPE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("DESCRIPTION", DbType.String, 2000));

            this.Database.AddEntityTable("MANORG_BIL_MKDWORK",
               new RefColumn("MKDWORK_ID", "MANORG_BIL_MKD_WORK", "GKH_DICT_CONTENT_REPAIR_MKD_WORK", "ID"),
               new RefColumn("WORKSERVICE_ID", "MANORG_BIL_SERVICE", "MANORG_BIL_WORK_SERVICE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("MANORG_BIL_ADDITION_SERVICE");
            this.Database.RemoveTable("MANORG_BIL_COMMUNAL_SERVICE");
            this.Database.RemoveTable("MANORG_BIL_WORK_SERVICE");
            this.Database.RemoveTable("MANORG_BIL_MKDWORK");
        }
    }
}
