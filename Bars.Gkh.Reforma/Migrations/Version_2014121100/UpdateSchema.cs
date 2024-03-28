namespace Bars.Gkh.Reforma.Migrations.Version_2014121100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014120200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.Delete("RFRM_REALITY_OBJECT", "1 = 1");
            Database.Delete("RFRM_MANAGING_ORG", "1 = 1");

            Database.RemoveColumn("RFRM_MANAGING_ORG", "MAN_ORG_ID");
            Database.AddColumn("RFRM_MANAGING_ORG", new Column("INN", DbType.String, ColumnProperty.NotNull | ColumnProperty.Unique));
        }

        public override void Down()
        {
            Database.Delete("RFRM_REALITY_OBJECT", "1 = 1");
            Database.Delete("RFRM_MANAGING_ORG", "1 = 1");

            Database.RemoveColumn("RFRM_MANAGING_ORG", "INN");
            Database.AddRefColumn("RFRM_MANAGING_ORG", new RefColumn("MAN_ORG_ID", ColumnProperty.NotNull, "RFRM_MAN_ORG", "GKH_MANAGING_ORGANIZATION", "ID"));
        }
    }
}