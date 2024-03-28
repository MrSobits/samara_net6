namespace Bars.GkhGji.Migrations.Version_2014010800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014010800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013122600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", "INT_NUMBER", DbType.Int32, ColumnProperty.NotNull, 0);
            Database.AddColumn("GJI_APPEAL_CITIZENS", "INT_SUBNUMBER", DbType.Int32, ColumnProperty.NotNull, 0);

            Database.ExecuteNonQuery("update GJI_APPEAL_CITIZENS set ACCEPTING = 0 where accepting = 10");
            Database.ExecuteNonQuery("update GJI_APPEAL_CITIZENS set ACCEPTING = 10 where accepting = 20");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "INT_NUMBER");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "INT_SUBNUMBER");
        }
    }
}