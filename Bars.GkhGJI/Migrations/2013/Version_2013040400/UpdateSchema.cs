namespace Bars.GkhGji.Migrations.Version_2013040400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_INSCHECK", new Column("NUM_DOCUMENT", DbType.String, 300));
            Database.AddColumn("GJI_INSPECTION_INSCHECK", new Column("TYPE_DOCUMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("GJI_INSPECTION_INSCHECK", new Column("DATE_DOCUMENT", DbType.DateTime));
            Database.AddColumn("GJI_INSPECTION_INSCHECK", new Column("DOC_FILE_ID", DbType.Int64, 22));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_INSCHECK", "DOC_FILE_ID");
            Database.RemoveColumn("GJI_INSPECTION_INSCHECK", "DATE_DOCUMENT");
            Database.RemoveColumn("GJI_INSPECTION_INSCHECK", "TYPE_DOCUMENT");
            Database.RemoveColumn("GJI_INSPECTION_INSCHECK", "NUM_DOCUMENT");
        }
    }
}