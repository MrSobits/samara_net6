namespace Bars.GisIntegration.Base.Migrations.Version_2016102800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016102800")]
    [MigrationDependsOn(typeof(Version_2016101900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveColumn("GI_TASK", "DATA_SUPPLIER_ID");
            this.Database.AddColumn(
                    "GI_PACKAGE",
                    new Column("IS_DELEGACY", DbType.Boolean, ColumnProperty.NotNull, this.Database.DatabaseKind == DbmsKind.Oracle ? 0 : (object)false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GI_PACKAGE", "IS_DELEGACY");
            this.Database.AddRefColumn("GI_TASK", new RefColumn("DATA_SUPPLIER_ID", ColumnProperty.Null, "TASK_DATA_SUP", "GI_CONTRAGENT", "ID"));
        }
    }
}