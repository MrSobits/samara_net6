namespace Bars.Gkh.Regions.Voronezh.Migrations._2018.Version_2018051400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018051400")]
    [MigrationDependsOn(typeof(Version_2018050800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName tableTmpId = new SchemaQualifiedObjectName { Name = "tmp_clw_id", Schema = "IMPORT" };
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"CREATE TABLE import.tmp_clw_id (account_id integer)");
        }

        public override void Down()
        {
            this.Database.RemoveTable(tableTmpId);
        }
    }
}