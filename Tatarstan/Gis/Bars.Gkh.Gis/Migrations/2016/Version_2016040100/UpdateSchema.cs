namespace Bars.Gkh.Gis.Migrations._2016.Version_2016040100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016040100")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("BIL_DICT_SERVICE", new Column("ORDER_NUMBER", DbType.Int32));
            this.Database.AddColumn("BIL_DICT_SERVICE", new Column("IS_ODN_SERVICE", DbType.Boolean));
            this.Database.AddColumn("BIL_DICT_SERVICE", new Column("PARENT_SERVICE_CODE", DbType.Int32));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("BIL_DICT_SERVICE", "ORDER_NUMBER");
            this.Database.RemoveColumn("BIL_DICT_SERVICE", "IS_ODN_SERVICE");
            this.Database.RemoveColumn("BIL_DICT_SERVICE", "PARENT_SERVICE_CODE");
        }
    }
}