namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015072200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015072101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddEntityTable("GJI_MKD_CHANGE_NOTIFICATION_FILE",
				new Column("DOC_NAME", DbType.String, 100, ColumnProperty.NotNull),
				new Column("DOC_NUMBER", DbType.String, 50, ColumnProperty.NotNull),
				new Column("DOC_DATE", DbType.DateTime, ColumnProperty.NotNull),
				new Column("DOC_DESC", DbType.String, 500, ColumnProperty.Null),
				new RefColumn("MKD_CHANGE_NOTIFICATION_ID", "MKD_CHANGE_NOTIFICATION_FILE_CN", "GJI_MKD_CHANGE_NOTIFICATION", "ID"),
				new RefColumn("FILE_ID", "MKD_CHANGE_NOTIFICATION_FILE_FI", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
			this.Database.RemoveTable("GJI_MKD_CHANGE_NOTIFICATION_FILE");
        }
    }
}