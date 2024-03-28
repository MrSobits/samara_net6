namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_1
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    [MigrationDependsOn(typeof(Version_0.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!this.Database.TableExists("GJI_DOCUMENT_CODE"))
            {
                //Справочник "Коды документов"
                this.Database.AddEntityTable("GJI_DOCUMENT_CODE",
                    new Column("TYPE", DbType.Int32),
                    new Column("CODE", DbType.Int32),
                    new Column("EXTERNAL_ID", DbType.String, 36));
                this.Database.AddIndex("IND_GJI_DOCUMENT_CODE_TYPE", false, "GJI_DOCUMENT_CODE", "TYPE");
                this.Database.AddIndex("IND_GJI_DOCUMENT_CODE_CODE", false, "GJI_DOCUMENT_CODE", "CODE");
            }
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DOCUMENT_CODE");
        }
    }
}