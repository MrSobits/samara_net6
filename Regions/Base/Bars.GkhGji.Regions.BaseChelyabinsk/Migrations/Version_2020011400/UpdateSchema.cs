namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020011400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2020011400")]
    [MigrationDependsOn(typeof(Version_2019080900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
		public override void Up()
		{

			this.Database.AddEntityTable(
                "GJI_APPCIT_ORDER",
                new Column("ORDER_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PERFOM_DATE", DbType.DateTime, ColumnProperty.None),
                new Column("DESCRIPTION", DbType.String, 5000),
                new Column("APPEAL_TEXT", DbType.String, 5000),
                new Column("PERSON", DbType.String, 500),
                new RefColumn("APPCIT_ID", "FK_GJI_APPCIT_ORDER_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("CONTRAGENT_ID", "FK_GJI_APPCIT_ORDER_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                new RefColumn("FILE_ID", "FK_GJI_APPCIT_ORDER_FILE", "B4_FILE_INFO", "ID"),
                new Column("EXECUTED", DbType.Int32, 4, ColumnProperty.NotNull, 30));

            this.Database.AddEntityTable(
                "GJI_APPCITORDER_FILE",
                new Column("DESCRIPTION", DbType.String, 500),            
                new RefColumn("APPCITORDER_ID", "FK_GJI_APPCITORDER_FILE_ORDER", "GJI_APPCIT_ORDER", "ID"),
                new RefColumn("FILE_ID", "FK_GJI_APPCITORDER_FILE_FILE", "B4_FILE_INFO", "ID"));


        }

		public override void Down()
		{
			this.Database.RemoveTable("GJI_APPCITORDER_FILE");
			this.Database.RemoveTable("GJI_APPCIT_ORDER");
		
		}
    }
}