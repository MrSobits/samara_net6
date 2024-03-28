namespace Bars.GkhCr.Migrations.Version_2013122400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_DEFECT_LIST", new Column("SUM", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_DEFECT_LIST", "SUM");
        }
    }
}