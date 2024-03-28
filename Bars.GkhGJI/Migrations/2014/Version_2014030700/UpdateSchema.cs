namespace Bars.GkhGji.Migrations.Version_2014030700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014030401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("DOCUMENT_TIME", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "DOCUMENT_TIME");
            
        }
    }
}