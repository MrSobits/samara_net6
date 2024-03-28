namespace Bars.GkhGji.Regions.Tyumen.Migrations.Version_2015021700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021700")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_APPLICANT_NOTIFY",
                new Column("CODE", DbType.Int32, ColumnProperty.NotNull),
                new Column("EMAIL_SUBJECT", DbType.String, 100, ColumnProperty.NotNull),
                new Column("EMAIL_TEMPLATE", DbType.String, 3000, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_APPLICANT_NOTIFY");
        }
    }
}