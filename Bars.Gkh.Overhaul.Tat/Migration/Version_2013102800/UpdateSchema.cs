namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013102800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013102601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "COMMON_ESTATE_ID");

            Database.AddRefColumn("OVRHL_STAGE2_VERSION", 
                new RefColumn("COMMON_ESTATE_ID", ColumnProperty.Null, "ST2VERSION_CE", "OVRHL_COMMON_ESTATE_OBJECT", "ID"));

            Database.AddColumn("OVRHL_SM_RECORD_VERSION", "DEFICIT_AFTER", DbType.Decimal, ColumnProperty.NotNull, 0);
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SM_RECORD_VERSION", "DEFICIT_AFTER");
            Database.RemoveColumn("OVRHL_STAGE2_VERSION", "COMMON_ESTATE_ID");
        }
    }
}