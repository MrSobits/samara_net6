namespace Sobits.GisGkh.Migrations._2019.Version_2019072200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2019072200")]
    [MigrationDependsOn(typeof(Version_2019061400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                GisGkhRequestsFileMap.TableName,
                new Column(nameof(GisGkhRequestsFile.GisGkhFileType).ToLower(), DbType.Int32, ColumnProperty.NotNull),
                new RefColumn(nameof(GisGkhRequestsFile.GisGkhRequests).ToLower(), $"{ GisGkhRequestsFileMap.TableName }_{ GisGkhRequestsMap.TableName }_ID", GisGkhRequestsMap.TableName, "ID"),
                new RefColumn(nameof(GisGkhRequestsFile.FileInfo).ToLower(), $"{ GisGkhRequestsFileMap.TableName }_FILE_INFO_REQ_ID", "B4_FILE_INFO", "ID"));

            Database.RemoveColumn(GisGkhRequestsMap.TableName, "reqfileinfo");
            Database.RemoveColumn(GisGkhRequestsMap.TableName, "respfileinfo");

        }

        public override void Down()
        {
            Database.AddRefColumn(
                GisGkhRequestsMap.TableName,
                new RefColumn("respfileinfo", $"{ GisGkhRequestsMap.TableName }_FILE_INFO_RESP_ID", "B4_FILE_INFO", "ID")
                );

            Database.AddRefColumn(
                GisGkhRequestsMap.TableName,
                new RefColumn("reqfileinfo", $"{ GisGkhRequestsMap.TableName }_FILE_INFO_REQ_ID", "B4_FILE_INFO", "ID")
                );

            Database.RemoveTable(GisGkhRequestsFileMap.TableName);
        }
    }
}