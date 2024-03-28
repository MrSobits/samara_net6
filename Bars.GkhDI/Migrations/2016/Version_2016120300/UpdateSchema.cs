namespace Bars.GkhDi.Migrations.Version_2016120300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhDi.Map;

    /// <summary>
    /// Миграция добавления группы полей "Период внесения платы по договору"
    /// </summary>
    [Migration("2016120300")]
    [MigrationDependsOn(typeof(Version_2016111800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            var id = "ID";
            var manOrgTable = "DI_DISINFO";
            var realityObjTable = "DI_DISINFO_REALOBJ";

            var pathIdIdx = $"{DisclosureInfoEmptyFieldsMap.TableName}_{DisclosureInfoEmptyFieldsMap.PathId}";
            var manOrgIdx = $"{DisclosureInfoEmptyFieldsMap.TableName}_TO_{manOrgTable}";

            var roPathIdIdx = $"{DisclosureInfoRealityObjEmptyFieldsMap.TableName}_{DisclosureInfoRealityObjEmptyFieldsMap.PathId}";
            var realityObjIdx = $"{DisclosureInfoRealityObjEmptyFieldsMap.TableName}_TO_{realityObjTable}";

            this.Database.AddEntityTable(DisclosureInfoEmptyFieldsMap.TableName,
                new Column(DisclosureInfoEmptyFieldsMap.FieldName, DbType.String),
                new Column(DisclosureInfoEmptyFieldsMap.PathId, DbType.Int32),
                new RefColumn(DisclosureInfoEmptyFieldsMap.ManOrg, manOrgIdx, manOrgTable, id));

            this.Database.AddIndex(pathIdIdx, false, DisclosureInfoEmptyFieldsMap.TableName, DisclosureInfoEmptyFieldsMap.PathId);

            this.Database.AddEntityTable(DisclosureInfoRealityObjEmptyFieldsMap.TableName,
                new Column(DisclosureInfoRealityObjEmptyFieldsMap.FieldName, DbType.String),
                new Column(DisclosureInfoRealityObjEmptyFieldsMap.PathId, DbType.Int32),
                new RefColumn(DisclosureInfoRealityObjEmptyFieldsMap.RealityObj, realityObjIdx, realityObjTable, id));

            this.Database.AddIndex(roPathIdIdx, false, DisclosureInfoRealityObjEmptyFieldsMap.TableName, DisclosureInfoRealityObjEmptyFieldsMap.PathId);
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable(DisclosureInfoEmptyFieldsMap.TableName);
            this.Database.RemoveTable(DisclosureInfoRealityObjEmptyFieldsMap.TableName);
        }
    }
}
