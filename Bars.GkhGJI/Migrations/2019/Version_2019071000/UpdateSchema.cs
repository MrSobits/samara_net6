namespace Bars.GkhGji.Migrations._2019.Version_2019071000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019071000")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019052300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
          
            Database.AddEntityTable("GJI_DICT_PROSECUTOR_OFFICE",
                 new Column("CODE", DbType.String, 15),
                 new Column("FC_CODE", DbType.String, 5),
                 new Column("LA_CODE", DbType.String, 10),
                 new Column("REGION", DbType.String, 5),
                 new Column("NAME", DbType.String, 500));
        }

        public override void Down()
        {           
            Database.RemoveTable("GJI_DICT_PROSECUTOR_OFFICE");
        }
    }
}