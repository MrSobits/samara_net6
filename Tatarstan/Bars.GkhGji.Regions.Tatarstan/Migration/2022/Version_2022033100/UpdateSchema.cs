namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022033100
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022033100")]
    [MigrationDependsOn(typeof(Version_2022032100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_DICT_PROSECUTOR_OFFICE";
        private const string ColumnName = "ERKNM_CODE";

        public override void Up()
        {
            this.Database.AddColumn(TableName, new Column(ColumnName, DbType.String, 10));

            var codesDict = new Dictionary<string, string>
            {
                {"1000000000", "2" },
                {"1050160000", "1267" },
                {"1050160001", "1268" },
                {"1050160002", "1269" },
                {"1050160003", "1270" },
                {"1050160004", "1271" },
                {"1050160005", "1272" },
                {"1050160006", "1273" },
                {"1050160007", "1274" },
                {"1050160008", "1275" },
                {"1050160009", "1276" },
                {"1050160010", "1277" },
                {"1050160011", "1278" },
                {"1050160012", "1279" },
                {"1050160013", "1280" },
                {"1050160014", "1281" },
                {"1050160015", "1282" },
                {"1050160016", "1283" },
                {"1050160017", "1284" },
                {"1050160018", "1285" },
                {"1050160019", "1286" },
                {"1050160020", "1287" },
                {"1050160021", "1288" },
                {"1050160022", "1289" },
                {"1050160023", "1290" },
                {"1050160024", "1291" },
                {"1050160025", "1292" },
                {"1050160026", "1293" },
                {"1050160027", "1294" },
                {"1050160028", "1295" },
                {"1050160029", "1296" },
                {"1050160030", "1297" },
                {"1050160031", "1298" },
                {"1050160032", "1299" },
                {"1050160033", "1300" },
                {"1050160034", "1301" },
                {"1050160035", "1302" },
                {"1050160036", "1303" },
                {"1050160037", "1304" },
                {"1050160038", "1305" },
                {"1050160039", "1306" },
                {"1050160040", "1307" },
                {"1050160041", "1308" },
                {"1050160042", "1309" },
                {"1050160043", "1310" },
                {"1050160044", "1311" },
                {"1050160045", "1312" },
                {"1050160046", "1313" },
                {"1050160047", "1314" },
                {"1050160048", "1315" },
                {"1050160049", "1316" },
                {"1050160050", "1317" },
                {"1050160051", "1318" },
                {"1050160052", "1319" },
                {"1050160053", "1320" },
                {"1050160054", "1321" },
                {"1050160055", "1322" },
                {"1050160056", "1323" }
            };

            foreach (var item in codesDict)
            {
                this.Database.Update(TableName, new[] { ColumnName }, new[] { item.Value }, whereSql: $"code = '{item.Key}'");
            }
        }

        public override void Down()
        {
            this.Database.RemoveColumn(TableName, ColumnName);
        }
    }
}
