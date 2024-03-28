namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014111601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014111600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_ACTCHECKRO_LTEXT", new Column("NOTREVEALEDVIOL", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_ACTCHECKRO_LTEXT", "NOTREVEALEDVIOL");
        }
    }
}