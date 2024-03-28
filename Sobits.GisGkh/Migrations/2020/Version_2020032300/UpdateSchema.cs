namespace Sobits.GisGkh.Migrations._2020.Version_2020032300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2020032300")]
    [MigrationDependsOn(typeof(Version_2020030300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumn(
                GisGkhRequestsMap.TableName,
                new Column("answer", DbType.String, 100000)
                );
        }

        public override void Down()
        {
           
        }
    }
}