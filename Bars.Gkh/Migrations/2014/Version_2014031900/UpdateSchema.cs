namespace Bars.Gkh.Migrations.Version_2014031900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014031600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("B4_FIAS_ADDRESS_UID",
                new RefColumn("FIAS_ADDRESS_ID", "B4_FIAS_UID", "B4_FIAS_ADDRESS", "ID"),
                new Column("ICS_UID", DbType.String, 36),
                new Column("BILLING_ID", DbType.String, 25));
        }

        public override void Down()
        {
            Database.RemoveTable("B4_FIAS_ADDRESS_UID");
        }
    }
}