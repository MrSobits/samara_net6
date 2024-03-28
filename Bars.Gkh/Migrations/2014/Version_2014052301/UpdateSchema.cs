namespace Bars.Gkh.MigrationsVersion_2014052301
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014052300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MANAGING_ORGANIZATION", new Column("CASE_NUMBER", DbType.String, 100, ColumnProperty.Null));
            Database.AddRefColumn("GKH_MANAGING_ORGANIZATION", new RefColumn("TSJ_HEAD_CONTACT_ID", ColumnProperty.Null, "MANORG_CC_TSJ_H", "GKH_CONTRAGENT_CONTACT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MANAGING_ORGANIZATION", "TSJ_HEAD_CONTACT_ID");
            Database.RemoveColumn("GKH_MANAGING_ORGANIZATION", "CASE_NUMBER");
        }
    }
}