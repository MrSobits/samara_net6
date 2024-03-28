namespace Bars.GkhGji.Migrations._2014.Version_2014111400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014102400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_FEATUREVIOL", new Column("FULL_NAME", DbType.String, 2000));
           
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_FEATUREVIOL", "GJI_DICT_FEATUREVIOL");
        }
    }
}
