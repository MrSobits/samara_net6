namespace Bars.GkhGji.Migrations._2024.Version_2024030138
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030138")]
    [MigrationDependsOn(typeof(Version_2024030137.UpdateSchema))]
    /// Является Version_2023021500 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteViewDisposal");
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewDisposal");
        }
    }
}