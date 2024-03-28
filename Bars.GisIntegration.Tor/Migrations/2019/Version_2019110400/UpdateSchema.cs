namespace Bars.GisIntegration.Tor.Migrations._2019.Version_2019110400
{
	using System.Data;

	using Bars.B4.Modules.Ecm7.Framework;
	using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	/// <summary>
	/// Миграция
	/// </summary>
	[Migration("2019110400")]
	public class UpdateSchema : Migration
	{
		public override void Up()
		{
			this.Database.AddEntityTable("GI_TOR_TASK",
			    new Column("REQUEST_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("SEND_OBJECT_TYPE", DbType.Int32, ColumnProperty.NotNull),
				new Column("TASK_STATE", DbType.Int32, ColumnProperty.NotNull),
				new RefColumn("USER_ID", "GJI_TOR_TASK_USER", "B4_USER", "ID"),
				new RefColumn("DISPOSAL_ID", "GJI_TOR_TASK_DISPOSAL", "GJI_DISPOSAL", "ID"),
				new RefColumn("REQUEST_FILE_ID", "GJI_TOR_TASK_FILE_REQUEST", "B4_FILE_INFO", "ID"),
				new RefColumn("RESPONSE_FILE_ID", "GJI_TOR_TASK_FILE_RESPONSE", "B4_FILE_INFO", "ID"),
				new RefColumn("LOG_FILE_ID", "GJI_TOR_TASK_FILE_LOG", "B4_FILE_INFO", "ID"));
		}

		public override void Down()
		{
			this.Database.RemoveTable("GI_TOR_TASK");
		}
	}
}