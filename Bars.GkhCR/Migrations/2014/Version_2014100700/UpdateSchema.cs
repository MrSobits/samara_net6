namespace Bars.GkhCr.Migrations.Version_2014100700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014100600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PER_ACT_PAYMENT", "IS_CANCELLED", DbType.Boolean, ColumnProperty.NotNull, defaultValue: false);
            Database.AddColumn("CR_OBJ_PER_ACT_PAYMENT", "TRANSFER_GUID", DbType.String, 40);
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PER_ACT_PAYMENT", "IS_CANCELLED");
            Database.RemoveColumn("CR_OBJ_PER_ACT_PAYMENT", "TRANSFER_GUID");
        }
    }
}