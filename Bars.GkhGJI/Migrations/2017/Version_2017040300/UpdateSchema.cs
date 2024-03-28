namespace Bars.GkhGji.Migrations._2017.Version_2017040300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    /// <summary>
    /// Миграция 2017040300
    /// </summary>
    [Migration("2017040300")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2015.Version_2015121400.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2016.Version_2016111800.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017022100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetDocumentCountViolationByViolStage");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetDocumentCountRobjectByViolStage");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetDocumentCountChildDocument");
            ViewManager.Create(this.Database, "GkhGji", "CreateFuncGetDocumentParentCountRobjectByViolStage");
            ViewManager.Create(this.Database, "GkhGji", "CreateFuncGetInspectionCountRobject");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetDisposalCountRobject");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetDisposalActCheckExist");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActsurveyRobjectMuId");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActcheckHasViolation");
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActcheckCountRobject");
        }
    }
}