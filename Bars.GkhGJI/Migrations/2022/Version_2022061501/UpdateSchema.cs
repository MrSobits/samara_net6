namespace Bars.GkhGji.Migrations._2022.Version_2022061501
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022061501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022061500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("TARIFF", DbType.Decimal, ColumnProperty.None, 10));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_SPECIAL_ACCOUNT_ROW", "TARIFF");
        }
    }
}