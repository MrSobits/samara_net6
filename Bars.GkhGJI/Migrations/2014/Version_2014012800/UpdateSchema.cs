namespace Bars.GkhGji.Migrations.Version_2014012800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014010800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_VIOLATION", new Column("SUM_AMOUNT_REMOVAL", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m));
            Database.AddColumn("GJI_INSPECTION_VIOL_STAGE", new Column("SUM_AMOUNT_REMOVAL", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_VIOL_STAGE", "SUM_AMOUNT_REMOVAL");
            Database.RemoveColumn("GJI_INSPECTION_VIOLATION", "SUM_AMOUNT_REMOVAL");
        }
    }
}