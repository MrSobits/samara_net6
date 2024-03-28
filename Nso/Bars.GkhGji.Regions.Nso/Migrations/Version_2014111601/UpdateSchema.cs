namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014111601
{
    using System.Data;
    
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014111600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECKRO_LTEXT", new Column("NOTREVEALEDVIOL", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECKRO_LTEXT", "NOTREVEALEDVIOL");
        }
    }
}