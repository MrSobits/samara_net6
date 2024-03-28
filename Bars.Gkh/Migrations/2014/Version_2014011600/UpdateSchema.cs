
namespace Bars.Gkh.Migrations.Version_201401600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014011500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
           Database.AddEntityTable(
               "GKH_FIELD_REQUIREMENT",
               new Column("REQUIREMENTID", DbType.String, 512, ColumnProperty.NotNull)
               );

           //ViewManager.Drop(Database, "Gkh");
           //ViewManager.Create(Database, "Gkh");
        }

        public override void Down()
        {
            //ViewManager.Drop(Database, "Gkh");
            Database.RemoveTable("GKH_FIELD_REQUIREMENT");
        }
    }
}