namespace Bars.GkhGji.Regions.Tyumen.Migrations.Version_2015022500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tyumen.Migrations.Version_2015021700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_NETWORK_OPERATOR",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "NOPERATOR_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                new Column("DESCRIPTION", DbType.String, 3000, ColumnProperty.Null));

            Database.AddEntityTable("GKH_DICT_TECH_DECISION",
                new Column("NAME", DbType.String, 200, ColumnProperty.NotNull));

            Database.AddEntityTable("GKH_NOP_RO",
                new RefColumn("NOPERATOR_ID", ColumnProperty.NotNull, "NOP_NOP_RO", "GKH_NETWORK_OPERATOR", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_NOP_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("BANDWIDTH", DbType.String, 100, ColumnProperty.NotNull));

            Database.AddEntityTable("GKH_NOP_RO_TDEC",
                new RefColumn("NOP_RO_ID", ColumnProperty.NotNull, "NOP_RO_NOP_RO_TDEC", "GKH_NOP_RO", "ID"),
                new RefColumn("TDEC_ID", ColumnProperty.NotNull, "TDEC_NOT_RO_TDEC", "GKH_DICT_TECH_DECISION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_NOP_RO_TDEC");
            Database.RemoveTable("GKH_NOP_RO");
            Database.RemoveTable("GKH_NETWORK_OPERATOR");
            Database.RemoveTable("GKH_DICT_TECH_DECISION");
        }
    }
}