namespace Bars.GkhGji.Migrations._2016.Version_2016111800
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;
    using System.Text;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Map;
    using Bars.GkhGji.Map.ResolutionRospotrebnadzor;

    /// <summary>
    /// Миграция 2016111800
    /// </summary>
    [Migration("2016111800")]
    [MigrationDependsOn(typeof(Version_2016101300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            if (!this.Database.ColumnExists("GJI_ACTCHECK", "ACT_TO_ROSPOTREBNADZOR"))
            {
                this.Database.AddColumn("GJI_ACTCHECK", new Column("ACT_TO_ROSPOTREBNADZOR", DbType.Int32, (int)YesNo.No));
                this.Database.ExecuteQuery($"UPDATE GJI_ACTCHECK SET ACT_TO_ROSPOTREBNADZOR = {(int)YesNo.No};");
            }

            this.CreateResolutionRospotrebnadzor();
            this.CreateResolutionRospotrebnadzorAnnex();
            this.CreateResolutionRospotrebnadzorDefinition();
            this.CreateResolutionRospotrebnadzorDispute();
            this.CreateResolutionRospotrebnadzorPayFine();
            this.CreateResolutionRospotrebnadzorViolation();
            this.CreateResolutionRospotrebnadzorArticleLaw();
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            if (this.Database.ColumnExists("GJI_ACTCHECK", "ACT_TO_ROSPOTREBNADZOR"))
            {
                this.Database.RemoveColumn("GJI_ACTCHECK", "ACT_TO_ROSPOTREBNADZOR");
            }

            ViewManager.Drop(this.Database, "GkhGji");

            this.Database.RemoveTable(ResolutionRospotrebnadzorAnnexMap.TableName);
            this.Database.RemoveTable(ResolutionRospotrebnadzorDefinitionMap.TableName);
            this.Database.RemoveTable(ResolutionRospotrebnadzorDisputeMap.TableName);
            this.Database.RemoveTable(ResolutionRospotrebnadzorPayFineMap.TableName);
            this.Database.RemoveTable(ResolutionRospotrebnadzorViolationMap.TableName);
            this.Database.RemoveTable(ResolutionRospotrebnadzorArticleLawMap.TableName);

            this.Database.RemoveTable(ResolutionRospotrebnadzorMap.TableName);
        }

        private void CreateResolutionRospotrebnadzor()
        {
            var id = "ID";
            var gjiTable = "GJI_DOCUMENT";
            var municipalityTable = "GKH_DICT_MUNICIPALITY";
            var officialTable = "GKH_DICT_INSPECTOR";
            var sanctionTable = "GJI_DICT_SANCTION";
            var executantTable = "GJI_DICT_EXECUTANT";
            var contragentTable = "GKH_CONTRAGENT";

            var index = $"{ResolutionRospotrebnadzorMap.TableName}_ID_TO_{gjiTable}";
            var fineMunicipalityIdx = $"{ResolutionRospotrebnadzorMap.TableName}_TO_{municipalityTable}";
            var officialIdx = $"{ResolutionRospotrebnadzorMap.TableName}_TO_{officialTable}";
            var locationIdx = $"{ResolutionRospotrebnadzorMap.TableName}_LOCATION_TO_{municipalityTable}";
            var sanctionIdx = $"{ResolutionRospotrebnadzorMap.TableName}_TO_{sanctionTable}";
            var executantIdx = $"{ResolutionRospotrebnadzorMap.TableName}_TO_{executantTable}";
            var contragentIdx = $"{ResolutionRospotrebnadzorMap.TableName}_TO_{contragentTable}";

            this.Database.AddTable(ResolutionRospotrebnadzorMap.TableName,
                new Column(id, DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column(ResolutionRospotrebnadzorMap.DocumentReason, DbType.String, 150),
                new Column(ResolutionRospotrebnadzorMap.DeliveryDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorMap.GisUin, DbType.String, 50),
                new Column(ResolutionRospotrebnadzorMap.RevocationReason, DbType.String),
                new Column(ResolutionRospotrebnadzorMap.TypeInitiativeOrg, DbType.Int32, ColumnProperty.NotNull, (int)TypeInitiativeOrgGji.Rospotrebnadzor),
                new Column(ResolutionRospotrebnadzorMap.ExpireReason, DbType.Int32),
                new Column(ResolutionRospotrebnadzorMap.PenaltyAmount, DbType.Decimal),
                new Column(ResolutionRospotrebnadzorMap.SspDocumentNum, DbType.String),
                new Column(ResolutionRospotrebnadzorMap.Paided, DbType.Int32, ColumnProperty.NotNull, (int)YesNoNotSet.NotSet),
                new Column(ResolutionRospotrebnadzorMap.TransferToSspDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorMap.PhysicalPerson, DbType.String),
                new Column(ResolutionRospotrebnadzorMap.PhysicalPersonInfo, DbType.String),
                new RefColumn(ResolutionRospotrebnadzorMap.FineMunicipality, fineMunicipalityIdx, municipalityTable, id),
                new RefColumn(ResolutionRospotrebnadzorMap.Official, officialIdx, officialTable, id),
                new RefColumn(ResolutionRospotrebnadzorMap.LocationMunicipality, locationIdx, municipalityTable, id),
                new RefColumn(ResolutionRospotrebnadzorMap.Sanction, sanctionIdx, sanctionTable, id),
                new RefColumn(ResolutionRospotrebnadzorMap.Executant, executantIdx, executantTable, id),
                new RefColumn(ResolutionRospotrebnadzorMap.Contragent, contragentIdx, contragentTable, id));

            this.Database.AddForeignKey($"FK_{index}", ResolutionRospotrebnadzorMap.TableName, id, gjiTable, id);
            this.Database.AddForeignKey($"FK_{fineMunicipalityIdx}", ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.FineMunicipality, municipalityTable, id);
            this.Database.AddForeignKey($"FK_{officialIdx}", ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Official, officialTable, id);
            this.Database.AddForeignKey($"FK_{locationIdx}", ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.LocationMunicipality, municipalityTable, id);
            this.Database.AddForeignKey($"FK_{sanctionIdx}", ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Sanction, sanctionTable, id);
            this.Database.AddForeignKey($"FK_{executantIdx}", ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Executant, executantTable, id);
            this.Database.AddForeignKey($"FK_{contragentIdx}", ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Contragent, contragentTable, id);

            this.Database.AddIndex($"IND_{index}", false, ResolutionRospotrebnadzorMap.TableName, id);
            this.Database.AddIndex($"IND_{fineMunicipalityIdx}", false, ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.FineMunicipality);
            this.Database.AddIndex($"IND_{officialIdx}", false, ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Official);
            this.Database.AddIndex($"IND_{locationIdx}", false, ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.LocationMunicipality);
            this.Database.AddIndex($"IND_{sanctionIdx}", false, ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Sanction);
            this.Database.AddIndex($"IND_{executantIdx}", false, ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Executant);
            this.Database.AddIndex($"IND_{contragentIdx}", false, ResolutionRospotrebnadzorMap.TableName, ResolutionRospotrebnadzorMap.Contragent);
        }

        private void CreateResolutionRospotrebnadzorAnnex()
        {
            var id = "ID";
            var fileTable = "B4_FILE_INFO";
            var resolutionTable = ResolutionRospotrebnadzorMap.TableName;

            var fileIdx = $"{ResolutionRospotrebnadzorAnnexMap.TableName}_TO_{fileTable}";
            var resolutionIdx = $"{ResolutionRospotrebnadzorAnnexMap.TableName}_TO_{resolutionTable}";

            this.Database.AddEntityTable(ResolutionRospotrebnadzorAnnexMap.TableName,
                new Column(ResolutionRospotrebnadzorAnnexMap.DocumentName, DbType.String),
                new Column(ResolutionRospotrebnadzorAnnexMap.DocumentDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorAnnexMap.Description, DbType.String),
                new RefColumn(ResolutionRospotrebnadzorAnnexMap.File, fileIdx, fileTable, id),
                new RefColumn(ResolutionRospotrebnadzorAnnexMap.Resolution, resolutionIdx, resolutionTable, id));
        }

        private void CreateResolutionRospotrebnadzorDefinition()
        {
            var id = "ID";
            var issuedDefinitionTable = "GKH_DICT_INSPECTOR";
            var resolutionTable = ResolutionRospotrebnadzorMap.TableName;

            var issuedDefinitionIdx = $"{ResolutionRospotrebnadzorDefinitionMap.TableName}_TO_{issuedDefinitionTable}";
            var resolutionIdx = $"{ResolutionRospotrebnadzorDefinitionMap.TableName}_TO_{resolutionTable}";

            this.Database.AddEntityTable(ResolutionRospotrebnadzorDefinitionMap.TableName,
                new Column(ResolutionRospotrebnadzorDefinitionMap.DocumentNum, DbType.String, 50),
                new Column(ResolutionRospotrebnadzorDefinitionMap.DocumentDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorDefinitionMap.TypeDefinition, DbType.Int32, ColumnProperty.NotNull, (int)TypeDefinitionResolution.Deferment),
                new Column(ResolutionRospotrebnadzorDefinitionMap.ExecutionDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorDefinitionMap.Description, DbType.String),
                new RefColumn(ResolutionRospotrebnadzorDefinitionMap.IssuedDefinition, issuedDefinitionIdx, issuedDefinitionTable, id),
                new RefColumn(ResolutionRospotrebnadzorDefinitionMap.Resolution, resolutionIdx, resolutionTable, id));
        }

        private void CreateResolutionRospotrebnadzorDispute()
        {
            var id = "ID";
            var fileTable = "B4_FILE_INFO";
            var courtTable = "GJI_DICT_COURT";
            var instanceTable = "GJI_DICT_INSTANCE";
            var courtVerdictTable = "GJI_DICT_COURTVERDICT";
            var lawyerTable = "GKH_DICT_INSPECTOR";
            var resolutionTable = ResolutionRospotrebnadzorMap.TableName;

            var fileIdx = $"{ResolutionRospotrebnadzorDisputeMap.TableName}_TO_{fileTable}";
            var courtIdx = $"{ResolutionRospotrebnadzorDisputeMap.TableName}_TO_{courtTable}";
            var instanceIdx = $"{ResolutionRospotrebnadzorDisputeMap.TableName}_TO_{instanceTable}";
            var courtVerdictIdx = $"{ResolutionRospotrebnadzorDisputeMap.TableName}_TO_{courtVerdictTable}";
            var lawyerIdx = $"{ResolutionRospotrebnadzorDisputeMap.TableName}_TO_{lawyerTable}";
            var resolutionIdx = $"{ResolutionRospotrebnadzorDisputeMap.TableName}_TO_{resolutionTable}";

            this.Database.AddEntityTable(ResolutionRospotrebnadzorDisputeMap.TableName,
                new Column(ResolutionRospotrebnadzorDisputeMap.DocumentNum, DbType.String, 50),
                new Column(ResolutionRospotrebnadzorDisputeMap.DocumentDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorDisputeMap.ProsecutionProtest, DbType.Boolean, ColumnProperty.NotNull, false),
                new Column(ResolutionRospotrebnadzorDisputeMap.Description, DbType.String),
                new Column(ResolutionRospotrebnadzorDisputeMap.Appeal, DbType.Int32),
                new RefColumn(ResolutionRospotrebnadzorDisputeMap.File, fileIdx, fileTable, id),
                new RefColumn(ResolutionRospotrebnadzorDisputeMap.Court, courtIdx, courtTable, id),
                new RefColumn(ResolutionRospotrebnadzorDisputeMap.Instance, instanceIdx, instanceTable, id),
                new RefColumn(ResolutionRospotrebnadzorDisputeMap.CourtVerdict, courtVerdictIdx, courtVerdictTable, id),
                new RefColumn(ResolutionRospotrebnadzorDisputeMap.Lawyer, lawyerIdx, lawyerTable, id),
                new RefColumn(ResolutionRospotrebnadzorDisputeMap.Resolution, resolutionIdx, resolutionTable, id));
        }

        private void CreateResolutionRospotrebnadzorPayFine()
        {
            var id = "ID";
            var resolutionTable = ResolutionRospotrebnadzorMap.TableName;

            var resolutionIdx = $"{ResolutionRospotrebnadzorPayFineMap.TableName}_TO_{resolutionTable}";

            this.Database.AddEntityTable(ResolutionRospotrebnadzorPayFineMap.TableName,
                new Column(ResolutionRospotrebnadzorPayFineMap.TypeDocumentPaid, DbType.Int32),
                new Column(ResolutionRospotrebnadzorPayFineMap.DocumentDate, DbType.DateTime),
                new Column(ResolutionRospotrebnadzorPayFineMap.DocumentNum, DbType.String, 50),
                new Column(ResolutionRospotrebnadzorPayFineMap.Amount, DbType.Decimal),
                new Column(ResolutionRospotrebnadzorPayFineMap.GisUip, DbType.String, 50),
                new RefColumn(ResolutionRospotrebnadzorPayFineMap.Resolution, resolutionIdx, resolutionTable, id));
        }

        private void CreateResolutionRospotrebnadzorViolation()
        {
            var id = "ID";
            var violationTable = "GJI_ACTCHECK_VIOLAT";
            var resolutionTable = ResolutionRospotrebnadzorMap.TableName;

            var violationIdx = $"{ResolutionRospotrebnadzorViolationMap.TableName}_TO_{violationTable}";
            var resolutionIdx = $"{ResolutionRospotrebnadzorViolationMap.TableName}_TO_{resolutionTable}";

            this.Database.AddEntityTable(ResolutionRospotrebnadzorViolationMap.TableName,
                new Column(ResolutionRospotrebnadzorViolationMap.Description, DbType.String),
                new RefColumn(ResolutionRospotrebnadzorViolationMap.Violation, violationIdx, violationTable, id),
                new RefColumn(ResolutionRospotrebnadzorViolationMap.Resolution, resolutionIdx, resolutionTable, id));
        }

        private void CreateResolutionRospotrebnadzorArticleLaw()
        {
            var id = "ID";
            var articleLawTable = "GJI_DICT_ARTICLELAW";
            var resolutionTable = ResolutionRospotrebnadzorMap.TableName;

            var articleLawIdx = $"{ResolutionRospotrebnadzorArticleLawMap.TableName}_TO_{articleLawTable}";
            var resolutionIdx = $"{ResolutionRospotrebnadzorArticleLawMap.TableName}_TO_{resolutionTable}";

            this.Database.AddEntityTable(ResolutionRospotrebnadzorArticleLawMap.TableName,
                new Column(ResolutionRospotrebnadzorArticleLawMap.Description, DbType.String),
                new RefColumn(ResolutionRospotrebnadzorArticleLawMap.ArticleLaw, articleLawIdx, articleLawTable, id),
                new RefColumn(ResolutionRospotrebnadzorArticleLawMap.Resolution, resolutionIdx, resolutionTable, id));
        }
    }
}