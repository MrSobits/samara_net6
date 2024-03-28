namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014032000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("DEC_MONTHLY_FEE_HISTORY",
                new Column("DECISION_VALUE", DbType.String, 1000, ColumnProperty.Null),
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "DEC_MONTHLY_FEE_PROT", "GKH_OBJ_D_PROTOCOL", "ID"),
                new Column("USER_NAME", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_MONTHLY_FEE_HISTORY");
        }
    }
}