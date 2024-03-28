namespace Bars.Gkh.Reforma.Migrations.Version_2014112600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014112500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("RFRM_DICT_OKOPF");
        }

        public override void Down()
        {
            Database.AddEntityTable(
                "RFRM_DICT_OKOPF",
                new Column("OKOPF", DbType.Int16, ColumnProperty.Null),
                new RefColumn("ORG_FORM_ID", ColumnProperty.NotNull, "RFRM_OKOPF_ORG_FORM", "GKH_DICT_ORG_FORM", "ID"));
        }
    }
}