namespace Bars.Gkh.Regions.Voronezh.Migrations._2016.Version_1
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Regions.Voronezh.Map.Dicts;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var zonalInspectionTable = "GKH_DICT_ZONAINSP";
            var zonalInspectionIdx = $"{ZonalInspectionPrefixMap.TableName}_TO_{zonalInspectionTable}";

            this.Database.AddEntityTable(
                ZonalInspectionPrefixMap.TableName,
                new Column(ZonalInspectionPrefixMap.ActCheckPrefix, DbType.String),
                new Column(ZonalInspectionPrefixMap.PrescriptionPrefix, DbType.String),
                new Column(ZonalInspectionPrefixMap.ProtocolPrefix, DbType.String),
                new RefColumn(ZonalInspectionPrefixMap.ZonalInspection, ColumnProperty.Unique, zonalInspectionIdx, zonalInspectionTable, "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable(ZonalInspectionPrefixMap.TableName);
        }
    }
}