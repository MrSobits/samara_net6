namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2022082200
{
    using System.Data;

    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022082200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022071100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("YEAR_CALC", DbType.Int32, ColumnProperty.NotNull, 2022));
            Database.ExecuteNonQuery("UPDATE OVRHL_VERSION_REC SET YEAR_CALC = YEAR");          
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "YEAR_CALC");
        }
    }
}