namespace Bars.GkhGji.Migrations.Version_2014021300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014012901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESCRIPTION_VIOLAT", "DESCRIPTION", DbType.String, 500);
            Database.AddColumn("GJI_INSPECTION_VIOLATION", "DESCRIPTION", DbType.String, 500);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PRESCRIPTION_VIOLAT", "DESCRIPTION");
            Database.RemoveColumn("GJI_INSPECTION_VIOLATION", "DESCRIPTION");
        }
    }
}