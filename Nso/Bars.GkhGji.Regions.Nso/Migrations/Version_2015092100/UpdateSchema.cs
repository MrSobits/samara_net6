namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015092100
{
	using global::Bars.B4.Modules.Ecm7.Framework;
	using System.Data;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
	using Bars.Gkh;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015091400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		public override void Up()
		{
			Database.AddTable(
				"GJI_NSO_ACTREMOVAL",
				new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
				new Column("ACQUAINT_WITH_DISP", DbType.String, 250),
				new Column("DOCUMENT_PLACE", DbType.String, 1000),
				new Column("DOCUMENT_TIME", DbType.DateTime, 25));

			Database.ExecuteNonQuery(@"insert into GJI_NSO_ACTREMOVAL (id)
                                     select id from GJI_ACTREMOVAL");

			Database.AddEntityTable("GJI_NSO_ACTREMOVAL_PROVDOC",
				new Column("DATE_PROVIDED", DbType.DateTime),
				new RefColumn("PROVDOC_ID", ColumnProperty.NotNull, "GJI_NSO_ACTR_PROVDOC_P", "GJI_DICT_PROVIDEDDOCUMENT", "ID"),
				new RefColumn("ACT_ID", ColumnProperty.NotNull, "GJI_NSO_ACTR_PROVDOC_A", "GJI_ACTREMOVAL", "ID"));

			Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_ANNEX",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_ANNEX_DOC", "GJI_ACTREMOVAL", "ID"),
				new RefColumn("FILE_ID", "GJI_NSO_ACTR_ANNEX_FILE", "B4_FILE_INFO", "ID"),
				new Column("DOCUMENT_DATE", DbType.DateTime),
				new Column("NAME", DbType.String, 300),
				new Column("DESCRIPTION", DbType.String, 2000),
				new Column("EXTERNAL_ID", DbType.String, 36));

			Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_DEFINITION",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_DEF_DOC", "GJI_ACTREMOVAL", "ID"),
				new RefColumn("ISSUED_DEFINITION_ID", "GJI_NSO_ACTR_DEF_ISD", "GKH_DICT_INSPECTOR", "ID"),
				new Column("EXECUTION_DATE", DbType.DateTime),
				new Column("DOCUMENT_DATE", DbType.DateTime),
				new Column("DOCUMENT_NUM", DbType.String, 300),
				new Column("DOC_NUMBER", DbType.Int32),
				new Column("DESCRIPTION", DbType.String, 2000),
				new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
				new Column("EXTERNAL_ID", DbType.String, 36));

			Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_INSPECTPART",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_INS_ACT", "GJI_ACTREMOVAL", "ID"),
				new RefColumn("INSPECTIONPART_ID", "GJI_NSO_ACTR_INS_PART", "GJI_DICT_INSPECTEDPART", "ID"),
				new Column("CHARACTER", DbType.String, 300),
				new Column("DESCRIPTION", DbType.String, 500),
				new Column("EXTERNAL_ID", DbType.String, 36));

			Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_PERIOD",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_PERIOD_DOC", "GJI_ACTREMOVAL", "ID"),
				new Column("DATE_CHECK", DbType.DateTime),
				new Column("DATE_START", DbType.DateTime),
				new Column("DATE_END", DbType.DateTime));

			Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_WITNESS",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_WIT_DOC", "GJI_ACTREMOVAL", "ID"),
				new Column("POSITION", DbType.String, 300),
				new Column("IS_FAMILIAR", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("FIO", DbType.String, ColumnProperty.NotNull, 300),
				new Column("EXTERNAL_ID", DbType.String, 36));

			ViewManager.Create(Database, "GkhGjiNso", "CreateViewActcheck");
		}

		public override void Down()
		{
			ViewManager.Drop(Database, "GkhGjiNso", "DeleteViewActcheck");

			Database.RemoveTable("GJI_NSO_ACTREMOVAL");
			Database.RemoveTable("GJI_NSO_ACTREMOVAL_PROVDOC");
			Database.RemoveTable("GJI_NSO_ACTREMOVAL_ANNEX");
			Database.RemoveTable("GJI_NSO_ACTREMOVAL_DEFINITION");
			Database.RemoveTable("GJI_NSO_ACTREMOVAL_INSPECTPART");
			Database.RemoveTable("GJI_NSO_ACTREMOVAL_PERIOD");
			Database.RemoveTable("GJI_NSO_ACTREMOVAL_WITNESS");
		}
    }
}