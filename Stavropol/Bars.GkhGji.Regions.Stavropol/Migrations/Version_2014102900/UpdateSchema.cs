using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.GkhGji.Regions.Stavropol.Migrations.Version_2014102900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Stavropol.Migrations.Version_2014102000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		public override void Up()
		{
			Database.AddEntityTable(
				"GJI_RESOLPROS_DEFINITION",
				new Column("DOCUMENT_DATE", DbType.DateTime),
				new Column("EXECUTION_DATE", DbType.DateTime),
				new Column("DOC_NUMBER", DbType.Int32),
				new Column("DOCUMENT_NUM", DbType.String, 300),
				new Column("DESCRIPTION", DbType.String, 2000),
				new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
				new Column("DEF_TIME", DbType.DateTime, 25),
				new Column("DATE_PROC", DbType.DateTime),
				new Column("RESOLPROS_ID", DbType.Int64, 22, ColumnProperty.NotNull),
				new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
				new Column("EXTERNAL_ID", DbType.String, 36));

			Database.AddIndex("IND_GJI_RESPROS_DEF_DOC", false, "GJI_RESOLPROS_DEFINITION", "RESOLPROS_ID");
			Database.AddIndex("IND_GJI_RESPROS_DEF_ISD", false, "GJI_RESOLPROS_DEFINITION", "ISSUED_DEFINITION_ID");
			Database.AddForeignKey("FK_GJI_RESPROS_DEF_ISD", "GJI_RESOLPROS_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR",
				"ID");
			Database.AddForeignKey("FK_GJI_RESPROS_DEF_DOC", "GJI_RESOLPROS_DEFINITION", "RESOLPROS_ID", "GJI_RESOLPROS", "ID");
		}

		public override void Down()
		{
			Database.RemoveTable("GJI_RESOLPROS_DEFINITION");
		}
    }
}