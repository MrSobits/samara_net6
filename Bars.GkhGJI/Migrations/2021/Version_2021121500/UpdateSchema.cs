namespace Bars.GkhGji.Migrations._2021.Version_2021121500
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021121500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021121000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {           

            Database.AddEntityTable(
              "GJI_EDS_INSPECTION_DOCUMENT",
              new RefColumn("EDS_ID", "GJI_EDS_INSPDOC_EDS", "GJI_EDS_INSPECTION", "ID"),
              new RefColumn("FILE_ID", "GJI_EDS_INSPDOC_FILE", "B4_FILE_INFO", "ID"),              
              new Column("DOCUMENT_DATE", DbType.DateTime),
              new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
              new Column("DESCRIPTION", DbType.String, 2000),
              new Column("EXTERNAL_ID", DbType.String, 36),
              new RefColumn("SIGNED_FILE_ID", "GJI_EDS_INSPDOC_SIGNED_FILE", "B4_FILE_INFO", "ID"),
              new RefColumn("SIGNATURE_FILE_ID", "GJI_EDS_INSPDOC_SIGNATURE", "B4_FILE_INFO", "ID"),
              new RefColumn("CERTIFICATE_FILE_ID", "GJI_EDS_INSPDOC_CERTIFICATE", "B4_FILE_INFO", "ID"),
              new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Enums.MessageCheck.NotSet));

          
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_EDS_INSPECTION_DOCUMENT");
        }
    }
}