namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015083100
{
	using System.Data;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015083100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migration.Version_2014092400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddJoinedSubclassTable("TOMSK_GJI_APPEAL_CITIZENS", "GJI_APPEAL_CITIZENS", "CHEL_APCIT",
                new Column("COMMENT", DbType.String, 2000, ColumnProperty.Null));

            Database.ExecuteNonQuery(@"insert into TOMSK_GJI_APPEAL_CITIZENS (id)
                                     select id from GJI_APPEAL_CITIZENS");

			Database.AddEntityTable("GJI_APPCIT_EXECUTANT",
				new Column("ORDER_DATE", DbType.DateTime, ColumnProperty.NotNull),
				new Column("PERFOM_DATE", DbType.DateTime, ColumnProperty.Null),
				new Column("RESPONSIBLE", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("DESCRIPTION", DbType.String, 255),
				new RefColumn("APPCIT_ID", ColumnProperty.NotNull, "GJI_APPCITEXEC_APP", "GJI_APPEAL_CITIZENS", "ID"),
				new RefColumn("EXECUTANT_ID", ColumnProperty.Null, "GJI_APPCITEXEC_EXEC", "GKH_DICT_INSPECTOR", "ID"),
				new RefColumn("AUTHOR_ID", ColumnProperty.Null, "GJI_APPCITEXEC_AUTH", "GKH_DICT_INSPECTOR", "ID"),
				new RefColumn("CONTROLLER_ID", ColumnProperty.Null, "GJI_APPCITEXEC_CTRL", "GKH_DICT_INSPECTOR", "ID"),
				new RefColumn("STATE_ID", ColumnProperty.Null, "GJI_APPCITEXEC_STATE", "B4_STATE", "ID"),
				new RefColumn("RESOLUTION_ID", ColumnProperty.Null, "GJI_APPCITEXEC_RES", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_EXECUTANT");
            Database.RemoveTable("TOMSK_GJI_APPEAL_CITIZENS");
        }
    }
}