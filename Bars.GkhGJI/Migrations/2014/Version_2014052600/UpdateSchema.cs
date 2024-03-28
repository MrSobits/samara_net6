namespace Bars.GkhGji.Migration.Version_2014052600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014052100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("ANOTHER_REASONS", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "ANOTHER_REASONS");
        }
    }
}