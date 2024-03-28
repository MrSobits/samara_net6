namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014060502
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060502")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014060501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_REALEST_WORKPRICE",
                new RefColumn("REAL_ESTATE_ID", ColumnProperty.NotNull, "OVRHL_REALEST_WORKPRICE_RE", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("WORK_PRICE_ID", ColumnProperty.NotNull, "OVRHL_REALEST_WORKPRICE_WP", "OVRHL_DICT_WORK_PRICE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_REALEST_WORKPRICE");
        }
    }
}