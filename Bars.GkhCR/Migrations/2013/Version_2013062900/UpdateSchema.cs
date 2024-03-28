namespace Bars.GkhCr.Migrations.Version_2013062900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013062900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013060400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PROTOCOL", new Column("SUM_ACT_VER_OF_COSTS", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PROTOCOL", "SUM_ACT_VER_OF_COSTS");
        }
    }
}