namespace Bars.GkhGji.Regions.Tula.Migrations.Version_2014090902
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tula.Migrations.Version_2014090901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_DICT_LEGAL_REASON", new Column("NAME", DbType.String, 2000));
        }

        public override void Down()
        {
            
        }
    }
}