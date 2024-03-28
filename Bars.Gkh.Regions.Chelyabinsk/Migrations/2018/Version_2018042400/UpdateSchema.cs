namespace Bars.Gkh.Regions.Chelyabinsk.Migrations._2018.Version_2018042400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018042400")]
    [MigrationDependsOn(typeof(Version_2018042200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName tableDesc = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" };
        public override void Up()
        {
            this.Database.AddColumn(tableDesc, "ExtractDate", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "ExtractNumber", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "HeadContent", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "Registrator", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "Appointment", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "NoShareHolding", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "RightAgainst", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "RightAssert", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "RightClaim", DbType.String, 1000);
            this.Database.AddColumn(tableDesc, "RightSteal", DbType.String, 1000);
        }

        public override void Down()
        {
            this.Database.RemoveColumn(tableDesc, "ExtractDate");
            this.Database.RemoveColumn(tableDesc, "ExtractNumber");
            this.Database.RemoveColumn(tableDesc, "HeadContent");
            this.Database.RemoveColumn(tableDesc, "Registrator");
            this.Database.RemoveColumn(tableDesc, "Appointment");
            this.Database.RemoveColumn(tableDesc, "NoShareHolding");
            this.Database.RemoveColumn(tableDesc, "RightAgainst");
            this.Database.RemoveColumn(tableDesc, "RightAssert");
            this.Database.RemoveColumn(tableDesc, "RightClaim");
            this.Database.RemoveColumn(tableDesc, "RightSteal");
        }
    }
}