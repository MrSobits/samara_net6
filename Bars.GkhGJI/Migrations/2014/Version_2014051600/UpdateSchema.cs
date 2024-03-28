namespace Bars.GkhGji.Migrations.Version_2014051600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014051301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_ACTCHECK_ROBJECT", new Column("DESCRIPTION", DbType.String, 2000));
        }

        public override void Down()
        {
           // при отмене миграции нельзя менят ьна предудущий размер длину строки . Строки обнулится
        }
    }
}