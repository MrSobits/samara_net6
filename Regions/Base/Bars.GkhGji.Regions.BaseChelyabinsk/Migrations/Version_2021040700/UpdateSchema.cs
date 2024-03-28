namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021040700
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021040700")]
    [MigrationDependsOn(typeof(Version_2021032200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
			this.Database.AddEntityTable("GJI_LICENSE_ACTION",
				new Column("APPLICANT_AGREEMENT", DbType.String, 500, ColumnProperty.Null),
				new Column("APPLICANT_EMAIL", DbType.String, 100, ColumnProperty.Null),
				new Column("APPLICANT_ESIA_ID", DbType.String, 200, ColumnProperty.Null),
				new Column("APPLICANT_NAME", DbType.String, 20, ColumnProperty.Null),
				new Column("APPLICANT_INN", DbType.String, 20, ColumnProperty.Null),
				new Column("APPLICANT_LASTNAME", DbType.String, 200, ColumnProperty.Null),
				new Column("APPLICANT_MIDDLENAME", DbType.String, 250, ColumnProperty.Null),
				new Column("APPLICANT_OKVED", DbType.String, 100, ColumnProperty.Null),
				new Column("APPLICANT_PHONE", DbType.String, 20, ColumnProperty.Null),
				new Column("APPLICANT_SNILS", DbType.String, 100, ColumnProperty.Null),
				new Column("APPLICANT_TYPE", DbType.String, 100, ColumnProperty.Null),
				new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.Null),
				new Column("DOCUMENT_ISSUER", DbType.String, 500, ColumnProperty.Null),
				new Column("DOCUMENT_NAME", DbType.String, 500, ColumnProperty.Null),
				new Column("DOCUMENT_NUMBER", DbType.String, 50, ColumnProperty.Null),
				new Column("DOCUMENT_SERIES", DbType.String, 50, ColumnProperty.Null),
				new Column("DOCUMENT_TYPE", DbType.String, 100, ColumnProperty.Null),
				new Column("ACTION_TYPE", DbType.Int32, ColumnProperty.Null),
				new Column("LICENSE_DATE", DbType.DateTime, ColumnProperty.Null),
				new Column("LICENSE_NUMBER", DbType.String, 100, ColumnProperty.Null),
				new Column("MIDDLENAMEFL", DbType.String, 250, ColumnProperty.Null),
				new Column("NAMEFL", DbType.String, 20, ColumnProperty.Null),
				new Column("POSITION", DbType.String, 200, ColumnProperty.Null),
				new Column("SURNAMEFL", DbType.String, 200, ColumnProperty.Null),
				new RefColumn("CONTRAGENT_ID", "GJI_LICENSE_ACTION_GC", "GKH_CONTRAGENT", "ID"),
				new RefColumn("FILE_INFO_ID", "GJI_LICENSE_ACTION_FI", "B4_FILE_INFO", "ID"),
				new RefColumn("STATE_ID", "GJI_LICENSE_ACTION_ST", "B4_STATE", "ID"));

			this.Database.AddEntityTable("GJI_LICENSE_ACTION_FILE",
                new RefColumn("LICENSE_ACTION_ID", "GJI_LICENSE_ACTION_FILE_CN", "GJI_LICENSE_ACTION", "ID"),
                new RefColumn("FILE_ID", "GJI_LICENSE_ACTION_FILE_FI", "B4_FILE_INFO", "ID"));


		}
        public override void Down()
        {
            this.Database.RemoveTable("GJI_LICENSE_ACTION_FILE");
			this.Database.RemoveTable("GJI_LICENSE_ACTION");
		}
    }
}