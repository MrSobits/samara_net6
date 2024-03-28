namespace Bars.GkhGji.Migrations._2021.Version_2021102600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021102600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021072600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_VIOLATION", "ERP_GUID", DbType.String, 50);          
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_VIOLATION", "ERP_GUID");
        }
    }
}