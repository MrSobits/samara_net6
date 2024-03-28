namespace Bars.GkhGji.Migrations._2024.Version_2024030139
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030139")]
    [MigrationDependsOn(typeof(Version_2024030138.UpdateSchema))]
    /// Является Version_2023030600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddPrimaryKey("gji_inspection_statement_pkey", "gji_inspection_statement", "id");

            //ViewManager.Drop(this.Database, "GkhGji", "DeleteViewInsStatement");
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewInsStatement");
        }
    }
}