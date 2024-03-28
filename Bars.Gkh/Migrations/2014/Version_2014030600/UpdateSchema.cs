namespace Bars.Gkh.Migrations.Version_2014030600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT",  new Column("AREA_COM_USAGE", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_COM_USAGE");
        }
    }
}