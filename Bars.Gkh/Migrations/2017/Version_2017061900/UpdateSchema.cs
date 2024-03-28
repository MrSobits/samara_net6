namespace Bars.Gkh.Migrations._2017.Version_2017061900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017061900")]
    [MigrationDependsOn(typeof(Version_2017061500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", "MONUMENT_DOCUMENT_NUMBER", DbType.String);
            this.Database.AddRefColumn("GKH_REALITY_OBJECT", new FileColumn("MONUMENT_FILE", "GKH_REALITY_OBJECT_MONUMENT_FILE"));
            this.Database.AddColumn("GKH_REALITY_OBJECT", "MONUMENT_DEPARTMENT_NAME", DbType.String);
        }

        public override void Down()
        { 
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "MONUMENT_DOCUMENT_NUMBER");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "MONUMENT_FILE");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "MONUMENT_DEPARTMENT_NAME");
        }
    }
}