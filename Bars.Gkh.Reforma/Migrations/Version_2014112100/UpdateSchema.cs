namespace Bars.Gkh.Reforma.Migrations.Version_2014112100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014111900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("RFRM_ACTION_LOG", new Column("DETAILS", DbType.String, 1000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("RFRM_ACTION_LOG", "DETAILS");
        }
    }
}