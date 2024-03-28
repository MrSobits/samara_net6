namespace Bars.GkhGji.Migrations.Version_2013032600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "TYPE_CHECK");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("TYPE_CHECK", DbType.Int32, 4, ColumnProperty.NotNull, 1));
        }
    }
}