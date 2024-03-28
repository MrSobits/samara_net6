namespace Bars.GisIntegration.Base.Migrations.Version_2016101400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016101400")]
    [MigrationDependsOn(typeof(Version_2016101100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GI_TASK", new RefColumn("DATA_SUPPLIER_ID", ColumnProperty.Null, "TASK_DATA_SUP", "GI_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GI_TASK", "DATA_SUPPLIER_ID");
        }
    }
}