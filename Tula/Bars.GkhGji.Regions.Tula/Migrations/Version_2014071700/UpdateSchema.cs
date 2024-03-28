namespace Bars.GkhGji.Regions.Tula.Migrations.Version_2014071700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tula.Migrations.Version_2014061101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOL_PROS_DEFINITION", new Column("DOC_NUMBER", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOL_PROS_DEFINITION", "DOC_NUMBER");
        }
    }
}