namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022100400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022100400")]
    [MigrationDependsOn(typeof(Version_2022081100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
                 "GJI_CH_APPCIT_ADMON_ANNEX",
                 new Column("APPCIT_ADMONITION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                 new Column("FILE_ID", DbType.Int64, 22),
                 new Column("DOCUMENT_DATE", DbType.DateTime),
                 new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                 new Column("DESCRIPTION", DbType.String, 500),
                 new Column("EXTERNAL_ID", DbType.String, 36),
                 new RefColumn("SIGNED_FILE_ID", "GJI_CH_APPCIT_ADMON_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"),
                 new RefColumn("SIGNATURE_FILE_ID", "GJI_CH_APPCIT_ADMON_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"),
                 new RefColumn("CERTIFICATE_FILE_ID", "GJI_CH_APPCIT_ADMON_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"),
                 new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));

            Database.AddIndex("IND_GJI_APPCIT_ADMONITION_ANNEX_FILE", false, "GJI_CH_APPCIT_ADMON_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_APPCIT_ADMONITION_ANNEX_DOC", false, "GJI_CH_APPCIT_ADMON_ANNEX", "APPCIT_ADMONITION_ID");
            Database.AddForeignKey("FK_GJI_APPCIT_ADMONITION_ANNEX_DOC", "GJI_CH_APPCIT_ADMON_ANNEX", "APPCIT_ADMONITION_ID", "GJI_CH_APPCIT_ADMONITION", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_ADMONITION_ANNEX_FILE", "GJI_CH_APPCIT_ADMON_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_APPCIT_ADMON_ANNEX");
        }
    }
}