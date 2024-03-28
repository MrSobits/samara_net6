namespace Sobits.GisGkh.Migrations._2019.Version_1
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Map;

    [Migration("Version_1")]
    public class UpdateSchema : Migration
    {

        public override void Up()
        {
            Database.AddEntityTable(
                GisGkhRequestsMap.TableName,
                new Column("messageguid", DbType.String),
                new Column("requestermessageguid", DbType.String),
                new Column("isexport", DbType.Int32, ColumnProperty.NotNull),
                new Column("typerequest", DbType.Int32, ColumnProperty.NotNull),
                new Column("requeststate", DbType.Int32, ColumnProperty.NotNull),
                new Column("answer", DbType.String),
                new RefColumn("operator", $"{ GisGkhRequestsMap.TableName }_GKH_OPERATOR_ID", "GKH_OPERATOR", "ID"),
                new Column("reqdate", DbType.DateTime),
                new Column("dictionarynumber", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable(GisGkhRequestsMap.TableName);
        }

    }
}