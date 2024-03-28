namespace Sobits.GisGkh.Migrations._2019.Version_2019111100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2019111100")]
    [MigrationDependsOn(typeof(Version_2019072900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumn(
                GisGkhRequestsMap.TableName,
                new Column("answer", DbType.String, 1000)
                );
            Database.RemoveColumn(
                GisGkhRequestsMap.TableName,
                "isexport"
                );
            Database.RemoveColumn(
                GisGkhRequestsMap.TableName,
                "reqdate"
                );
            Database.AddRefColumn(
                NsiItemMap.TableName,
                new RefColumn("parentitem", $"{ NsiItemMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", "id")
                );
        }

        public override void Down()
        {
            Database.RemoveColumn(
                  NsiItemMap.TableName,
                  "parentitem"
                  );
            Database.AddColumn(
                 GisGkhRequestsMap.TableName,
                 new Column("reqdate", DbType.DateTime)
                 );
            Database.AddColumn(
                 GisGkhRequestsMap.TableName,
                 new Column("isexport", DbType.Int32, ColumnProperty.NotNull, 10)
                 );
        }
    }
}