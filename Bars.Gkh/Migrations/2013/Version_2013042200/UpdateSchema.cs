namespace Bars.Gkh.Migrations.Version_2013042200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013042200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013041800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_BUILDER_DOCUMENT", new Column("DESCRIPTION", DbType.String, 2000));
            Database.ChangeColumn("GKH_BUILDER_DOCUMENT", new Column("DOCUMENT_NUM", DbType.String, 300));
            Database.ChangeColumn("GKH_BUILDER_DOCUMENT", new Column("DOCUMENT_NAME", DbType.String, 300));
        }

        public override void Down()
        {
            
        }
    }
}