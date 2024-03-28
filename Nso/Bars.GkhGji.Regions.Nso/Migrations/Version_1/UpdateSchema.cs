namespace Bars.GkhGji.Regions.Nso.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Справочник "Коды документов"
            Database.AddEntityTable("GJI_DOCUMENT_CODE",
                new Column("TYPE", DbType.Int32),
                new Column("CODE", DbType.Int32),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DOCUMENT_CODE_TYPE", false, "GJI_DOCUMENT_CODE", "TYPE");
            Database.AddIndex("IND_GJI_DOCUMENT_CODE_CODE", false, "GJI_DOCUMENT_CODE", "CODE");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DOCUMENT_CODE");
        }
    }
}