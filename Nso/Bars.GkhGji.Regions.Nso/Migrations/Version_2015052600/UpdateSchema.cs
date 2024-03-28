namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015052600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015041800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_SURVEY_SUBJ_REQ",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));

            Database.AddEntityTable(
                "GJI_DICT_RES_VIOL_CLAIM",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_SURVEY_SUBJ_REQ");
            Database.RemoveTable("GJI_DICT_RES_VIOL_CLAIM");
        }
    }
}
