namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014042500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014041500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_APPCIT_ANS_ADDR",
                new RefColumn("REVENUE_SOURCE_ID", ColumnProperty.NotNull, "GJI_APPCIT_ANS_ADDR_RS", "GJI_DICT_REVENUESOURCE", "ID"),
                new RefColumn("ANSWER_ID", ColumnProperty.NotNull, "GJI_APPCIT_ANS_ADDR_A", "GJI_APPCIT_ANSWER", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_ANS_ADDR");
        }
    }
}