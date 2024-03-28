namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015052600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015041800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_DICT_SURVEY_SUBJ_REQ",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));

            this.Database.AddEntityTable(
                "GJI_DICT_RES_VIOL_CLAIM",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_SURVEY_SUBJ_REQ");
            this.Database.RemoveTable("GJI_DICT_RES_VIOL_CLAIM");
        }
    }
}
