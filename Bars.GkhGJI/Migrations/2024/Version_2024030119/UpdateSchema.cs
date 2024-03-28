namespace Bars.GkhGji.Migrations._2024.Version_2024030119
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030119")]
    [MigrationDependsOn(typeof(Version_2024030118.UpdateSchema))]
    /// Является Version_2021100600 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteViewAppealCits");
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewAppealCits");
        }
    }
}