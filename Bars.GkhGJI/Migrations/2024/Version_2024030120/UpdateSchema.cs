namespace Bars.GkhGji.Migrations._2024.Version_2024030120
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030120")]
    [MigrationDependsOn(typeof(Version_2024030119.UpdateSchema))]
    /// Является Version_2021111300 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            //new[]
            //{
            //    "DeleteViewDisposal",
            //    "DeleteViewActcheck",
            //    "DeleteViewActRemoval",
            //    "DeleteViewPrescription",
            //    "DeleteViewProtocol",
            //    "DeleteViewResolution"
            //}.ForEach(x =>
            //{
            //    ViewManager.Drop(this.Database, "GkhGji", x);
            //});

            //new[]
            //{
            //    "CreateViewDisposal",
            //    "CreateViewActcheck",
            //    "CreateViewActRemoval",
            //    "CreateViewPrescription",
            //    "CreateViewProtocol",
            //    "CreateViewResolution"
            //}.ForEach(x =>
            //{
            //    ViewManager.Create(this.Database, "GkhGji", x);
            //});
        }
    }
}