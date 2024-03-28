namespace Bars.GkhGji.Migrations._2015.Version_2015061102
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015061102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2015053100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("GJI_DOCUMENT_INSPECTOR", "INSPECTOR_ORDER"))
            {
                Database.AddColumn("GJI_DOCUMENT_INSPECTOR", new Column("INSPECTOR_ORDER", DbType.Int32));
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("GJI_DOCUMENT_INSPECTOR", "INSPECTOR_ORDER"))
            {
                Database.RemoveColumn("GJI_DOCUMENT_INSPECTOR", "INSPECTOR_ORDER");
            }
        }
    }
}
