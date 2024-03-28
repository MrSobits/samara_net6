namespace Bars.Gkh.Regions.Nso.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_NSO_ROBJ_DOCUMENT",
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "ROBJECT_DOC_ROBJECT", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "ROBJECT_DOC_FILE", "B4_FILE_INFO", "ID"),
                new Column("NAME", DbType.String, 300),
                new Column("DOCUMENT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_NSO_ROBJ_DOCUMENT");
        }
    }
}