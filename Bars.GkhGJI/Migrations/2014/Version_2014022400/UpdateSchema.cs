namespace Bars.GkhGji.Migrations.Version_2014022400
{
    using System.Data;
    
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014022001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("PART", DbType.String, 50));
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("ARTICLE", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "PART");
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "ARTICLE");
        }
    }
}