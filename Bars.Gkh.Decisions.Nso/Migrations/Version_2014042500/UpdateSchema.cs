namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014042500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014040700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("PHONE_AUTHORIZED_PERSON", DbType.String, 200, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "PHONE_AUTHORIZED_PERSON");
        }
    }
}
