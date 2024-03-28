namespace Bars.GkhGji.Regions.Zabaykalye.Migrations.Version_2014080600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Zabaykalye.Migrations.Version_2014080400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_DICT_SURVEY_SUBJ", new Column("NAME", DbType.String, 500));
        }

        public override void Down()
        {
            Database.ChangeColumn("GJI_DICT_SURVEY_SUBJ", new Column("NAME", DbType.String, 300));
        }
    }
}