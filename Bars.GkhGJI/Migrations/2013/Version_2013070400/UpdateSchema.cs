namespace Bars.GkhGji.Migrations.Version_2013070400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013070400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013070200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_VIOL_STAGE", new Column("DATE_PLAN_REMOVAL", DbType.DateTime));
            Database.AddColumn("GJI_INSPECTION_VIOL_STAGE", new Column("DATE_FACT_REMOVAL", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_VIOL_STAGE", "DATE_PLAN_REMOVAL");
            Database.RemoveColumn("GJI_INSPECTION_VIOL_STAGE", "DATE_FACT_REMOVAL");
        }
    }
}