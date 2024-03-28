namespace Bars.GkhCr.Migrations.Version_2014021000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014012100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_OBJ_PER_ACT_PAYMENT",
                new RefColumn("ACT_ID", ColumnProperty.NotNull, "CR_PER_ACT_PAYMENT", "CR_OBJ_PERFOMED_WORK_ACT", "ID"),
                new Column("TYPE_ACT_PAYMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DATE_PAYMENT", DbType.Date),
                new Column("SUM",  DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PERCENT", DbType.Decimal, ColumnProperty.NotNull, 0m)
             );
        }

        public override void Down()
        {
            Database.RemoveTable("CR_OBJ_PER_ACT_PAYMENT");
        }
    }
}