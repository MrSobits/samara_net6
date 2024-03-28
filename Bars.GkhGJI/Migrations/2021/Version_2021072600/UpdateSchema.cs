namespace Bars.GkhGji.Migrations._2021.Version_2021072600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2021072600")]
    [MigrationDependsOn(typeof(Version_2021072100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetDocumentArticleLaw");
        }
        public override void Down()
        {
         
        }
    }
}