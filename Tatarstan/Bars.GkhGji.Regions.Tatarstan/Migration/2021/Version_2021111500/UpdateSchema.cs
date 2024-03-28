namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021111500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh;

    [Migration("2021111500")]
    [MigrationDependsOn(typeof(Version_2021031900.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2021.Version_2021111300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            new []
            {
                "CreateFunctionGetDisposalRobjectAddress",
                "CreateFunctionGetActcheckRobjectAddress",
                "CreateViewDisposal",
                "CreateViewActcheck",
                "CreateViewActRemoval",
                "CreateViewPrescription",
                "CreateViewProtocol",
                "CreateViewResolution"
            }.ForEach(x =>
            {
                ViewManager.Create(this.Database, "GkhGjiTatarstan", x);
            });
        }
    }
}