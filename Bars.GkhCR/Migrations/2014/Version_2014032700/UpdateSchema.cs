namespace Bars.GkhCr.Migrations.Version_2014032700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014032600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PER_ACT_PAYMENT", new Column("SUM_PAID", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PER_ACT_PAYMENT", "SUM_PAID");
        }
    }
}