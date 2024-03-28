namespace Bars.GkhGji.Migrations._2022.Version_2022062200
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022062200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022061501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("INF_AMMOUNT_ARREAR_PAY_CONT_MAJOR_REPAIRS", DbType.Decimal, ColumnProperty.None, 0));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("AMMOUNT_DEBT_CREDIT", DbType.Decimal, ColumnProperty.None, 0));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_SPECIAL_ACCOUNT_ROW", "AMMOUNT_DEBT_CREDIT");
            Database.RemoveColumn("GJI_SPECIAL_ACCOUNT_ROW", "INF_AMMOUNT_ARREAR_PAY_CONT_MAJOR_REPAIRS");
        }
    }
}