namespace Bars.GkhGji.Migrations.Version_2014030401
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("PERSON_FOLLOW_CONVERSION", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "PERSON_FOLLOW_CONVERSION");
            
        }
    }
}