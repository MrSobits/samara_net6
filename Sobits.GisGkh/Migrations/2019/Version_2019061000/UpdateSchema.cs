namespace Sobits.GisGkh.Migrations._2019.Version_2019061000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;

    [Migration("2019061000")]
    [MigrationDependsOn(typeof(Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                NsiListMap.TableName,
                new Column(nameof(NsiList.GisGkhName).ToLower(), DbType.String),
                new Column(nameof(NsiList.GisGkhCode).ToLower(), DbType.String),
                new Column(nameof(NsiList.EntityName).ToLower(), DbType.String),
                new Column(nameof(NsiList.RefreshDate).ToLower(), DbType.DateTime),
                new Column(nameof(NsiList.MatchDate).ToLower(), DbType.DateTime),
                new Column(nameof(NsiList.ModifiedDate).ToLower(), DbType.DateTime));

            Database.AddEntityTable(
                NsiItemMap.TableName,
                new RefColumn(nameof(NsiItem.NsiList).ToLower(), $"{ NsiItemMap.TableName }_{ NsiListMap.TableName }_ID", $"{ NsiListMap.TableName }", nameof(NsiList.Id).ToLower()),
                new Column(nameof(NsiItem.GisGkhItemCode).ToLower(), DbType.String),
                new Column(nameof(NsiItem.GisGkhGUID).ToLower(), DbType.String),
                new Column(nameof(NsiItem.EntityItemId).ToLower(), DbType.Int64));
        }

        public override void Down()
        {
            Database.RemoveTable(NsiItemMap.TableName);
            Database.RemoveTable(NsiListMap.TableName);
        }
    }
}