namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015092100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015091400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		public override void Up()
		{
			this.Database.AddTable(
				"GJI_NSO_ACTREMOVAL",
				new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
				new Column("ACQUAINT_WITH_DISP", DbType.String, 250),
				new Column("DOCUMENT_PLACE", DbType.String, 1000),
				new Column("DOCUMENT_TIME", DbType.DateTime, 25));

			this.Database.ExecuteNonQuery(@"insert into GJI_NSO_ACTREMOVAL (id)
                                     select id from GJI_ACTREMOVAL");

			this.Database.AddEntityTable("GJI_NSO_ACTREMOVAL_PROVDOC",
				new Column("DATE_PROVIDED", DbType.DateTime),
				new RefColumn("PROVDOC_ID", ColumnProperty.NotNull, "GJI_NSO_ACTR_PROVDOC_P", "GJI_DICT_PROVIDEDDOCUMENT", "ID"),
				new RefColumn("ACT_ID", ColumnProperty.NotNull, "GJI_NSO_ACTR_PROVDOC_A", "GJI_ACTREMOVAL", "ID"));

			this.Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_ANNEX",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_ANNEX_DOC", "GJI_ACTREMOVAL", "ID"),
				new RefColumn("FILE_ID", "GJI_NSO_ACTR_ANNEX_FILE", "B4_FILE_INFO", "ID"),
				new Column("DOCUMENT_DATE", DbType.DateTime),
				new Column("NAME", DbType.String, 300),
				new Column("DESCRIPTION", DbType.String, 2000),
				new Column("EXTERNAL_ID", DbType.String, 36));

			this.Database.AddEntityTable(
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

			this.Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_INSPECTPART",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_INS_ACT", "GJI_ACTREMOVAL", "ID"),
				new RefColumn("INSPECTIONPART_ID", "GJI_NSO_ACTR_INS_PART", "GJI_DICT_INSPECTEDPART", "ID"),
				new Column("CHARACTER", DbType.String, 300),
				new Column("DESCRIPTION", DbType.String, 500),
				new Column("EXTERNAL_ID", DbType.String, 36));

			this.Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_PERIOD",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_PERIOD_DOC", "GJI_ACTREMOVAL", "ID"),
				new Column("DATE_CHECK", DbType.DateTime),
				new Column("DATE_START", DbType.DateTime),
				new Column("DATE_END", DbType.DateTime));

			this.Database.AddEntityTable(
				"GJI_NSO_ACTREMOVAL_WITNESS",
				new RefColumn("ACTREMOVAL_ID", "GJI_NSO_ACTR_WIT_DOC", "GJI_ACTREMOVAL", "ID"),
				new Column("POSITION", DbType.String, 300),
				new Column("IS_FAMILIAR", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("FIO", DbType.String, ColumnProperty.NotNull, 300),
				new Column("EXTERNAL_ID", DbType.String, 36));
		}

		public override void Down()
		{
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL");
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL_PROVDOC");
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL_ANNEX");
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL_DEFINITION");
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL_INSPECTPART");
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL_PERIOD");
			this.Database.RemoveTable("GJI_NSO_ACTREMOVAL_WITNESS");
		}
    }
}