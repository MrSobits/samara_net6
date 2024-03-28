namespace Sobits.GisGkh.Migrations._2019.Version_2019072400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2019072400")]
    [MigrationDependsOn(typeof(Version_2019072200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                GisGkhPremisesMap.TableName,
                new RefColumn(nameof(GisGkhPremises.RealityObject).ToLower(), $"{ GisGkhPremisesMap.TableName }_GKH_REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID"),
                new Column(nameof(GisGkhPremises.RoomType).ToLower(), DbType.Int16, ColumnProperty.NotNull),
                new Column(nameof(GisGkhPremises.PremisesNum).ToLower(), DbType.String),
                new Column(nameof(GisGkhPremises.Floor).ToLower(), DbType.String),
                new Column(nameof(GisGkhPremises.TotalArea).ToLower(), DbType.Decimal),
                new Column(nameof(GisGkhPremises.GrossArea).ToLower(), DbType.Decimal),
                new Column(nameof(GisGkhPremises.ModificationDate).ToLower(), DbType.DateTime, ColumnProperty.NotNull),
                new Column(nameof(GisGkhPremises.TerminationDate).ToLower(), DbType.DateTime),
                new Column(nameof(GisGkhPremises.IsCommonProperty).ToLower(), DbType.Boolean),
                new Column(nameof(GisGkhPremises.PremisesUniqueNumber).ToLower(), DbType.String, ColumnProperty.NotNull),
                new Column(nameof(GisGkhPremises.PremisesGUID).ToLower(), DbType.String, ColumnProperty.NotNull),
                new Column(nameof(GisGkhPremises.EntranceNum).ToLower(), DbType.String),
                new Column(nameof(GisGkhPremises.CadastralNumber).ToLower(), DbType.String),
                new Column(nameof(GisGkhPremises.No_RSO_GKN_EGRP_Data).ToLower(), DbType.Boolean),
                new Column(nameof(GisGkhPremises.No_RSO_GKN_EGRP_Registered).ToLower(), DbType.Boolean));
        }

        public override void Down()
        {
            Database.RemoveTable(GisGkhPremisesMap.TableName);
        }
    }
}