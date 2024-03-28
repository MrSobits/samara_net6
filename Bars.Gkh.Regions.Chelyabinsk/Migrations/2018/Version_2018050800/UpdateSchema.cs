namespace Bars.Gkh.Regions.Chelyabinsk.Migrations._2018.Version_2018050800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018050800")]
    [MigrationDependsOn(typeof(Version_2018042400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName tableDesc = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" };
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"ALTER TABLE import.rosregextractdesc ADD xml xml;");
        }

        public override void Down()
        {
            this.Database.RemoveColumn(tableDesc, "XML");
        }
    }
}