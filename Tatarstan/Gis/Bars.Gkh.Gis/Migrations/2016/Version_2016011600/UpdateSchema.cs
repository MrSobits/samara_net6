namespace Bars.Gkh.Gis.Migrations._2016.Version_2016011600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016011600")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("BIL_IMPORT_ADDRESS", new Column("NZP_DOM", DbType.Int32));
            this.Database.AddColumn("BIL_IMPORT_ADDRESS", new Column("DATA_BANK_ID", DbType.Int32));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("BIL_IMPORT_ADDRESS", "NZP_DOM");
            this.Database.RemoveColumn("BIL_IMPORT_ADDRESS", "DATA_BANK_ID");
        }
    }
}