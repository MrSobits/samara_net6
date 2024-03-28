namespace Sobits.GisGkh.Migrations._2019.Version_2019072900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2019072900")]
    [MigrationDependsOn(typeof(Version_2019072400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn(
                NsiListMap.TableName,
                new Column(nameof(NsiList.ListGroup).ToLower(), DbType.Int32)
                );
        }

        public override void Down()
        {
            Database.RemoveColumn(NsiListMap.TableName, nameof(NsiList.ListGroup).ToLower());
        }
    }
}