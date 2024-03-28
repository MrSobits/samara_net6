namespace Sobits.GisGkh.Migrations._2020.Version_2020032400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2020032400")]
    [MigrationDependsOn(typeof(Version_2020032300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                 GisGkhRegionalProgramCRMap.TableName,
                 new Column(nameof(GisGkhRegionalProgramCR.GisGkhTransportGuid).ToLower(), DbType.String, 36),
                 new Column(nameof(GisGkhRegionalProgramCR.GisGkhGuid).ToLower(), DbType.String, 36),
                 new Column(nameof(GisGkhRegionalProgramCR.WorkWith).ToLower(), DbType.Boolean)
                 );

            Database.AddEntityTable(
                 GisGkhVersionRecordMap.TableName,
                 new RefColumn(nameof(GisGkhVersionRecord.GisGkhRegionalProgramCR).ToLower(), $"{ GisGkhVersionRecordMap.TableName }_GIS_PROGRAM_ID", "GIS_GKH_REGIONAL_PROGRAM_CR", "ID"),
                 new RefColumn(nameof(GisGkhVersionRecord.VersionRecord).ToLower(), $"{ GisGkhVersionRecordMap.TableName }_VERSION_REC_ID", "OVRHL_VERSION_REC", "ID"),
                 new RefColumn(nameof(GisGkhVersionRecord.VersionRecordStage1).ToLower(), $"{ GisGkhVersionRecordMap.TableName }_STAGE1_VERSION_ID", "OVRHL_STAGE1_VERSION", "ID"),
                 new Column(nameof(GisGkhVersionRecord.GisGkhTransportGuid).ToLower(), DbType.String, 36),
                 new Column(nameof(GisGkhVersionRecord.GisGkhGuid).ToLower(), DbType.String, 36)
                 );
        }

        public override void Down()
        {
            Database.RemoveTable(GisGkhVersionRecordMap.TableName);
            Database.RemoveTable(GisGkhRegionalProgramCRMap.TableName);
        }
    }
}