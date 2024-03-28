namespace Bars.GkhGji.Migrations._2020.Version_2020021400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020021400")]
    [MigrationDependsOn(typeof(Version_2020011500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
          
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("KBK", DbType.String));
        }

        public override void Down()
        {
           
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "KBK");
        }
    }
}