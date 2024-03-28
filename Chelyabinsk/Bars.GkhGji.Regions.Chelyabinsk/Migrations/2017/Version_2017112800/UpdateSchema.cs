namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017112800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017112800")]
    [MigrationDependsOn(typeof(Version_2017111600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Категория подателей заявления
            Database.AddEntityTable(
                "GJI_CH_EFFECTIVE_INDEX",
                 new Column("TYPE_KND", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                 new Column("YEAR_ENUM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                 new Column("CURRENT_INDEX", DbType.Decimal, ColumnProperty.None),
                 new Column("TARGET_INDEX", DbType.Decimal, ColumnProperty.None),
                 new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                 new Column("NAME", DbType.String, 300, ColumnProperty.None));

         
        }
        public override void Down()
        {
          Database.RemoveTable("GJI_CH_EFFECTIVE_INDEX");
        }
    }
}