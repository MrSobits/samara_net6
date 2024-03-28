namespace Bars.GkhGji.Migrations.Version_2014012901
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014012900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_INSPECTION_VIOL_STAGE", "SUM_AMOUNT_REMOVAL");
            Database.RemoveColumn("GJI_INSPECTION_VIOLATION", "SUM_AMOUNT_REMOVAL");
            Database.AddColumn("GJI_INSPECTION_VIOLATION", new Column("SUM_AMOUNT_REMOVAL", DbType.Decimal.WithSize(8, 2), ColumnProperty.Null, null));
            Database.AddColumn("GJI_INSPECTION_VIOL_STAGE", new Column("SUM_AMOUNT_REMOVAL", DbType.Decimal.WithSize(8, 2), ColumnProperty.Null, null));
        }

        public override void Down()
        {
            
        }
    }
}