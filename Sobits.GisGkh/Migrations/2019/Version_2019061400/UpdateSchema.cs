namespace Sobits.GisGkh.Migrations._2019.Version_2019061400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;

    [Migration("2019061400")]
    [MigrationDependsOn(typeof(Version_2019061300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn(
                NsiItemMap.TableName,
                new Column(nameof(NsiItem.IsActual).ToLower(), System.Data.DbType.Int32)
                );

            Database.AddRefColumn(
                GisGkhRequestsMap.TableName,
                new RefColumn("reqfileinfo", $"{ GisGkhRequestsMap.TableName }_FILE_INFO_REQ_ID", "B4_FILE_INFO", "ID")
                );

            Database.AddRefColumn(
                GisGkhRequestsMap.TableName,
                new RefColumn("respfileinfo", $"{ GisGkhRequestsMap.TableName }_FILE_INFO_RESP_ID", "B4_FILE_INFO", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveColumn(GisGkhRequestsMap.TableName, "respfileinfo");
            Database.RemoveColumn(GisGkhRequestsMap.TableName, "reqfileinfo");
            Database.RemoveColumn(NsiItemMap.TableName, nameof(NsiItem.IsActual).ToLower());
        }
    }
}