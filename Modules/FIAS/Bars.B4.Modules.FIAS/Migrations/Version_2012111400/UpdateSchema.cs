namespace Bars.B4.Modules.FIAS.Migrations.Version_2012111400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2012111400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("B4_FIAS_ADDRESS", new Column("ADDRESS_GUID", DbType.String, 1000));
        }

        public override void Down()
        {
            Database.RemoveColumn("B4_FIAS_ADDRESS", "ADDRESS_GUID");
        }
    }
}