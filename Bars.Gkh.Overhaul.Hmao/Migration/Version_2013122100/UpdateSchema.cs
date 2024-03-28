namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121702.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_SHARE_FINANC_CEO",
                new Column("CEO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CSHARE", DbType.Decimal, ColumnProperty.NotNull));

            Database.AddIndex("IND_OVRHL_SHAREFINCEO_C", false, "OVRHL_SHARE_FINANC_CEO", "CEO_ID");
            Database.AddForeignKey("FK_OVRHL_SHAREFINCEO_C", "OVRHL_SHARE_FINANC_CEO", "CEO_ID", "OVRHL_COMMON_ESTATE_OBJECT", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SHARE_FINANC_CEO");
        }
    }
}