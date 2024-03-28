namespace Bars.GkhGji.Migrations._2015.Version_2015100200
{
	using System.Data;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015093001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_DICT_SURVEY_SUBJ", new Column("NAME", DbType.String, 2000));
        }

        public override void Down()
        {
        }
    }
}