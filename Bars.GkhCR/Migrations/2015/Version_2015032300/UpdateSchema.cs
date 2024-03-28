namespace Bars.GkhCr.Migrations.Version_2015032300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015031600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_DEFECT_LIST", "TYPE_DEFECT_LIST", DbType.Int32, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_DEFECT_LIST", "TYPE_DEFECT_LIST");
        }
    }
}