using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Migrations.Version_2014122402
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014122402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014122401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // удаляю колонку поскольку в миграции 2014122300 не на ту таблицу проставился FK 
            Database.RemoveColumn("GKH_MANORG_LIC_DOC", "LIC_ID");

            Database.AddRefColumn("GKH_MANORG_LIC_DOC", new RefColumn("LIC_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_DOC_L", "GKH_MANORG_LICENSE", "ID"));

        }

        public override void Down()
        {
            // ненадо откатыфвать новая колонка лучше прежней
        }
    }
}