namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015091400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015080400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddTable("GJI_PROTOCOL197",
				new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
		        new Column("PHYSICAL_PERSON", DbType.String, 300),
		        new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
		        new Column("DATE_TO_COURT", DbType.DateTime),
		        new Column("TO_COURT", DbType.Boolean, ColumnProperty.NotNull, false),
		        new Column("DESCRIPTION", DbType.String, 2000),
		        new Column("DATE_OF_PROCEEDINGS", DbType.Date),
		        new Column("HOUR_OF_PROCEEDINGS", DbType.Int32),
		        new Column("MINUTE_OF_PROCEEDINGS", DbType.Int32),
		        new Column("PERSON_FOLLOW_CONVERSION", DbType.String),
				new Column("FORMAT_DATE", DbType.DateTime),
                new Column("NOTIF_NUM", DbType.String, 100),
                new Column("PROCEEDINGS_PLACE", DbType.String, 1000),
                new Column("REMARKS", DbType.String, 1000),
				new Column("PERSON_REG_ADDRESS", DbType.String, 2000, ColumnProperty.Null),
				new Column("PERSON_JOB", DbType.String, 2000, ColumnProperty.Null),
				new Column("PERSON_POSITION", DbType.String, 2000, ColumnProperty.Null),
				new Column("PERSON_BIRTHDATE", DbType.String, 2000, ColumnProperty.Null),
				new Column("PERSON_DOC", DbType.String, 2000, ColumnProperty.Null),
				new Column("PERSON_SALARY", DbType.String, 2000, ColumnProperty.Null),
				new Column("PERSON_RELAT", DbType.String, 2000, ColumnProperty.Null),
				new Column("FORMAT_PLACE", DbType.String, 500, ColumnProperty.Null),
				new Column("FORMAT_HOUR", DbType.Int32, ColumnProperty.Null),
				new Column("FORMAT_MINUTE", DbType.Int32, ColumnProperty.Null),
				new Column("PERSON_FACT_ADDRESS", DbType.String, 250, ColumnProperty.Null),
				new Column("TYPE_PRESENCE", DbType.Int16, ColumnProperty.NotNull, (int)TypeRepresentativePresence.None),
				new Column("REPRESENTATIVE", DbType.String, 500, ColumnProperty.Null),
				new Column("REASON_TYPE_REQ", DbType.String, 1000, ColumnProperty.Null),
				new Column("DELIV_THROUGH_OFFICE", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("PROCEEDING_COPY_NUM", DbType.Int32, ColumnProperty.Null),
				new Column("DATE_OF_VIOLATION", DbType.DateTime, ColumnProperty.Null),
				new Column("HOUR_OF_VIOLATION", DbType.Int32, ColumnProperty.Null),
				new Column("MINUTE_OF_VIOLATION", DbType.Int32, ColumnProperty.Null),
				new RefColumn("RESOLVE_VIOL_CLAIM_ID", ColumnProperty.Null, "PROTO197_RES_V_CL", "GJI_DICT_RES_VIOL_CLAIM", "ID"),
				new RefColumn("NORMATIVE_DOC_ID", ColumnProperty.Null, "PROT197_NORM_DOC", "GKH_DICT_NORMATIVE_DOC", "ID"),
		        new RefColumn("EXECUTANT_ID", ColumnProperty.Null, "GJI_PROT197_EXE", "GJI_DICT_EXECUTANT", "ID"),
		        new RefColumn("CONTRAGENT_ID", ColumnProperty.Null, "GJI_PROT197_CTR", "GKH_CONTRAGENT", "ID")
		        );

			this.Database.AddForeignKey("FK_GJI_PROT197_DOC", "GJI_PROTOCOL197", "ID", "GJI_DOCUMENT", "ID");

			this.Database.AddEntityTable(
				"GJI_PROTOCOL197_SUR_REQ",
				new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "PROT197_REQ", "GJI_PROTOCOL197", "ID"),
				new RefColumn("REQUIREMENT_ID", ColumnProperty.NotNull, "PROT197_REQ_REQ", "GJI_DICT_SURVEY_SUBJ_REQ", "ID"));

	        this.Database.AddTable(
		        "GJI_PROTOCOL197_VIOLAT",
		        new RefColumn("ID", "GJI_PROT197_VIOLAT_STG", "GJI_INSPECTION_VIOL_STAGE", "ID"),
		        new Column("DESCRIPTION", DbType.String, 1000));

			this.Database.AddEntityTable(
				"GJI_PROTOCOL197_ARTLAW",
				new RefColumn("PROTOCOL_ID", "GJI_PROT197_ARTLAW_DOC", "GJI_PROTOCOL197", "ID"),
				new RefColumn("ARTICLELAW_ID", "GJI_PROT197_ARTLAW_ARL", "GJI_DICT_ARTICLELAW", "ID"),
				new Column("DESCRIPTION", DbType.String, 2000),
				new Column("EXTERNAL_ID", DbType.String, 36));

			this.Database.AddEntityTable(
				"GJI_PROT197_ACTIV_DIRECT",
				new RefColumn("ACTIVEDIRECT_ID", ColumnProperty.NotNull, "GJI_PROT197_ACTIV_DIRECT_A", "GJI_ACTIVITY_DIRECTION", "ID"),
				new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROT197_ACTIV_DIRECT_P", "GJI_PROTOCOL197", "ID"));

	        this.Database.AddEntityTable(
		        "GJI_PROTOCOL197_ANNEX",
		        new RefColumn("PROTOCOL_ID", "GJI_PROT197_ANNEX_DOC", "GJI_PROTOCOL197", "ID"),
				new RefColumn("FILE_ID", "GJI_PROT197_ANNEX_FILE", "B4_FILE_INFO", "ID"),
		        new Column("DOCUMENT_DATE", DbType.DateTime),
		        new Column("NAME", DbType.String, 300),
		        new Column("DESCRIPTION", DbType.String, 2000),
		        new Column("EXTERNAL_ID", DbType.String, 36));

			this.Database.AddEntityTable(
				"GJI_PROTOCOL197_LTEXT",
				new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROTOCOL197_LTEXT_P", "GJI_PROTOCOL197", "ID"),
				new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null),
				new Column("WITNESSES", DbType.Binary),
				new Column("VICTIMS", DbType.Binary));
        }

        public override void Down()
        {
			this.Database.RemoveTable("GJI_PROTOCOL197");
			this.Database.RemoveTable("GJI_PROTOCOL197_SUR_REQ");
			this.Database.RemoveTable("GJI_PROTOCOL197_VIOLAT");
			this.Database.RemoveTable("GJI_PROTOCOL197_ARTLAW");
			this.Database.RemoveTable("GJI_PROT197_ACTIV_DIRECT");
			this.Database.RemoveTable("GJI_PROTOCOL197_ANNEX");
			this.Database.RemoveTable("GJI_PROTOCOL197_LTEXT");
        }
    }
}