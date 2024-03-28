namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2022101700
{
    using System.Data;

    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022101700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022082200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("ADDIT_EXPENCES", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.ExecuteNonQuery("UPDATE OVRHL_SUBSIDY_REC_VERSION SET ADDIT_EXPENCES = 0");          
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "YEAR_CALC");
        }
    }
}